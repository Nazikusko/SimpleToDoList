using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string FILE_NAME = "ToDoListSaveFile.txt";
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    public static AppSaveModel LoadDataFromDisk()
    {
        AppSaveModel result;
        if (File.Exists(SaveFilePath))
        {
            try
            {
                result = JsonConvert.DeserializeObject<AppSaveModel>(File.ReadAllText(SaveFilePath));
                return result;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception.Message);
            }
        }

        return GetDefaultModel();
    }

    public static void SaveDataToDisk(AppSaveModel model)
    {
        File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(model, Formatting.Indented));
    }

    private static AppSaveModel GetDefaultModel()
    {
        return new AppSaveModel()
        {
            TaskLists = new List<TaskList>(),
            CurrentTaskListIndex = 0,
        };
    }
}
