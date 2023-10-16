using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class VillagePanel : BasePanel
{
    [Header("UI")]
    [SerializeField] private Button btnStore = null;
    [SerializeField] private Button btnSell = null;
    [SerializeField] private Button btnINN = null;
    [SerializeField] private Button btnLeave = null;
    [SerializeField] private ScrollRect srContents = null;
    [SerializeField] private Text textCoin = null;
    [Header("Others")]
    [SerializeField] private ElementCard contentElement = null;

    private List<ElementCard> contentList = new List<ElementCard>();
    private int currentLevel = 0;

    protected override void Start()
    {
        base.Start();
        Hide();
        btnStore.onClick.AddListener(() => OnClickOpenStore(0));
        btnSell.onClick.AddListener(() => OnClickSellCards(0));
        btnINN.onClick.AddListener(() => OnClickINN(0));
        btnLeave.onClick.AddListener(OnClickLeave);
    }

    public void ShowWindow(int level, int playerIndex)
    {
        currentLevel = level;
        var playerData = MainSystem.Instance.PlayerDatas[playerIndex];
        textCoin.text = $"Your Coin : {playerData.Coin}";
        Show();
    }

    public void OnClickOpenStore(int playerIndex)
    {
        ClearContents();
        var index = 0;
        var dm = MainSystem.Instance.DataManager;
        var name = string.Empty;
        var content = string.Empty;
        var price = 0;
        var isRare = false;
        var id = 0;
        for(var i = 0; i < 5; i++)
        {
            var emptyCard = GetCard();
            isRare = Random.Range(0, 2) > 0;
            index = i;
            if(isRare == true)
            {
                var card = dm.GetRandomRareItem(currentLevel);
                name = card.name;
                content = card.description;
                price = card.buy;
                id = card.id;
            }
            else
            {
                var card = dm.GetRandomNormalItem(currentLevel);
                name = card.name;
                content = card.description;
                price = card.buy;
                id = card.id;
            }
            emptyCard.UpdateUI(name, content, price, id, isRare);
            emptyCard.gameObject.SetActive(true);
            emptyCard.OnClick(() => OnClickBuyCard(emptyCard, playerIndex));
        }
    }

    public void OnClickSellCards(int playerIndex)
    {
        ClearContents();
        var playerData = MainSystem.Instance.PlayerDatas[playerIndex];
        var dm = MainSystem.Instance.DataManager;
        foreach (var id in playerData.ItemIdList)
        {
            var emptyCard = GetCard();
            var card = dm.GetNormalItem(id);
            emptyCard.UpdateUI(card.name, card.description, card.sell, card.id, false);
            emptyCard.OnClick(() => OnClickSellCard(emptyCard, playerIndex));
            emptyCard.gameObject.SetActive(true);
        }
        foreach (var id in playerData.RareItemIdList)
        {
            var emptyCard = GetCard();
            var card = dm.GetRarityItem(id);
            emptyCard.UpdateUI(card.name, card.description, card.sell, card.id, true);
            emptyCard.OnClick(() => OnClickSellCard(emptyCard, playerIndex));
            emptyCard.gameObject.SetActive(true);
        }
    }

    private void OnClickBuyCard(ElementCard card, int playerIndex)
    {
        var playerData = MainSystem.Instance.PlayerDatas[playerIndex];
        var comfirmPanel = MainSystem.Instance.UIManager.GetPanel<ConfirmPanel>();
        var price = card.Price;
        if (playerData.Classes == Classes.Merchant) price -= 1;
        if (playerData.Coin >= price)
        {
            comfirmPanel.ShowContent("Do you want to buy the card?", () =>
            {
                playerData.Coin -= price;
                card.gameObject.SetActive(false);
                var controlPanel = MainSystem.Instance.UIManager.GetPanel<ControlPanel>();
                controlPanel.AddCard(card.Id, card.IsRare);
            });
        }
        else
        {
            comfirmPanel.ShowContent("Not enough coin!!", null);
        }

    }

    private void OnClickSellCard(ElementCard card, int playerIndex)
    {
        var playerData = MainSystem.Instance.PlayerDatas[playerIndex];
        var comfirmPanel = MainSystem.Instance.UIManager.GetPanel<ConfirmPanel>();
        comfirmPanel.ShowContent("Do you want to sell the card?", () =>
        {
            var price = card.Price;
            if (playerData.Classes == Classes.Merchant) price += 1;
            playerData.Coin += price;
            card.gameObject.SetActive(false);
            MainSystem.Instance.RemovePlayerItem(playerIndex, card.IsRare, card.Id);
            var playerPanel = MainSystem.Instance.UIManager.GetPanel<PlayerPanel>();
            playerPanel.RemoveCard(playerIndex, card.Id, card.IsRare);
        });
    }

    private void OnClickINN(int playerIndex)
    {
        var price = 0;
        switch(currentLevel)
        {
            case 1:
                price = 2;
                break;
            case 2:
                price = 4;
                break;
            case 3:
                price = 6;
                break;
            case 4:
                price = 8;
                break;
            case 5:
                price = 10;
                break;
        }
        var playerData = MainSystem.Instance.PlayerDatas[playerIndex];
        var comfirmPanel = MainSystem.Instance.UIManager.GetPanel<ConfirmPanel>();
        if (playerData.Exceed == Exceed.Human) price -= 1;
        if (playerData.Coin >= price)
        {
            comfirmPanel.ShowContent("Do you want to have a rest in the INN?", () =>
            {
                playerData.Coin -= price;
                playerData.AddHP(playerData.MaxHitPoint);
            });
        }
        else
        {
            comfirmPanel.ShowContent("Not enough coin!!", null);
        }
    }

    private void OnClickLeave()
    {
        var controlPanel = MainSystem.Instance.UIManager.GetPanel<ControlPanel>();
        controlPanel.DrawMapCard();
        Hide();
    }

    private ElementCard GetCard()
    {
        if(contentList.Count >= 0)
        {
            foreach(var data in contentList)
            {
                if (data.gameObject.activeSelf == false)
                    return data;
            }
        }
        var nData = Instantiate(contentElement, srContents.content);
        nData.Initailize();
        contentList.Add(nData);
        return nData;
    }

    private void ClearContents()
    {
        if (contentList.Count >= 0)
        {
            foreach (var data in contentList)
            {
                data.gameObject.SetActive(false);
            }
        }
    }

}
