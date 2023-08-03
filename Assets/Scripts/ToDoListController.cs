using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToDoListController : Singleton<ToDoListController>
{
    [SerializeField] private ToDoItemView _toDoItemViewPrefab;
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private CreateListController _createListController;
    [SerializeField] private Button _createNewListButton;

    private List<TaskDataModel> _tasks = new List<TaskDataModel>();
    private List<ToDoItemView> _tasksView = new List<ToDoItemView>();

    public bool ListHaveTasks => _tasks.Count > 0;

    protected override void DoAwake()
    {
        _createListController.okButtonClickAction += OkButtonClickHandler;
        _createNewListButton.onClick.AddListener(() => _createListController.gameObject.SetActive(true));
    }

    void Start()
    {
        var data = SaveManager.LoadDataFromDisk();
        if (data != null && data.Count > 0)
        {
            _createListController.gameObject.SetActive(false);
            _tasks = data;
            UpdateList();
        }
        else
        {
            _createListController.gameObject.SetActive(true);

        }
    }

    void OnApplicationQuit()
    {
        SaveManager.SaveDataToDisk(_tasks);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SaveManager.SaveDataToDisk(_tasks);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveManager.SaveDataToDisk(_tasks);
    }

    private void OkButtonClickHandler(List<TaskDataModel> tasksData)
    {
        _tasks = tasksData;
        UpdateList();
    }

    public void UpdateList()
    {
        for (int i = 0; i < _contentHolder.childCount; i++)
        {
            Destroy(_contentHolder.GetChild(i).gameObject);
        }

        for (int i = 0; i < _tasks.Count; i++)
        {
            var view = Instantiate(_toDoItemViewPrefab, _contentHolder);
            _tasksView.Add(view);
            view.Init(i, _tasks[i], null);
        }
    }
}
