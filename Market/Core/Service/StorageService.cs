using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Market.Service;

public class StorageService
{
    public class SettingsInfo
    {
        public int? UserId { get; set; }
    }

    public SettingsInfo GetData()
    {
        try
        {
            var store = IsolatedStorageFile.GetUserStoreForAssembly();
            SettingsInfo? settings;
            using (var stream = store.OpenFile("settings.cfg", FileMode.OpenOrCreate, FileAccess.Read))
            {
                settings = JsonSerializer.Deserialize<SettingsInfo>(stream);
                if (settings == null)
                {
                    return new SettingsInfo();
                }
            }
            return settings;
        }
        catch(Exception e)
        {
            Console.Write(e);
            return new SettingsInfo();
        }
    }
    
    public void SetData(SettingsInfo settingsInfo)
    {
        try
        {
            var store = IsolatedStorageFile.GetUserStoreForAssembly();
            var stream = new StreamWriter(store.OpenFile("settings.cfg", FileMode.Truncate, FileAccess.Write));
            var str = JsonSerializer.Serialize(settingsInfo);
            stream.Write(str);
            stream.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}