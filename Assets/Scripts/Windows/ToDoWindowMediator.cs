using UnityEngine;

public class ToDoWindowMediator : Mediator<ToDoWindowView>
{
    private ToDoWindowModel model;
    private int _currentTaskListIndex;

    public ToDoWindowMediator(int currentTaskListIndex)
    {
        if (currentTaskListIndex >= 0)
        {
            _currentTaskListIndex = currentTaskListIndex;
        }
    }

    protected override void Show()
    {
        if (TaskManager.AppModel.TaskLists.Count == 0 || _currentTaskListIndex >= TaskManager.AppModel.TaskLists.Count)
        {
            return;
        }

        model = new ToDoWindowModel()
        {
            AllTaskLists = TaskManager.AppModel.TaskLists,
            CurrentIndex = _currentTaskListIndex,
            IsNeedToUpdateTasksList = true,
        };

        View.Model = model;
        View.INPUT_RECEIVED += OnInputReceived;
        View.OnTaskListButtonClicked += OnTaskListClickHandler;
    }
    protected override void Hide()
    {
        View.INPUT_RECEIVED -= OnInputReceived;
        View.OnTaskListButtonClicked -= OnTaskListClickHandler;
    }

    private void OnInputReceived(ToDoWindowInputEnum input, object arg2)
    {
        switch (input)
        {
            case ToDoWindowInputEnum.NewList:
                CreateOrEditListHandler(TaskManager.AppModel.TaskLists.Count);
                break;
            case ToDoWindowInputEnum.DelComplete:
                RemoveCompleteButtonHandler();
                break;
            case ToDoWindowInputEnum.EditTaskList:
                CreateOrEditListHandler(_currentTaskListIndex);
                break;
            case ToDoWindowInputEnum.DeleteTaskList:

                UiManager.Instance.ShowAdditional<DialogPopUpWindowMediator>(UiElementType.PopUp,
                    new DialogPopUpWindowMediatorParameters()
                    {
                        Message = "Are you sure you want to delete this task list?",
                        OkButtonAction = DeleteTaskList
                    });
                break;
        }
    }

    private void RemoveCompleteButtonHandler()
    {
        if (model.AllTaskLists[_currentTaskListIndex].Tasks.FindAll(t => t.isTaskDone).Count == 0)
            return;

        UiManager.Instance.ShowAdditional<DialogPopUpWindowMediator>(UiElementType.PopUp,
            new DialogPopUpWindowMediatorParameters()
            {
                Message = "Are you sure you want to delete completed items?",
                OkButtonAction = RemoveCompleteTasks
            });
    }

    private void DeleteTaskList()
    {
        if (TaskManager.TryToDeleteTaskList(_currentTaskListIndex))
        {
            if (TaskManager.AppModel.TaskLists.Count == 0)
            {
                UiManager.Instance.HideAdditional<ToDoWindowMediator>();
                UiManager.Instance.ShowAdditional<CreateListWindowMediator>(UiElementType.Window, TaskManager.AppModel.TaskLists.Count);
            }
            else
            {
                if (_currentTaskListIndex == TaskManager.AppModel.TaskLists.Count)
                {
                    _currentTaskListIndex--;
                }

                _currentTaskListIndex = Mathf.Max(0, _currentTaskListIndex);
                TaskManager.AppModel.CurrentTaskListIndex = _currentTaskListIndex;
                TaskManager.SaveModel();

                model.IsNeedToUpdateTasksList = true;
                model.CurrentIndex = _currentTaskListIndex;
                model.SetChanged();
            }
        }
    }

    private void RemoveCompleteTasks()
    {
        model.AllTaskLists[_currentTaskListIndex].Tasks.RemoveAll(t => t.isTaskDone);
        model.SetChanged();

        if (model.AllTaskLists.Count == 0)
        {
            CreateOrEditListHandler(TaskManager.AppModel.TaskLists.Count);
        }
    }

    private void OnTaskListClickHandler(int index)
    {
        TaskManager.AppModel.CurrentTaskListIndex = index;
        _currentTaskListIndex = index;
        
        model.CurrentIndex = index;
        model.IsNeedToUpdateTasksList = false;
        model.SetChanged();
    }

    private void CreateOrEditListHandler(int index)
    {
        UiManager.Instance.HideAdditional<ToDoWindowMediator>();
        UiManager.Instance.ShowAdditional<CreateListWindowMediator>(UiElementType.Window, index);
    }
}
