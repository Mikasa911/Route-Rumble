using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem 
{
    private static string profilesPath = Application.persistentDataPath + "/profiles.sbr";
    public static void SaveData(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "SaveData.sbr";
        Debug.Log(path);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            SaveData data = new SaveData(gameManager.profilesList);
            foreach(ProfileData pro in data.profilesList)
            {
                Debug.Log(pro.profileName);
            }
            formatter.Serialize(stream, data);
        }
    }
    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "SaveData.sbr";
        if (File.Exists(path))
        {
            Debug.Log(path);
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }
}
