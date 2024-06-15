using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListButtonView : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;

    private int _index;
    private Action<int> _onclickAction;
    public int Index => _index;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void InitButton(string text, int index, bool isCurrent, Action<int> OnclickAction)
    {
        _index = index;
        _buttonText.text = text;
        _onclickAction = OnclickAction;
        SetCurrentStatus(isCurrent);
    }

    public void SetCurrentStatus(bool isCurrent)
    {
        _image.color = isCurrent ? Color.green : Color.white;
    }

    private void OnButtonClickHandler()
    {
        _onclickAction?.Invoke(_index);
    }
}
