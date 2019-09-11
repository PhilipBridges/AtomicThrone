using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SavePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        Save save = new Save();

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static Save LoadSave()
    {
        string path = Application.persistentDataPath + "/player.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Save data = formatter.Deserialize(stream) as Save;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Save not found in: " + path);
            return null;
        }
    }
}
