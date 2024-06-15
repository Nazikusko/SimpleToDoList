using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateListWindowModel : Observable
{
    public string InputFieldText;
    public string ListName;
    public bool IsCancelButtonActive;
}

public enum CreateListWindowInputEnum
{
    Ok,
    Cancel,
    Clear,
    Paste,
}

public class CreateListWindowView : BaseHudWithModel<CreateListWindowModel, CreateListWindowInputEnum>
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private Button _pasteButton;
    [SerializeField] private TMP_InputField _listNameInputField;
    [SerializeField] private TMP_InputField _inputField;

    public TMP_InputField InputField => _inputField;
    public TMP_InputField ListNameInputField => _listNameInputField;

    private string _oldEditText;
    private string _editText;
    private bool _keepOldTextInField;

    protected override void OnEnable()
    {
        AddListener(_okButton, CreateListWindowInputEnum.Ok);
        AddListener(_clearButton, CreateListWindowInputEnum.Clear);
        AddListener(_cancelButton, CreateListWindowInputEnum.Cancel);
        AddListener(_pasteButton, CreateListWindowInputEnum.Paste);

        _inputField.onValueChanged.AddListener(OnInputFieldValueChangedHandler);
        _inputField.onEndEdit.AddListener(OnEndEditHandler);
        _inputField.onTouchScreenKeyboardStatusChanged.AddListener(ReportChangeStatus);
    }

    protected override void OnDisable()
    {
        RemoveListener(_okButton);
        RemoveListener(_clearButton);
        RemoveListener(_cancelButton);
        RemoveListener(_pasteButton);

        _inputField.onValueChanged.RemoveAllListeners();
        _inputField.onEndEdit.RemoveAllListeners();
        _inputField.onTouchScreenKeyboardStatusChanged.RemoveAllListeners();
    }

    protected override void OnModelChanged(CreateListWindowModel model)
    {
        _inputField.text = model.InputFieldText;
        _cancelButton.interactable = model.IsCancelButtonActive;
        _okButton.interactable = !string.IsNullOrEmpty(_inputField.text);
        _listNameInputField.text = model.ListName;
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
}
