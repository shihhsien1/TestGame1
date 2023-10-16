using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel : BasePanel
{
    [SerializeField] private Text textContent = null;
    [SerializeField] private ButtonObjects btnConfirm = null;
    [SerializeField] private ButtonObjects btnCancel = null;

    protected override void Start()
    {
        base.Start();
        Hide();
    }

    public void ShowContent(string content, Action confirmCallback, Action cancelCallback = null)
    {
        Show();
        textContent.text = content;
        btnConfirm.Button.onClick.RemoveAllListeners();
        btnConfirm.Button.onClick.AddListener(() => 
        {
            confirmCallback?.Invoke();
            Hide();
        });
        btnCancel.Button.onClick.RemoveAllListeners();
        btnCancel.Button.onClick.AddListener(() =>
        {
            cancelCallback?.Invoke();
            Hide();
        });
    }
}
