using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWindow : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_Text _messageText;

    public void ShowWindow(string messageText, Action okButtonAction, Action cancelButtonAction)
    {
        _okButton.onClick.RemoveAllListeners();
        _cancelButton.onClick.RemoveAllListeners();
        _messageText.text = messageText;

        _okButton.onClick.AddListener(() =>
        {
            okButtonAction?.Invoke();
            Close();
        });
        _cancelButton.onClick.AddListener(() =>
        {
            cancelButtonAction?.Invoke();
            Close();
        });
        
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
