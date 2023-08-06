using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToDoItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private TMP_Text _indexText;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _ItemBackgroundImage;
    [SerializeField] private Button _itemButton;

    private TaskDataModel _taskData;
    private Action<TaskDataModel> _valueChangedAction;

    void OnDestroy()
    {
        _itemButton.onClick.RemoveAllListeners();
    }

    public void Init(int index, TaskDataModel data, Action<TaskDataModel> onValueChangeAction)
    {
        _taskData = data;
        _indexText.text = (index + 1).ToString();
        _toggle.isOn = data.isTaskDone;
        SetDoneTask(data.isTaskDone);
        _taskText.text = data.taskText;
        _valueChangedAction = onValueChangeAction;

        _itemButton.onClick.AddListener(() =>
        {
            _taskData.isTaskDone = !_taskData.isTaskDone;
            SetDoneTask(_taskData.isTaskDone);
            _valueChangedAction?.Invoke(_taskData);
        });
    }

    public void SetDoneTask(bool isDone)
    {
        _ItemBackgroundImage.color = isDone ? Color.green : Color.white;
        _taskData.isTaskDone = isDone;

        _toggle.isOn = isDone;
    }
}
