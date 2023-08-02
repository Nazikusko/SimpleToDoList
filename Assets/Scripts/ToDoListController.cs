using System.Collections.Generic;
using UnityEngine;

public class ToDoListController : MonoBehaviour
{
    [SerializeField] private ToDoItemView _toDoItemViewPrefab;
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private CreateListController _createListController;

    private List<TaskData> _tasks;
    private List<ToDoItemView> _tasksView = new List<ToDoItemView>();

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
            _createListController.okButtonClickAction += OkButtonClickHandler;
        }
    }

    private void OkButtonClickHandler(List<TaskData> tasksData)
    {
        _tasks = tasksData;
        UpdateList();
    }

    void OnApplicationQuit()
    {
        SaveManager.SaveDataToDisk(_tasks);
    }

    public class TaskData
    {
        public string taskText;
        public bool isTaskDone;
    }
}
