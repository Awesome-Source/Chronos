namespace Apollo.Core.Interfaces
{
    public interface IRowParser
    {
        string GetString(string columnName);
        string? GetNullableString(string columnName);
        short GetShort(string columnName);
        short? GetNullableShort(string columnName);
        int GetInt(string columnName);
        int? GetNullableInt(string columnName);
        long GetLong(string columnName);
        DateTime GetDateTime(string columnName);
        bool GetBool(string columnName);
    }
}
