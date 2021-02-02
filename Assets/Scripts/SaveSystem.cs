using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    
    private static string levelPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "level.tri";
    private static BinaryFormatter formatter = new BinaryFormatter();
    
    public static void SaveLevel(int level, int snapshot) {
        FileStream stream = new FileStream(levelPath, FileMode.Create);
        
        GameData data = new GameData(level, snapshot);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadLevel() {
        if (File.Exists(levelPath)) {
            
            Debug.Log("THE PATH EXISTS");
            
            FileStream stream = new FileStream(levelPath, FileMode.Open);
            GameData data = (GameData) formatter.Deserialize(stream);
            
            Debug.Log("THE DATA IS: " + data);
            
            stream.Close();
            return data;
        }
        return null;
    }
}
