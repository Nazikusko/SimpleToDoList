using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string FILE_NAME = "ToDoListSaveFile.txt";
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    public static List<TaskDataModel> LoadDataFromDisk()
    {
        return File.Exists(SaveFilePath) ? JsonConvert.DeserializeObject<List<TaskDataModel>>(File.ReadAllText(SaveFilePath)) : 
            new List<TaskDataModel>();
    }

    public static void SaveDataToDisk(List<TaskDataModel> tasks)
    {
        File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(tasks));
    }
}
