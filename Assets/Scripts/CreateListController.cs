using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateListController : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_InputField _inputField;

    public Action<List<ToDoListController.TaskData>> okButtonClickAction;

    void Awake()
    {
        _okButton.onClick.AddListener(OkButtonClickHandler);
        _inputField.onValueChanged.AddListener(OnInputFieldValueChangedHandler);
    }

    private void OnInputFieldValueChangedHandler(string value)
    {
        _okButton.enabled = !string.IsNullOrEmpty(value);
    }

    private void OkButtonClickHandler()
    {
        var stringItems = _inputField.text.Split("\n");

        var tasksData = new List<ToDoListController.TaskData>();
        foreach (var taskText in stringItems)
        {
            if (!string.IsNullOrEmpty(taskText))
                tasksData.Add(new ToDoListController.TaskData()
                {
                    isTaskDone = false,
                    taskText = taskText,
                });
        }

        okButtonClickAction?.Invoke(tasksData);
        gameObject.SetActive(false);
    }
}
