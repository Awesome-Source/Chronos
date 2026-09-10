using Apollo.Core.Interfaces;

namespace Apollo.Sqlite
{
    public record ForeignKeyCheckResult(string TableName, long RowId, string ReferredTableName, long ForeignKeyId)
    {
        public static ForeignKeyCheckResult ParseFromPragma(IRowParser rowParser)
        {
            return new ForeignKeyCheckResult(rowParser.GetString("table"), rowParser.GetLong("rowid"), rowParser.GetString("parent"), rowParser.GetLong("fkid"));
        }
    }
}
