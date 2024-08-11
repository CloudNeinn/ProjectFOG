using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; // Import the Newtonsoft.Json namespace
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Read serialized data from the file
                string dataToLoad;
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize data from JSON to GameData
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // Create directory where save file will be written
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Configure JsonSerializerSettings to handle self-referencing loops
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serialize game data object to JSON using Newtonsoft.Json
            string dataToStore = JsonConvert.SerializeObject(data, settings);

            // Write serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
            Debug.Log("Saved data to " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

}
