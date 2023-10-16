using System.Collections.Generic;
using Tables;
using System;
using System.Linq;
using System.Diagnostics;

public class DataManager
{
    public readonly int[] MapSizes = new int[] { 30, 40, 50 };
    public readonly int[] EventCardCount = new int[] { 9, 12, 15 };
    public readonly int[] DestinyCardCount = new int[] { 9, 12, 15 };
    public readonly int[] MonsterCardCount = new int[] { 9, 12, 15 };
    private List<EventCardData> eventCardDatas = null;
    private List<DestinyCardData> destinyCardDatas = null;
    private List<MonsterCardData> monsterCardDatas = null;
    private List<ItemData> normalItemDatas = null;
    private List<RarityItemData> rareItemDatas = null;
    private Dictionary<int, EventCardData> eventCardDic = new Dictionary<int, EventCardData>();
    private Dictionary<int, DestinyCardData> destinyCardDic = new Dictionary<int, DestinyCardData>();
    private Dictionary<int, MonsterCardData> monsterCardDic = new Dictionary<int, MonsterCardData>();
    private Dictionary<int, ItemData> normalItemDic = new Dictionary<int, ItemData>();
    private Dictionary<int, RarityItemData> rareItemDic = new Dictionary<int, RarityItemData>();
    private List<CardDeck> cardDecks = new List<CardDeck>();
    private int currentStep = 0;

    public DataManager() 
    {
        //eventCardDatas = JsonHelper.DeserializeData<EventCardData>("EventCards");
        //destinyCardDatas = JsonHelper.DeserializeData<DestinyCardData>("DestinyCards");
        //monsterCardDatas = JsonHelper.DeserializeData<MonsterCardData>("MonsterCards");
        normalItemDatas = DataHelper.JsonDeserializeData<ItemData>("NormalItemDatas");
        rareItemDatas = DataHelper.JsonDeserializeData<RarityItemData>("RareItemDatas");
        if(eventCardDatas != null)
        {
            foreach (var data in eventCardDatas)
            {
                if (eventCardDic.ContainsKey(data.id) == true) continue;
                eventCardDic.Add(data.id, data);
            }
        }
        if(destinyCardDatas != null)
        {
            foreach (var data in destinyCardDatas)
            {
                if (destinyCardDic.ContainsKey(data.id) == true) continue;
                destinyCardDic.Add(data.id, data);
            }
        }
        if(monsterCardDatas != null)
        {
            foreach (var data in monsterCardDatas)
            {
                if (monsterCardDic.ContainsKey(data.id) == true) continue;
                monsterCardDic.Add(data.id, data);
            }
        }
        foreach (var data in normalItemDatas)
        {
            if (normalItemDic.ContainsKey(data.id) == true) continue;
            normalItemDic.Add(data.id, data);
        }
        foreach (var data in rareItemDatas)
        {
            if (rareItemDic.ContainsKey(data.id) == true) continue;
            rareItemDic.Add(data.id, data);
        }
    }
    public void GenerateMap(MapSize mapSize)
    {
        cardDecks.Clear();
        var deckSize = MapSizes[(int)mapSize];
        var eventCount = EventCardCount[(int)mapSize];
        var destinyCount = DestinyCardCount[(int)mapSize];
        var monsterCount = MonsterCardCount[(int)mapSize];
        var rdCardTypeList = Enum.GetNames(typeof(CardType)).ToList();
        rdCardTypeList.Remove(CardType.None.ToString());
        rdCardTypeList.Remove(CardType.Village.ToString());
        for(var i = 0; i < deckSize; i++)
        {
            var rdCardIndex = UnityEngine.Random.Range(0, rdCardTypeList.Count);
            if(cardDecks.Count == 4 || cardDecks.Count == 14 || cardDecks.Count == 24 || cardDecks.Count == 34 || cardDecks.Count == 44)
            {
                cardDecks.Add(new CardDeck(CardType.Village, 0, GetCardLevel));
            }
            else if (rdCardTypeList[rdCardIndex] == "Event")
            {
                var id = GetRandomId(eventCardDatas);
                cardDecks.Add(new CardDeck(CardType.Event, id, GetCardLevel));
                eventCount--;
                if (eventCount <= 0) rdCardTypeList.Remove(CardType.Event.ToString());
            }
            else if(rdCardTypeList[rdCardIndex] == "Destiny")
            {
                var id = GetRandomId(destinyCardDatas);
                cardDecks.Add(new CardDeck(CardType.Destiny, id, GetCardLevel));
                destinyCount--;
                if (destinyCount <= 0) rdCardTypeList.Remove(CardType.Destiny.ToString());
            }
            else if (rdCardTypeList[rdCardIndex] == "Monster")
            {
                var id = GetRandomId(monsterCardDatas);
                cardDecks.Add(new CardDeck(CardType.Monster, id, GetCardLevel));
                monsterCount--;
                if (monsterCount <= 0) rdCardTypeList.Remove(CardType.Monster.ToString());
            }
        }

    }
    public CardDeck GetCard()
    {
        if (cardDecks.Count <= 0) return new CardDeck(CardType.None, 0, 0);
        var card = cardDecks[currentStep];
        currentStep++;
        return card;
    }
    public EventCardData GetEventCard(int id)
    {
        eventCardDic.TryGetValue(id, out var data);
        return data;
    }
    public DestinyCardData GetDestinyCard(int id)
    {
        destinyCardDic.TryGetValue(id, out var data);
        return data;
    }
    public MonsterCardData GetMonsterCard(int id)
    {
        monsterCardDic.TryGetValue(id, out var data);
        return data;
    }
    public ItemData GetNormalItem(int id)
    {
        normalItemDic.TryGetValue(id, out var data);
        return data;
    }
    public RarityItemData GetRarityItem(int id)
    {
        rareItemDic.TryGetValue(id, out var data);
        return data;
    }
    public ItemData GetRandomNormalItem(int level)
    {
        var list = new List<ItemData>();
        foreach(var data in normalItemDatas)
        {
            if(data.level == level)
                list.Add(data);
        }
        var index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }
    public RarityItemData GetRandomRareItem(int level)
    {
        var list = new List<RarityItemData>();
        foreach (var data in rareItemDatas)
        {
            if (data.level == level)
                list.Add(data);
        }
        var index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }
    private int GetCardLevel
    {
        get 
        {
            var cardCount = cardDecks.Count;
            if (cardCount >= 0 && cardCount < 11)
                return 1;
            else if (cardCount >= 11 && cardCount < 21)
                return 2;
            else if (cardCount >= 21 && cardCount < 31)
                return 3;
            else if (cardCount >= 31 && cardCount < 41)
                return 4;
            else if (cardCount >= 41 && cardCount < 51)
                return 5;
            else
                return 0;
        }
    }
    private int GetRandomId(List<EventCardData> cardList)
    {
        var copyList = cardList.ToList();
        foreach(var data in cardList)
        {
            if (data.level != GetCardLevel)
                copyList.Remove(data);
        }
        var rdIndex = UnityEngine.Random.Range(0, copyList.Count);
        var rdData = copyList[rdIndex];
        return rdData.id;
    }
    private int GetRandomId(List<DestinyCardData> cardList)
    {
        var copyList = cardList.ToList();
        foreach (var data in cardList)
        {
            if (data.level != GetCardLevel)
                copyList.Remove(data);
        }
        var rdIndex = UnityEngine.Random.Range(0, copyList.Count);
        var rdData = copyList[rdIndex];
        return rdData.id;
    }
    private int GetRandomId(List<MonsterCardData> cardList)
    {
        var copyList = cardList.ToList();
        foreach (var data in cardList)
        {
            if (data.level != GetCardLevel)
                copyList.Remove(data);
        }
        var rdIndex = UnityEngine.Random.Range(0, copyList.Count);
        var rdData = copyList[rdIndex];
        return rdData.id;
    }

    public struct CardDeck
    {
        public CardType cardType;
        public int id;
        public int level;
        public CardDeck(CardType cardType, int id, int level)
        {
            this.cardType = cardType;
            this.id = id;
            this.level = level;
        }
    }
}

