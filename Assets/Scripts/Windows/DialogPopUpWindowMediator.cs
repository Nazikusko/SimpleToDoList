using System;

public class DialogPopUpWindowMediatorParameters
{
    public Action OkButtonAction;
    public string Message;
}

public class DialogPopUpWindowMediator : Mediator<DialogPopUpWindowView>
{
    private DialogPopUpWindowModel model;

    private Action _okButtonAction;
    private string _message;

    public DialogPopUpWindowMediator(DialogPopUpWindowMediatorParameters parameters)
    {
        _okButtonAction = parameters.OkButtonAction;
        _message = parameters.Message;
    }

    protected override void Show()
    {
        model = new DialogPopUpWindowModel()
        {
            DialogText = _message,
        };

        View.Model = model;
        View.INPUT_RECEIVED += OnInputReceived;
    }

    private void OnInputReceived(DialogPopUpWindowInputEnum input, object arg2)
    {
        switch (input)
        {
            case DialogPopUpWindowInputEnum.Ok:
                _okButtonAction?.Invoke();
                UiManager.Instance.HideAdditional<DialogPopUpWindowMediator>();
                break;

            case DialogPopUpWindowInputEnum.Cancel:
                UiManager.Instance.HideAdditional<DialogPopUpWindowMediator>();
                break;
        }
    }

    protected override void Hide()
    {
        View.INPUT_RECEIVED -= OnInputReceived;
    }
}
