namespace SalesForceBackup.Interfaces
{
    public interface IAppSettings
    {
        string Get(string key);
        void Set(string key, string value);
    }
}
