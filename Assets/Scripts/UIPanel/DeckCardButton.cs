using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardButton : MonoBehaviour
{
    [SerializeField] private Button btn = null;
    [SerializeField] private Text textBtn = null;
    public int Id { get; set; } = 0;
    public string DeckDeck { get; set; } = string.Empty;

    public void UpdateUI(string btnContent, Action btnCallback)
    {
        btn?.onClick.RemoveAllListeners();
        btn?.onClick.AddListener(() => btnCallback?.Invoke());
        textBtn.text = btnContent;
        //btn.image.sprite = 
    }
}
