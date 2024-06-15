
using System.Collections.Generic;

public class TaskDataModel
{
    public string taskText;
    public bool isTaskDone;
}

public class TaskList
{
    public string TaskListName;
    public List<TaskDataModel> Tasks;
}

public class AppSaveModel
{
    public List<TaskList> TaskLists;
    public int CurrentTaskListIndex;
    public int ListsCounter;
}