using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static CardDeckPanel;

public class CardDeckPanel : BasePanel
{
    [Serializable]
    public class ChooseDeckUI
    {
        public GameObject panel = null;
        public Button btnNewDeck = null;
        public Button btnLeave = null;
        public ScrollRect srDecks = null;
    }
    [Serializable]
    public class DeckDetailUI
    {
        public GameObject panel = null;
        public ScrollRect srEventList = null;
        public ScrollRect srDestinyList = null;
        public ScrollRect srMonsterList = null;
        public Button btnNewCard = null;
        public Button btnSaveDeck = null;
        public Button btnCancel = null;
        public InputField ifDeckName = null;
    }
    [Header("UI")]
    [SerializeField] private ChooseDeckUI chooseDeckUI = null;
    [SerializeField] private DeckDetailUI deckDetailUI = null;
    [Header("Others")]
    [SerializeField] private DeckCardButton cardElement = null;

    private List<EventCardData> eventDataList = null;
    private List<DestinyCardData> destinyDataList = null;
    private List<MonsterCardData> monsterDataList = null;
    private List<LoadDeckData> loadDeckDatas = null;
    private List<DeckCardButton> deckObjList = new List<DeckCardButton>();
    private List<DeckCardButton> eventObjList = new List<DeckCardButton>();
    private List<DeckCardButton> destinyObjList = new List<DeckCardButton>();
    private List<DeckCardButton> monsterObjList = new List<DeckCardButton>();

    private void OnEnable()
    {
        if(loadDeckDatas != null) loadDeckDatas.Clear();
        loadDeckDatas = DataHelper.JsonDeserializeData<LoadDeckData>("DeckDatas");
        if(loadDeckDatas == null)
        {
            loadDeckDatas = new List<LoadDeckData>();
        }
        else
        {
            var hasDeck = false;
            foreach (var data in loadDeckDatas)
            {
                hasDeck = false;
                foreach (var cData in deckObjList)
                {
                    if(cData.DeckDeck == data.deckName)
                    {
                        hasDeck = true;
                        break;
                    }
                }
                if (hasDeck == true) continue;
                var obj = Instantiate(cardElement, chooseDeckUI.srDecks.content);
                obj.UpdateUI(data.deckName, () => OnClickDeck(data));
                obj.DeckDeck = data.deckName;
                deckObjList.Add(obj);
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        chooseDeckUI.btnNewDeck?.onClick.AddListener(OnClickNewDeck);
        chooseDeckUI.btnLeave?.onClick.AddListener(Hide);
        deckDetailUI.btnNewCard?.onClick.AddListener(OnClickNewCard);
        deckDetailUI.btnSaveDeck?.onClick.AddListener(OnClickSaveDeck);
        deckDetailUI.btnCancel?.onClick.AddListener(OnClickCanel);
        Hide();
    }
    #region ChooseDeckUI
    private void OnClickNewDeck()
    {
        chooseDeckUI.panel.SetActive(false);
        deckDetailUI.panel.SetActive(true);
        eventDataList = new List<EventCardData>();
        destinyDataList = new List<DestinyCardData>();
        monsterDataList = new List<MonsterCardData>();
    }
    private void OnClickDeck(LoadDeckData data)
    {
        chooseDeckUI.panel.SetActive(false);
        deckDetailUI.panel.SetActive(true);
        eventDataList = DataHelper.JsonDeserializeData<EventCardData>(data.deckName + "/EventCards");
        destinyDataList = DataHelper.JsonDeserializeData<DestinyCardData>(data.deckName + "/DestinyCards");
        monsterDataList = DataHelper.JsonDeserializeData<MonsterCardData>(data.deckName + "/MonsterCards");
        UpdateCardList();
        deckDetailUI.ifDeckName.text = data.deckName;
    }
    private void UpdateCardList()
    {
        DeckCardButton objCard = null;
        if (eventDataList != null)
        {
            foreach (var eData in eventDataList)
            {
                objCard = null;
                foreach (var cData in eventObjList)
                {
                    if(eData.id == cData.Id)
                    {
                        objCard = cData;
                        break;
                    }
                }
                if (objCard != null)
                {
                    objCard.UpdateUI(eData.name, () =>
                    {
                        var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
                        cardMakePanel.ShowByEvent(eData);
                        Hide();
                    });
                    continue;
                }
                var obj = Instantiate(cardElement, deckDetailUI.srEventList.content);
                obj.UpdateUI(eData.name, () =>
                {
                    var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
                    cardMakePanel.ShowByEvent(eData);
                    Hide();
                });
                obj.Id = eData.id;
                eventObjList.Add(obj);
            }
        }
        else
        {
            eventDataList = new List<EventCardData>();
        }
        if (destinyDataList != null)
        {
            foreach (var eData in destinyDataList)
            {
                objCard = null;
                foreach (var cData in destinyObjList)
                {
                    if (eData.id == cData.Id)
                    {
                        objCard = cData;
                        break;
                    }
                }
                if (objCard != null)
                {
                    objCard.UpdateUI(eData.name, () =>
                    {
                        var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
                        cardMakePanel.ShowByDestiny(eData);
                        Hide();
                    });
                    continue;
                }
                var obj = Instantiate(cardElement, deckDetailUI.srDestinyList.content);
                obj.UpdateUI(eData.name, () =>
                {
                    var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
                    cardMakePanel.ShowByDestiny(eData);
                    Hide();
                });
                obj.Id = eData.id;
                destinyObjList.Add(obj);
            }
        }
        else
        {
            destinyDataList = new List<DestinyCardData>();
        }
        if (monsterDataList != null)
        {
            foreach (var eData in monsterDataList)
            {
                objCard = null;
                foreach (var cData in monsterObjList)
                {
                    if (eData.id == cData.Id)
                    {
                        objCard = cData;
                        break;
                    }
                }
                if (objCard != null)
                {
                    objCard.UpdateUI(eData.name, () =>
                    {
                        var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
                        cardMakePanel.ShowByMonster(eData);
                        Hide();
                    });
                    continue;
                }
                var obj = Instantiate(cardElement, deckDetailUI.srMonsterList.content);
                obj.UpdateUI(eData.name, () =>
                {
                    var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
                    cardMakePanel.ShowByMonster(eData);
                    Hide();
                });
                obj.Id = eData.id;
                monsterObjList.Add(obj);
            }
        }
        else
        {
            monsterDataList = new List<MonsterCardData>();
        }
    }
    #endregion
    #region DeckDetailUI
    private void OnClickSaveDeck()
    {
        var deckName = deckDetailUI.ifDeckName.text;
        if(string.IsNullOrEmpty(deckName) == true)
        {
            Debug.LogWarning("Deck name is empty");
            return;
        }
        deckDetailUI.panel.SetActive(false);
        chooseDeckUI.panel.SetActive(true);
        var nData = new LoadDeckData();
        nData.deckName = deckName;
        if (eventDataList.Count > 0) DataHelper.JsonSerializeData(eventDataList, deckName, "EventCards");
        if (destinyDataList.Count > 0) DataHelper.JsonSerializeData(destinyDataList, deckName, "DestinyCards");
        if (monsterDataList.Count > 0) DataHelper.JsonSerializeData(monsterDataList, deckName, "MonsterCards");
        if (loadDeckDatas != null)
        {
            foreach(var cData in loadDeckDatas)
            {
                if (deckName == cData.deckName)
                    return;
            }
        }
        loadDeckDatas.Add(nData);
        DataHelper.JsonSerializeData(loadDeckDatas, string.Empty, "DeckDatas");
    }
    private void OnClickNewCard()
    {
        var cardMakePanel = MainSystem.Instance.UIManager.GetPanel<CardMakePanel>();
        cardMakePanel.ShowDefault();
        Hide();
    }
    private void OnClickCanel()
    {
        deckDetailUI.panel.SetActive(false);
        chooseDeckUI.panel.SetActive(true);
    }
    #endregion
    public bool AddEventCard(EventCardData data)
    {
        if (eventDataList == null) return false;
        for(var i = 0; i < eventDataList.Count; i++)
        {
            if (eventDataList[i].id == data.id)
            {
                eventDataList[i] = data;
                UpdateCardList();
                return true;
            }
        }
        if (eventDataList.Count >= 30) return false;
        data.id = eventDataList.Count + 1;
        data.title = "Event Card";
        eventDataList.Add(data);
        UpdateCardList();
        return true;
    }
    public bool AddDestinyCard(DestinyCardData data)
    {
        if (destinyDataList == null) return false;
        for (var i = 0; i < destinyDataList.Count; i++)
        {
            if (destinyDataList[i].id == data.id)
            {
                destinyDataList[i] = data;
                UpdateCardList();
                return true;
            }
        }
        if(destinyDataList.Count >= 30) return false;
        data.id = destinyDataList.Count + 1;
        data.title = "Destiny Card";
        destinyDataList.Add(data);
        UpdateCardList();
        return true;
    }
    public bool AddMonsterCard(MonsterCardData data)
    {
        if (monsterDataList == null) return false;
        for (var i = 0; i < monsterDataList.Count; i++)
        {
            if (monsterDataList[i].id == data.id)
            {
                monsterDataList[i] = data;
                UpdateCardList();
                return true;
            }
        }
        if (monsterDataList.Count >= 30) return false;
        data.id = monsterDataList.Count + 1;
        data.title = "Monster Card";
        monsterDataList.Add(data);
        UpdateCardList();
        return true;
    }
}
