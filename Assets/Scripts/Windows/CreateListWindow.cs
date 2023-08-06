using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateListWindow : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _clearTextButton;
    [SerializeField] private Button _pasteButton;
    [SerializeField] private TMP_InputField _inputField;

    public Action<List<TaskDataModel>> okButtonClickAction;

    private string _oldEditText;
    private string _editText;
    private bool _keepOldTextInField;

    void Awake()
    {
        _okButton.onClick.AddListener(OkButtonClickHandler);
        _inputField.onValueChanged.AddListener(OnInputFieldValueChangedHandler);
        _inputField.onEndEdit.AddListener(OnEndEditHandler);
        _inputField.onTouchScreenKeyboardStatusChanged.AddListener(ReportChangeStatus);
        _cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        _clearTextButton.onClick.AddListener(() => _inputField.text = string.Empty);
        _pasteButton.onClick.AddListener(() => _inputField.text = GUIUtility.systemCopyBuffer);
    }

    void OnDestroy()
    {
        _okButton.onClick.RemoveAllListeners();
        _inputField.onValueChanged.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
        _clearTextButton.onClick.RemoveAllListeners();
        _pasteButton.onClick.RemoveAllListeners();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void ShowWindow(bool isShow)
    {
        gameObject.SetActive(isShow);

        if (!isShow) return;

        _cancelButton.interactable = ToDoListController.Instance.ListHaveTasks;
        _okButton.interactable = !string.IsNullOrEmpty(_inputField.text);
    }
    private void OnInputFieldValueChangedHandler(string currentText)
    {
        _okButton.interactable = !string.IsNullOrEmpty(currentText);
        _oldEditText = _editText;
        _editText = currentText;
    }

    public void OnEndEditHandler(string text)
    {
        if (_keepOldTextInField)
        {
            _editText = _oldEditText;
            _inputField.text = _editText;
            _keepOldTextInField = false;
        }
    }

    private void ReportChangeStatus(TouchScreenKeyboard.Status newStatus)
    {
        if (newStatus == TouchScreenKeyboard.Status.Canceled)
            _keepOldTextInField = true;
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

    public void SetListEditorText(string listEditorText)
    {
        _inputField.text = listEditorText;
    }
}
