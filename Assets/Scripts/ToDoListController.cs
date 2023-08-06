using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToDoListController : Singleton<ToDoListController>
{
    [SerializeField] private ToDoItemView _toDoItemViewPrefab;
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private CreateListWindow _createListWindow;
    [SerializeField] private PopUpWindow _popUpWindow;
    [SerializeField] private Button _createNewListButton;
    [SerializeField] private Button _removeCompleteButton;

    private List<TaskViewData> _tasksView = new List<TaskViewData>();

    public bool ListHaveTasks => _tasksView.Count > 0;

    protected override void DoAwake()
    {
        _createListWindow.okButtonClickAction += UpdateList;
        _createNewListButton.onClick.AddListener(CreateNewListHandler);
        _removeCompleteButton.onClick.AddListener(RemoveCompleteButtonHandler);
    }

    void Start()
    {
        _popUpWindow.Close();

        var data = SaveManager.LoadDataFromDisk();
        if (data != null && data.Count > 0)
        {
            _createListWindow.ShowWindow(false);

            UpdateListEditorText(data);
            UpdateList(data);
        }
        else
        {
            _createListWindow.ShowWindow(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void OnApplicationQuit()
    {
        SaveManager.SaveDataToDisk(_tasksView.Select(t => t.taskDataModel).ToList());
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SaveManager.SaveDataToDisk(_tasksView.Select(t => t.taskDataModel).ToList());
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveManager.SaveDataToDisk(_tasksView.Select(t => t.taskDataModel).ToList());
    }

    private void RemoveCompleteButtonHandler()
    {
        if (_tasksView.FindAll(t => t.taskDataModel.isTaskDone).Count == 0)
            return;

        _popUpWindow.ShowWindow("Are you sure you want to delete completed items?", RemoveCompleteTasks, null);
    }

    private void RemoveCompleteTasks()
    {
        List<TaskViewData> updatedTasks = new List<TaskViewData>();

        foreach (var taskView in _tasksView)
        {
            if (taskView.taskDataModel.isTaskDone)
            {
                Destroy(taskView.itemView.gameObject);
            }
            else
            {
                updatedTasks.Add(taskView);
            }
        }
        _tasksView = updatedTasks;

        if (_tasksView.Count > 0)
            UpdateList(_tasksView.Select(t => t.taskDataModel).ToList());
        else
            CreateNewListHandler();
    }

    private void UpdateListEditorText(List<TaskDataModel> data)
    {
        string listEditorText = string.Empty;
        foreach (TaskDataModel model in data)
        {
            listEditorText += model.taskText + "\n";
        }

        _createListWindow.SetListEditorText(listEditorText);
    }

    private void CreateNewListHandler()
    {
        UpdateListEditorText(_tasksView.Select(t => t.taskDataModel).ToList());
        _createListWindow.ShowWindow(true);
    }

    public void UpdateList(List<TaskDataModel> tasksData)
    {
        for (int i = 0; i < _contentHolder.childCount; i++)
        {
            Destroy(_contentHolder.GetChild(i).gameObject);
        }

        _tasksView.Clear();
        for (int i = 0; i < tasksData.Count; i++)
        {
            var view = Instantiate(_toDoItemViewPrefab, _contentHolder);

            _tasksView.Add(new TaskViewData()
            {
                taskDataModel = tasksData[i],
                itemView = view
            });

            view.Init(i, tasksData[i], OnTaskValueChanged);
        }

        UpdateDelButtonStatus();
    }

    private void OnTaskValueChanged(TaskDataModel _)
    {
        UpdateDelButtonStatus();
    }

    private void UpdateDelButtonStatus()
    {
        _removeCompleteButton.interactable = !_tasksView.TrueForAll(t => !t.taskDataModel.isTaskDone);
    }

    private class TaskViewData
    {
        public TaskDataModel taskDataModel;
        public ToDoItemView itemView;
    }

}
