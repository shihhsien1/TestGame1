using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningPanel : BasePanel
{
    [SerializeField] private Button btnNewGame = null;
    [SerializeField] private Button btnDeck = null;
    [SerializeField] private Button btnLeave = null;
    protected override void Start()
    {
        base.Start();
        btnNewGame.onClick.AddListener(OnClickNewGame);
        btnDeck.onClick.AddListener(OnClickDeck);
        btnLeave.onClick.AddListener(OnClickLeave);
    }
    private void OnClickNewGame()
    {
        SceneManager.LoadScene("Main");
    }

    private void OnClickDeck()
    {
        var cardDeckPanel = MainSystem.Instance.UIManager.GetPanel<CardDeckPanel>();
        cardDeckPanel.Show();
    }

    private void OnClickLeave() 
    {
        Application.Quit();
    }

}
