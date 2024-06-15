using System.Collections.Generic;

public static class TaskManager
{
    private static AppSaveModel _currentAppModel;
    public static AppSaveModel AppModel => _currentAppModel;

    public static void UpdateTasksList(List<TaskDataModel> tasks, string taskListName, int index)
    {
        if (tasks == null)
            return;

        if (index < _currentAppModel.TaskLists.Count)
        {
            _currentAppModel.TaskLists[index].Tasks = tasks;
            _currentAppModel.TaskLists[index].TaskListName = taskListName;
        }
        else
        {
            _currentAppModel.TaskLists.Add(new TaskList()
            {
                Tasks = tasks,
                TaskListName = taskListName
            });
        }
        SaveModel();
    }

    public static bool TryToDeleteTaskList(int index)
    {
        if (index < _currentAppModel.TaskLists.Count)
        {
            _currentAppModel.TaskLists.RemoveAt(index);
            SaveModel();
            return true;
        }

        return false;
    }

    public static void SaveModel()
    {
        SaveManager.SaveDataToDisk(_currentAppModel);
    }

    public static AppSaveModel LoadTasks()
    {
        _currentAppModel = SaveManager.LoadDataFromDisk();
        return _currentAppModel;
    }
}
