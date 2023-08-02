using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;
using static ToDoListController;

public class ToDoItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private TMP_Text _indexText;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _ItemBackgroundImage;
    [SerializeField] private Button _itemButton;

    private ToDoListController.TaskData _taskData;

    void Awake()
    {

    }

    void OnDestroy()
    {
        //_toggle.onValueChanged.RemoveAllListeners();
        //_itemButton.onClick.RemoveAllListeners();
    }

    public void Init(int index, ToDoListController.TaskData data, Action<bool> onClickAction)
    {
        _taskData = data;
        _indexText.text = (index + 1).ToString();
        _toggle.isOn = data.isTaskDone;
        SetDoneTask(data.isTaskDone);
        _taskText.text = data.taskText;

        _itemButton.onClick.AddListener(() =>
        {
            _taskData.isTaskDone = !_taskData.isTaskDone;
            SetDoneTask(_taskData.isTaskDone);
            onClickAction?.Invoke(_taskData.isTaskDone);
        });
    }

    public void SetDoneTask(bool isDone)
    {
        _ItemBackgroundImage.color = isDone ? Color.green : Color.white;
        _taskData.isTaskDone = isDone;
        _toggle.isOn = isDone;
    }
}
