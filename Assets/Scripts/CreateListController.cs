using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateListController : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _clearTextButton;
    [SerializeField] private Button _pasteButton;
    [SerializeField] private TMP_InputField _inputField;

    public Action<List<TaskDataModel>> okButtonClickAction;

    void Awake()
    {
        _okButton.onClick.AddListener(OkButtonClickHandler);
        _inputField.onValueChanged.AddListener(OnInputFieldValueChangedHandler);
        _cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        _clearTextButton.onClick.AddListener(() => _inputField.text = string.Empty);
        _pasteButton.onClick.AddListener(() => _inputField.text = GUIUtility.systemCopyBuffer);

    }

    void OnEnable()
    {
        _cancelButton.interactable = ToDoListController.Instance.ListHaveTasks;
        _okButton.interactable = !string.IsNullOrEmpty(_inputField.text);
    }

    void OnDestroy()
    {
        _okButton.onClick.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
        _clearTextButton.onClick.RemoveAllListeners();
        _pasteButton.onClick.RemoveAllListeners();
    }

    private void OnInputFieldValueChangedHandler(string value)
    {
        _okButton.interactable = !string.IsNullOrEmpty(value);
    }

    private void OkButtonClickHandler()
    {
        var stringItems = _inputField.text.Split("\n");

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

        okButtonClickAction?.Invoke(tasksData);
        gameObject.SetActive(false);
    }
}
