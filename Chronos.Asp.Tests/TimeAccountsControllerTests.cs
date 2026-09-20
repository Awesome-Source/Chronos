using System.Net;
using System.Net.Http.Json;
using System.Text;
using Chronos.Asp.Contracts;

namespace Chronos.Asp.Tests
{
    /// <summary>
    /// The in-memory database is shared by the whole process (see SqliteInMemoryDataBaseAccessor), so these tests
    /// must not run in parallel and must not assume an empty database. Every test works with uniquely named accounts.
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public sealed class TimeAccountsControllerTests
    {
        private const string BaseRoute = "api/timeaccounts";

        private static ChronosWebApplicationFactory _factory = null!;
        private static HttpClient _client = null!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _factory = new ChronosWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkWithList()
        {
            var response = await _client.GetAsync(BaseRoute);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var accounts = await response.Content.ReadFromJsonAsync<List<TimeAccountDto>>();
            Assert.IsNotNull(accounts);
        }

        [TestMethod]
        public async Task Create_ReturnsNoContent_AndAccountIsListed()
        {
            var name = UniqueName();

            var response = await _client.PostAsJsonAsync(BaseRoute, new CreateTimeAccountRequest(name, "#112233", true));

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            var created = await FindByNameAsync(name);
            Assert.IsNotNull(created);
            Assert.AreEqual("#112233", created.Color);
            Assert.IsTrue(created.IsWorkTime);
        }

        [TestMethod]
        public async Task Update_ChangesStoredValues()
        {
            var account = await CreateAndFindAsync(UniqueName(), "#112233", true);
            var newName = UniqueName();

            var response = await _client.PutAsJsonAsync($"{BaseRoute}/{account.Id}", new UpdateTimeAccountRequest(newName, "#445566", false));

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            var accounts = await GetAllAsync();
            Assert.IsFalse(accounts.Any(a => a.Name == account.Name));

            var updated = accounts.Single(a => a.Id == account.Id);
            Assert.AreEqual(newName, updated.Name);
            Assert.AreEqual("#445566", updated.Color);
            Assert.IsFalse(updated.IsWorkTime);
        }

        [TestMethod]
        public async Task Remove_DeletesAccount()
        {
            var account = await CreateAndFindAsync(UniqueName(), "#112233", false);

            var response = await _client.DeleteAsync($"{BaseRoute}/{account.Id}");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            var accounts = await GetAllAsync();
            Assert.IsFalse(accounts.Any(a => a.Id == account.Id));
        }

        [TestMethod]
        public async Task Create_WithMalformedJson_ReturnsBadRequest()
        {
            using var content = new StringContent("this is not json", Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(BaseRoute, content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static string UniqueName()
        {
            return $"Account-{Guid.NewGuid():N}";
        }

        private static async Task<List<TimeAccountDto>> GetAllAsync()
        {
            var accounts = await _client.GetFromJsonAsync<List<TimeAccountDto>>(BaseRoute);
            Assert.IsNotNull(accounts);

            return accounts;
        }

        private static async Task<TimeAccountDto?> FindByNameAsync(string name)
        {
            var accounts = await GetAllAsync();
            return accounts.SingleOrDefault(a => a.Name == name);
        }

        private static async Task<TimeAccountDto> CreateAndFindAsync(string name, string colorHex, bool isWorkTime)
        {
            var response = await _client.PostAsJsonAsync(BaseRoute, new CreateTimeAccountRequest(name, colorHex, isWorkTime));
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var created = await FindByNameAsync(name);
            Assert.IsNotNull(created);

            return created;
        }

        // The core TimeAccount cannot be deserialized directly (constructor parameter "internalId" does not match the JSON property "id").
        private sealed record TimeAccountDto(int Id, string Name, string Color, bool IsWorkTime);
    }
}
