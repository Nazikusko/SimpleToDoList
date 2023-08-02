using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string FILE_NAME = "ToDoListSaveFile.txt";
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    public static List<ToDoListController.TaskData> LoadDataFromDisk()
    {
        return File.Exists(SaveFilePath) ? JsonConvert.DeserializeObject<List<ToDoListController.TaskData>>(File.ReadAllText(SaveFilePath)) : 
            new List<ToDoListController.TaskData>();
    }

    public static void SaveDataToDisk(List<ToDoListController.TaskData> tasks)
    {
        File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(tasks));
    }
}
