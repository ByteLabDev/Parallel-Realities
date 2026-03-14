using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveSettings(SettingsManager settingsManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.prllsave";
        FileStream stream = new FileStream(path, FileMode.Create);
        SettingsData data = new SettingsData(settingsManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.prllsave";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SettingsData data = formatter.Deserialize(stream) as SettingsData;

            stream.Close();
            
            return data;
        }
        else
        {
            Debug.Log("Settings file not found in"+path);
            return null;
        }
    }
}
