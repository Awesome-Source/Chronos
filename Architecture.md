# Chronos – Architecture Overview

Starting-point document for future contributors. It describes the solution layout, layering, database schema, key classes, runtime flows, conventions and known quirks. Verify against the code before relying on details – this is a snapshot, not a contract.

## 1. What Chronos is

Chronos is a small, single-user, locally hosted web app for tracking the time spent on activities during the day. An ASP.NET Core backend serves a JSON API and a framework-free static frontend. Data lives in a local SQLite file. There is no authentication, no tests, no logging framework and no build step for the frontend (see `README.md` for the status list and TODOs).

- Runtime: **.NET 10** (`net10.0`), nullable reference types and implicit usings enabled everywhere.
- Persistence: SQLite via `Microsoft.Data.Sqlite` (raw SQL, no ORM).
- Frontend: vanilla JS/HTML/CSS in `wwwroot`, no bundler, no npm, no framework.

## 2. Solution layout

`Chronos.sln` contains four projects. Dependency direction is strictly downward:

```
Chronos.Asp   (Sdk.Web)      HTTP API + static frontend + composition root
    └─► Chronos.Core          domain services, repositories, DB patches
            └─► Apollo.Sqlite     SQLite implementation of the Apollo abstractions
                    └─► Apollo.Core   (folder: Apollo/) DB abstractions + patch-based initializer
```

| Project (folder) | Assembly / root namespace | Purpose |
|---|---|---|
| `Apollo/` | `Apollo.Core` | Generic, Chronos-agnostic DB abstractions (`IDatabaseAccessor`, `IRowParser`, …) and `DatabaseInitializer`, a numbered-patch schema migrator. Meant to be reusable. |
| `Apollo.Sqlite/` | `Apollo.Sqlite` | SQLite implementation of the Apollo interfaces plus a foreign-key checker. Only project referencing `Microsoft.Data.Sqlite`. |
| `Chronos.Core/` | `Chronos.Core` | The Chronos domain: data objects, service + repository interfaces and implementations, DB patches, DI registration. References `Microsoft.Extensions.DependencyInjection`. |
| `Chronos.Asp/` | `Chronos.Asp` | Web host: `Program.cs`, controllers, request records, exception middleware, JSON converter, `wwwroot/` frontend. |

Note the folder/project-name mismatch: folder `Apollo/` ↔ project `Apollo.Core.csproj` ↔ namespace `Apollo.Core`.

## 3. Layering inside Chronos.Core

```
Controller  ──►  I*Service  ──►  I*Repository  ──►  IDatabaseAccessor (Apollo)  ──►  SQLite
(Chronos.Asp)    (public)         (internal*)          (transient, per repo)
```

- `Chronos.Core/Contracts/DataObjects` – plain public DTO/domain classes (immutable-ish, ctor-populated). Serialized directly to JSON by the controllers.
- `Chronos.Core/Contracts/Services` – **public** service interfaces (what the web layer sees).
- `Chronos.Core/Contracts/Repositories` – repository interfaces, **`internal`** except `IStatisticsRepository` (public).
- `Chronos.Core/Implementations/{Services,Repositories}` – **`internal`** implementations, so the web layer can only reach the services.
- `Chronos.Core/Implementations/Database` – `DatabaseConnectionConfiguration` and `Patches/`.
- `Chronos.Core/Extensions/ConversionExtensions.cs` – `bool ↔ int (0/1)` and `TimeOnly ↔ seconds since midnight` helpers used by the repositories.
- Most services are thin pass-throughs to their repository. Real logic lives in `TrackingService` (validation, "no tracking day → empty list") and `StatisticsService` (week math, proportions), plus SQL in `TrackingRecordRepository` / `TrackingTargetRepository`.

### Composition / startup

- `ServiceCollectionExtensions.AddChronosCore(services, appDataDirectory)` registers everything: `IDatabaseAccessor` **transient** (`SqliteDataBaseAccessor`), everything else **singleton**, including `ChronosCore`.
- `ChronosCore` is a facade holding all services; its `Initialize()` builds the patch list (`InitialPatch`, `Patch1Categories`, `Patch2DropActivityCategoryId`, `Patch3ObjectiveIsDone`), runs `DatabaseInitializer.Run`, then calls `TrackingService.CompleteActiveEntryInPastIfExisting(today)`.
- `Program.Main` (Chronos.Asp): `AddControllers` (+ `TimeSpanSecondsJsonConverter`), resolves the app-data dir, `AddChronosCore`, builds the app, calls `ChronosCore.Initialize()` **before** serving requests (an invalid/unpatchable DB throws `DatabaseInitializationException` and the app does not start), then `UseDefaultFiles` → `UseStaticFiles` (Dev: `Cache-Control: No-Store`; otherwise `max-age=300`) → `UseExceptionHandler(ExceptionHandling.Configure)` → `UseAuthorization` → `MapControllers`.
- Data directory: `%APPDATA%\Chronos\` (created if missing); DB file `chronos.db`. Connection string: `Data Source=<path>;Mode=ReadWriteCreate;Foreign Keys=True`.
- Dev URL: `http://localhost:5209` (`Properties/launchSettings.json`, gitignored).
- Run: `dotnet run --project Chronos.Asp` (Development profile) – "Checkout + compile" is the deployment story.

## 4. Apollo (DB abstraction layer)

Interfaces in `Apollo/Interfaces`:

- `IWithinTransactionExecutor` – `ExecuteNonQuery`, `ExecuteQuery<T>(sql, Func<IRowParser,T>, Dictionary<string,object>? parameters)`.
- `IConnectionExecutor` – same on one connection, plus `ExecuteInTransaction(Action|Func<IWithinTransactionExecutor,…>)`.
- `IDatabaseAccessor : IConnectionExecutor` – adds `ExecuteOnSingleConnection(Action<IConnectionExecutor>)` for connection-scoped state such as PRAGMAs.
- `IRowParser` – typed column getters by name (`GetInt`, `GetNullableInt`, `GetString`, `GetBool`, `GetLong`, `GetDateTime`, …).
- `IDatabasePatch` – `PatchMetaInfo` (number + comment), `BeforeExecution(IConnectionExecutor)`, `Execute(IWithinTransactionExecutor)`, `AfterExecution(IConnectionExecutor)`.
- `IPatchInfoRepository`, `IDatabaseConnectionConfiguration` (`ConnectionString`).

`DatabaseInitializer.Run(patches)`:

1. Patches must be numbered `0..n` in order, no gaps (`EnsureAllPatchesAreInOrderWithoutGap`).
2. Loads installed patches from `patch_installations` (the table is created on demand by `PatchInfoRepository`); installed numbers must be contiguous from 0; more installed patches than known ⇒ "application may be outdated".
3. Installs each missing patch on a **single connection**: `BeforeExecution` → transaction { `Execute` + register in `patch_installations` } → `AfterExecution` (in `finally`).
4. Any failure throws `DatabaseInitializationException`. Progress is written with `Console.WriteLine` (no logger yet).

`Apollo.Sqlite` implementation notes:

- `SqliteDataBaseAccessor`: every call opens its own connection from the Microsoft.Data.Sqlite pool (`Pooling = true`). The constructor runs `PRAGMA journal_mode = WAL` (persisted in the file).
- `SqliteConnectionExecutor`: `ExecuteInTransaction` uses `BeginTransaction(deferred: false)` (write lock up front); `ExecuteQuery` uses a deferred transaction so pure reads stay parallel. Rolls back and rethrows on exception. `ExecuteNonQuery` on this class runs **without** a transaction.
- `SqliteWithinTransactionExecutor` / `SqliteRowParser`: command creation, `Prepare()` when parameters are given, column-name→index lookup per query.
- `ForeignKeyChecker.Execute(executor)`: runs `PRAGMA foreign_key_check` and throws `ForeignKeyCheckFailedException` (carries `ForeignKeyCheckResult` records). Used at the end of table-rebuilding patches.

## 5. Domain model & database schema

### Concepts

- **TimeAccount** – a bucket of time with a colour; `is_worktime` marks it as productive. Statistics only count work-time accounts.
- **Activity** – *what* you do (belongs to exactly one time account).
- **Category** – groups objectives. Category id 1 is the seeded placeholder `-`.
- **Objective** – *why*/the goal (name, description, category, `is_done`).
- **TrackingDay** – a calendar day that has tracking data (created lazily).
- **TrackingTarget** – a (day, activity, objective, planned?) combination you can track against that day. Time accounts are reached via the activity.
- **TrackingRecord** – a completed interval (start/end/duration) against a target.
- **ActiveTrackingRecord** – the single currently running interval (start only). At most one row is ever expected.

### Schema (SQLite, all tables `STRICT`, FKs enforced, no `ON DELETE CASCADE`)

```
time_accounts        (id PK AI, name TEXT, color TEXT, is_worktime INTEGER)
categories           (id PK AI, name TEXT)                       -- seeded: (1,'-')
activities           (id PK AI, name TEXT,
                      time_account_id → time_accounts.id)
objectives           (id PK AI, name TEXT, description TEXT,
                      category_id → categories.id,
                      is_done INTEGER NOT NULL DEFAULT 0)
tracking_days        (id PK AI, year INTEGER, month INTEGER, day INTEGER,
                      UNIQUE(year, month, day))
tracking_targets     (id PK AI,
                      tracking_day_id → tracking_days.id,
                      activity_id → activities.id,
                      objective_id → objectives.id,
                      is_planned INTEGER)
tracking_records     (id PK AI,
                      tracking_target_id → tracking_targets.id,
                      start_time INTEGER, end_time INTEGER,      -- seconds since midnight
                      duration INTEGER)                          -- seconds, = end - start
active_tracking_record (id PK AI,
                      tracking_target_id → tracking_targets.id,
                      start_time INTEGER)                        -- seconds since midnight
patch_installations  (patch_number INT PK, installation_date TEXT, patch_comment TEXT)
```

Relationship sketch:

```
time_accounts 1─* activities 1─* tracking_targets *─1 objectives *─1 categories
                                      │ *─1 tracking_days
                                      ├─* tracking_records
                                      └─* active_tracking_record (≤1 row)
```

Conventions:

- Booleans are `INTEGER` 0/1 (`ToIntRepresentation` / `ToBoolFromIntRepresentation`). Note the column `is_worktime` vs. C# `IsWorkTime`.
- Times of day are **seconds since midnight** (`ToSecondsSinceMidnight` / `FromSecondsSinceMidnight`); dates are split into `year/month/day` columns (statistics compare via `year*10000+month*100+day`). A record cannot cross midnight.
- Inserts use `INSERT … RETURNING id` through `ExecuteQuery` to get generated ids.
- Foreign-key violations on delete surface as SQLite error 19 / `ForeignKeyCheckFailedException` → HTTP 409.

### Patch history (`Chronos.Core/Implementations/Database/Patches`)

| # | Class | Change |
|---|---|---|
| 0 | `InitialPatch` | Creates all base tables. |
| 1 | `Patch1Categories` | Adds `categories` (+ seed `-`), rebuilds `activities`/`objectives` with `category_id` (FKs off around it, `ForeignKeyChecker` afterwards). |
| 2 | `Patch2DropActivityCategoryId` | Removes the accidental NOT NULL `activities.category_id` (table rebuild – SQLite can't drop a column referenced by an FK). |
| 3 | `Patch3ObjectiveIsDone` | `ALTER TABLE objectives ADD COLUMN is_done INTEGER NOT NULL DEFAULT 0`. |

**How to add a schema change:** create `PatchNCommentName : IDatabasePatch` (internal) with the next number, append it to the list in `ChronosCore.Initialize()`. Never edit an already-released patch. For anything SQLite's `ALTER TABLE` can't do, follow the Patch1/Patch2 pattern: `BeforeExecution` sets `PRAGMA FOREIGN_KEYS = OFF`, `Execute` creates `patch_<table>`, copies, drops, renames, then `ForeignKeyChecker.Execute`, `AfterExecution` sets it back `ON`. Patches currently run without a backup of the DB file (open TODO in `README.md`).

## 6. Important classes (Chronos.Core)

Data objects (`Contracts/DataObjects`): `TimeAccount`, `Activity` (with `TimeAccountName`), `Category`, `Objective` (with `CategoryName`, `IsDone`), `TrackingRecord` (joined with activity/objective names, `IsActive` flag), `EvaluatedTrackingTarget` (target + accumulated time + `IsActive`; the `AccumulatedTime`/`IsActive` setters are `internal`), `TimeAccountBalance` / `RelativeTimeAccountBalance` (adds `Proportion` 0..1), `DailyTimeAccountDuration`, `TimeAccountDuration`, `DailyTimeAccountBreakdown`; `ActiveTrackingRecord` is `internal`.

Services (public interfaces in `Contracts/Services`):

- `ITimeAccountService`, `IActivityService`, `ICategoryService`, `IObjectiveService` – CRUD (`Create`/`GetAll`/`Update`/`Remove`; `IObjectiveService.Create` returns the new id).
- `ITrackingService` – the core workflow:
  - `CreateTarget(date, activityId, objectiveId, isPlanned)` → get-or-create tracking day, insert target.
  - `StartTracking(targetId, start)` → `CompleteActiveRecordAndStartNew` (closes the running record at `start`, opens the new one, atomically). `StopTracking(end)` → closes the running record.
  - `GetEvaluatedTrackingTargetsForDay(DateTime now)` → today's targets with accumulated time; merges the running record's elapsed time (`now − start`) and flags `IsActive`.
  - `GetTrackingTargetsForDay(date)` / `GetTimeSheetForDay(date)` → targets from completed records only (currently both call the same repository method).
  - `GetRecordsForDay`, `AddRecord` (throws `ArgumentException` if `end <= start` → HTTP 400), `UpdateRecord`, `RemoveRecord`.
  - `CompleteActiveEntryInPastIfExisting(date)`, `TryGetLatestTrackingDayBefore(date, out date)`.
- `IStatisticsService` – all-time productive balances, current-week (Monday-start) balances and per-day breakdown, each with relative proportions.

Repositories worth knowing: `TrackingRecordRepository` (transactional start/stop/complete logic; `GetRecordsForDay` `UNION ALL`s completed records with the active one), `TrackingTargetRepository` (evaluated-target SQL, merging of the active target), `TrackingDayRepository`, `StatisticsRepository` (aggregation SQL), `PatchInfoRepository`.

## 7. HTTP API (Chronos.Asp)

Controllers are `[ApiController]`, thin, and return service results directly. Request bodies are records in `Contracts/Requests.cs`. `TimeSpan` values are serialized as **total seconds (number)** by `TimeSpanSecondsJsonConverter`; `DateOnly` as `yyyy-MM-dd`, `TimeOnly` as `HH:mm:ss`. JSON property names are camelCase (ASP.NET default).

| Route | Verbs | Notes |
|---|---|---|
| `api/timeaccounts[/{id}]` | GET, POST, PUT, DELETE | `{name, colorHex, isWorkTime}` |
| `api/activities[/{id}]` | GET, POST, PUT, DELETE | `{name, timeAccountId}` |
| `api/objectives[/{id}]` | GET, POST (returns `{id}`), PUT, DELETE | `{name, description, categoryId[, isDone]}` |
| `api/categories[/{id}]` | GET, POST, PUT, DELETE | `{name}` |
| `api/tracking/targets/today` | GET | evaluated targets incl. running one (`isActive`) |
| `api/tracking/targets?date=` | GET | targets of a day |
| `api/tracking/targets` | POST | `{activityId, objectiveId, isPlannedActivity, date?}` → `{id}`; date defaults to today |
| `api/tracking/targets/{id}/start` | POST | `{start}` – switches from any running target |
| `api/tracking/stop` | POST | `{end}` |
| `api/tracking/records?date=` | GET | |
| `api/tracking/records[/{id}]` | POST, PUT, DELETE | `{trackingTargetId, start, end}` / `{start, end}` |
| `api/tracking/timesheet?date=` | GET | |
| `api/tracking/latest-day-before?date=` | GET | `{date}` or 404 |
| `api/statistics/productive-time-account-balances` | GET | all time |
| `api/statistics/productive-time-account-balances/current-week` | GET | |
| `api/statistics/daily-time-account-durations/current-week` | GET | |

Error handling (`Middleware/ExceptionHandling.cs`, registered via `UseExceptionHandler`): responds `application/problem+json` with `ProblemDetails.Title` – `ForeignKeyCheckFailedException` / SQLite error 19 → **409** "Cannot delete: this item is still referenced…", `ArgumentException` → **400** (message), anything else → **500** generic. The frontend surfaces `title` as a toast.

Server "now" is used for "today" in `/targets/today` and the statistics endpoints; the client sends its own times for start/stop/records.

## 8. Frontend (`Chronos.Asp/wwwroot`)

Single-page app: `index.html` holds the shell (sidebar nav, status bar, one `div.page` per view, two modals, toast container) and loads plain `<script>` files in a fixed order – **globals, not ES modules** (`icons → format → dom → charts → tabledropdown → state → api → views/* → app`). Handlers are attached with inline `onclick="…"`. Fonts (Manrope, JetBrains Mono) come from Google Fonts; there are no other external dependencies. Theme/styling: `css/style.css` (CSS variables).

| File | Role |
|---|---|
| `js/app.js` | Orchestrator: `PAGES`/`PAGE_RENDERERS`, `showPage`, modal + toast helpers, `refreshMasterDataCache`, status bar, 10 s poll (`pollActiveTracking`: status bar, plus tracking table/dashboard when visible), `init` on `DOMContentLoaded`. |
| `js/state.js` | Mutable UI state (`currentPage`, selected dates/tabs), the master-data `cache`, `masterView` sort/filter state. Volatile tracking data is never cached – it is re-fetched per page show. |
| `js/api.js` | **Only** place with route strings/`fetch`. `apiRequest` throws `Error(problem.title)` on non-2xx; exposes the `Api.*` object. |
| `js/format.js` | Date/time/duration formatting (`formatDuration`, `dateToInputValue`, `escapeHtml`, `TODAY`/`YESTERDAY`, …). |
| `js/dom.js` | `patchInnerHtml` – in-place DOM patcher (matches by `data-key`, else position) so polling refreshes don't flicker; **not for regions with form controls**. |
| `js/charts.js` | Dependency-free SVG charts (`donutChartHtml`, stacked bars); colours from time-account colour with fallback palette. |
| `js/tabledropdown.js` | `TableDropdown` class – select-like control with a filterable table panel (used in the new-target/new-record modals). |
| `js/icons.js` | Inline SVG icon strings. |
| `js/views/dashboard.js` | Donuts (all-time + current week) and per-day stacked bar chart. |
| `js/views/tracking.js` | Clock in/out screen: today's targets, start selected / stop, "new tracking target" modal (existing or new objective). |
| `js/views/records.js` | Raw record editor per day, gap/overlap conflict detection with "align to neighbour" fixes, "add record" modal. |
| `js/views/timesheet.js` | Read-only report for a past day (defaults to latest tracked day before today). |
| `js/views/statistics.js` | Placeholder list of planned reports ("Coming soon"). |
| `js/views/masterdata.js` | Inline-edit CRUD tables for accounts/activities/objectives/categories with client-side sort + filter. Delete failures come from the server as toasts. |

Frontend conventions: 2-space indent, single quotes, `async/await`, functions named `renderX` / `refreshX`, HTML built as template strings and escaped with `escapeHtml`. The client sends `HH:MM:SS` (`timeInputToTimeOnlyString`) and `yyyy-MM-dd` strings.

## 9. Key runtime flows

- **Start/switch tracking:** UI → `POST /tracking/targets/{id}/start {start}` → `TrackingRecordRepository.CompleteActiveRecordAndStartNew` in one transaction: if a row exists in `active_tracking_record`, insert a completed `tracking_records` row (start = old start, end = new start) and replace the active row; otherwise just insert the active row.
- **Stop:** `POST /tracking/stop {end}` → completes the active row into `tracking_records` and clears `active_tracking_record`.
- **Live timer:** the running interval is not stored as a record; `TrackingTargetRepository` computes `evaluationTime − start_time` for it and merges it into the target's total. The frontend polls every 10 s and re-renders the status bar/tables.
- **Day rollover (partial):** at startup `CompleteActiveEntryInPastIfExisting` closes an active record belonging to an earlier day at `23:59:59`. There is no live rollover while the app keeps running (README TODO: "Complete active records on day change").
- **Adding a target:** `TrackingService.CreateTarget` → `GetOrCreateTrackingDay(date)` (lazily creates the `tracking_days` row) → insert `tracking_targets`.
- **Statistics:** SQL aggregates `tracking_records.duration` via targets → activities → time accounts; `StatisticsService` filters to work-time accounts for balances, groups per day, and computes proportions. Only completed records count (the running one is excluded).

## 10. Coding conventions

C# (Core/Apollo/Asp):

- 4-space indent, Allman braces, `_camelCase` private readonly fields, constructor injection, file-per-type. Files use block-scoped `namespace X { }`; many start with a UTF-8 BOM – match the file you are editing.
- SQL is inline in repositories, parameterised with `@UPPER_SNAKE` names via `Dictionary<string, object>`; results parsed with a `ParseX(IRowParser)` method; ids returned with `RETURNING id`.
- Domain classes are constructor-populated with get-only properties (`{ get; }`) unless the field is editable (`{ get; set; }`); `Equals`/`GetHashCode` by `Id` on `TimeAccount`/`Activity`.
- Repository interfaces and all implementations are `internal`; only service interfaces and data objects are public (add `public` deliberately if the web layer needs more).
- New feature checklist: DTO in `Contracts/DataObjects` → repository interface + implementation (+ DI registration in `ServiceCollectionExtensions`) → service interface + implementation (+ registration, and expose on `ChronosCore` if desired) → request records in `Chronos.Asp/Contracts/Requests.cs` → controller → `Api.*` method in `api.js` → view code. Schema change ⇒ new patch (§5).

## 11. Known quirks, gaps and gotchas

- **No tests, no logging** (`Console.WriteLine` and `//TODO log exception` markers stand in); no backup before DB patches.
- **No auth / single user:** the API is unauthenticated; intended for local use (`AllowedHosts: *`).
- **Day rollover** only handled at startup, and only if *today's* tracking day already exists (`CompleteActiveEntryInPastIfExisting` returns early when `TryGetTrackingDay(today)` fails). A stale active record from yesterday is therefore not necessarily closed until a target exists for today.
- **`TryGetLatestTrackingDayBefore`** orders by `id DESC` (creation order), not by date, and merely excludes the given date – so it can return a *later* day or a mis-ordered result if days were created out of chronological order (e.g. backfilled).
- **Record ids in the Records view:** the active record is returned with the id of the `active_tracking_record` row, which lives in a different id space than `tracking_records.id`. Don't treat an `isActive` record id as editable/deletable through `/tracking/records/{id}`.
- **`GetTrackingTargetsForDay` and `GetTimeSheetForDay`** currently share the same repository call (completed records only).
- **Deleting referenced master data** fails with 409 (no cascades); the UI relies on the server for that check.
- **Midnight-crossing intervals** are not representable (times are seconds since midnight, `TimeOnly`).
- **Localization, statistics page, dashboard polish** are open items (README TODOs); the Statistics page is a placeholder.
- `Apollo.Core.DatabaseInitializer` treats "patch numbers must equal list index" as an invariant – renumbering or removing patches breaks existing databases.
- `SqliteDataBaseAccessor`'s constructor executes `PRAGMA journal_mode = WAL`; because the accessor is registered as transient, each repository triggers this once at construction (harmless but worth knowing).
- The `.vs/`, `bin/`, `obj/` and `Properties/launchSettings.json` are not tracked.
