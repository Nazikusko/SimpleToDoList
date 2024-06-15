using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToDoWindowModel : Observable
{
    public List<TaskList> AllTaskLists;
    public int CurrentIndex;
    public bool IsNeedToUpdateTasksList;
}

public enum ToDoWindowInputEnum
{
    NewList,
    DelComplete,
    EditTaskList,
    DeleteTaskList,
}

public class ToDoWindowView : BaseHudWithModel<ToDoWindowModel, ToDoWindowInputEnum>
{
    public event Action<TaskDataModel> OnTaskItemValueChanged;
    public event Action<int> OnTaskListButtonClicked;

    [SerializeField] private Button _newListButton;
    [SerializeField] private Button _delCompleteButton;
    [SerializeField] private Button _editTaskList;
    [SerializeField] private Button _deleteTaskList;
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private ToDoItemView _toDoItemViewPrefab;
    [SerializeField] private ListButtonView _listButtonPrefab;
    [SerializeField] private Transform _listButtonsContentHolder;

    private List<ToDoItemView> _tasksView = new List<ToDoItemView>();
    private List<ListButtonView> _currentTaskLists = new List<ListButtonView>();

    protected override void OnEnable()
    {
        AddListener(_delCompleteButton, ToDoWindowInputEnum.DelComplete);
        AddListener(_newListButton, ToDoWindowInputEnum.NewList);
        AddListener(_editTaskList, ToDoWindowInputEnum.EditTaskList);
        AddListener(_deleteTaskList, ToDoWindowInputEnum.DeleteTaskList);
    }

    protected override void OnDisable()
    {
        RemoveListener(_delCompleteButton);
        RemoveListener(_newListButton);
        RemoveListener(_editTaskList);
        RemoveListener(_deleteTaskList);
    }

    protected override void OnModelChanged(ToDoWindowModel model)
    {
        UpdateList(model.AllTaskLists[model.CurrentIndex].Tasks);
        UpdateListButton(model);
    }

    public void UpdateListButton(ToDoWindowModel model)
    {
        if (model.IsNeedToUpdateTasksList)
        {
            foreach (var taskList in _currentTaskLists)
            {
                DestroyImmediate(taskList.gameObject);
            }
            _currentTaskLists.Clear();

            for (int i = 0; i < model.AllTaskLists.Count; i++)
            {
                var button = Instantiate(_listButtonPrefab, _listButtonsContentHolder);
                button.InitButton(model.AllTaskLists[i].TaskListName, i, i == model.CurrentIndex, OnListButtonClicked);
                _currentTaskLists.Add(button);
            }

            Canvas.ForceUpdateCanvases();

            var holderTransform = _listButtonsContentHolder.GetComponent<RectTransform>();

            var width = holderTransform.sizeDelta.x;
            var position = -width * ((float)model.CurrentIndex / model.AllTaskLists.Count);
            holderTransform.anchoredPosition = new Vector2(position, holderTransform.anchoredPosition.y);
        }
        else
        {
            for (int i = 0; i < model.AllTaskLists.Count; i++)
            {
                _currentTaskLists[i].SetCurrentStatus(i == model.CurrentIndex);
            }
        }
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
            _tasksView.Add(view);
            view.Init(i, tasksData[i], OnTaskValueChanged);
        }

        UpdateDelButtonStatus();
    }

    private void UpdateDelButtonStatus()
    {
        _delCompleteButton.interactable = !_tasksView.TrueForAll(t => !t.Model.isTaskDone);
    }

    private void OnTaskValueChanged(TaskDataModel model)
    {
        OnTaskItemValueChanged?.Invoke(model);
        UpdateDelButtonStatus();
    }

    private void OnListButtonClicked(int index)
    {
        OnTaskListButtonClicked?.Invoke(index);
    }
}
