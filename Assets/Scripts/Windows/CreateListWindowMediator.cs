using System.Collections.Generic;
using UnityEngine;

public class CreateListWindowMediator : Mediator<CreateListWindowView>
{
    public const string NEW_LIST_DEFAULT_NAME = "List{0}";

    private CreateListWindowModel model;
    private int _taskListIndex;

    public CreateListWindowMediator(int taskListIndex)
    {
        if (taskListIndex >= 0)
        {
            _taskListIndex = taskListIndex;
        }
    }

    protected override void Show()
    {
        string inputFieldText;
        string inputFieldListName;
        if (_taskListIndex < TaskManager.AppModel.TaskLists.Count)
        {
            inputFieldText = TaskDataToEditorText(TaskManager.AppModel.TaskLists[_taskListIndex].Tasks);
            inputFieldListName = TaskManager.AppModel.TaskLists[_taskListIndex].TaskListName;
        }
        else
        {
            inputFieldText = string.Empty;
            inputFieldListName = string.Format(NEW_LIST_DEFAULT_NAME, TaskManager.AppModel.ListsCounter + 1);
        }


        model = new CreateListWindowModel()
        {
            InputFieldText = inputFieldText,
            IsCancelButtonActive = TaskManager.AppModel.TaskLists.Count > 0,
            ListName = inputFieldListName,
        };

        View.Model = model;
        View.INPUT_RECEIVED += OnInputReceived;
    }

    private void OnInputReceived(CreateListWindowInputEnum input, object arg2)
    {
        switch (input)
        {
            case CreateListWindowInputEnum.Ok:
                var taskData = ConvertTextToTasks();
                if (taskData.Count == 0)
                {
                    return;
                }

                TaskManager.AppModel.ListsCounter++;
                var inputFieldText = View.ListNameInputField.text;
                var listName = string.IsNullOrEmpty(inputFieldText) ?
                    string.Format(NEW_LIST_DEFAULT_NAME, TaskManager.AppModel.ListsCounter) : inputFieldText;

                TaskManager.UpdateTasksList(taskData, listName, _taskListIndex);
                TaskManager.AppModel.CurrentTaskListIndex = _taskListIndex;
                ShowToDoWindow(_taskListIndex);
                break;

            case CreateListWindowInputEnum.Cancel:
                ShowToDoWindow(TaskManager.AppModel.CurrentTaskListIndex);
                break;
            case CreateListWindowInputEnum.Clear:
                model.InputFieldText = string.Empty;
                model.SetChanged();
                break;
            case CreateListWindowInputEnum.Paste:
                model.InputFieldText = GUIUtility.systemCopyBuffer;
                model.SetChanged();
                break;
        }
    }

    protected override void Hide()
    {
        View.INPUT_RECEIVED -= OnInputReceived;
    }

    private void ShowToDoWindow(int index)
    {
        UiManager.Instance.HideAdditional<CreateListWindowMediator>();
        UiManager.Instance.ShowAdditional<ToDoWindowMediator>(UiElementType.Window, index);
    }

    private string TaskDataToEditorText(List<TaskDataModel> data)
    {
        string listEditorText = string.Empty;
        foreach (TaskDataModel model in data)
        {
            listEditorText += model.taskText + "\n";
        }

        return listEditorText;
    }

    public List<TaskDataModel> ConvertTextToTasks()
    {
        var stringItems = View.InputField.text.Split("\n");

        var tasksData = new List<TaskDataModel>();
        foreach (var taskText in stringItems)
        {
            var clearTaskText = taskText.Trim();
            if (string.IsNullOrEmpty(clearTaskText)) continue;

            tasksData.Add(new TaskDataModel()
            {
                isTaskDone = false,
                taskText = clearTaskText,
            });
        }

        return tasksData;
    }
}
