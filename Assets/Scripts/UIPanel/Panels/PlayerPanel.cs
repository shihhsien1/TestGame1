using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerPanel : BasePanel
{
    [Header("UI")]
    [SerializeField] private Text textName = null;
    [SerializeField] private Image imgHP = null;
    [SerializeField] private Image imgMP = null;
    [SerializeField] private Text textHP = null;
    [SerializeField] private Text textMP = null;
    [SerializeField] private Button btnStatus = null;
    [SerializeField] private ScrollRect srCards = null;
    [Header("Others")]
    [SerializeField] private CardButton cardElement = null;

    private List<CardButton> itemList = new List<CardButton>();

    protected override void Start()
    {
        base.Start();
        btnStatus.onClick.AddListener(() => MainSystem.Instance.UIManager.GetPanel<StatusPanel>().ShowPanel());
    }

    public string Name
    {
        get { return textName.text; }
        set { textName.text = value; }
    }

    public void UpdateHP(int playerIndex)
    {
        var hp = MainSystem.Instance.PlayerDatas[playerIndex].HitPoint;
        var maxHP = MainSystem.Instance.PlayerDatas[playerIndex].MaxHitPoint;
        var amount = (float)hp / maxHP;
        imgHP.fillAmount = amount;
        textHP.text = $"{hp}/{maxHP}";
    }

    public void UpdateMP(int playerIndex)
    {
        var mp = MainSystem.Instance.PlayerDatas[playerIndex].ManaPoint;
        var maxMP = MainSystem.Instance.PlayerDatas[playerIndex].MaxManaPoint;
        var amount = (float)mp / maxMP;
        imgMP.fillAmount = amount;
        textMP.text = $"{mp}/{maxMP}";
    }

    public (bool, bool) AddCard(int playerIndex, int itemId, bool isRare)
    {
        var data = GetContents(itemId, isRare);
        if(string.IsNullOrEmpty(data.Item1) == true) return (false, false);
        var emptyCard = GetEmptyCard();
        if (emptyCard == null) return (true, true);
        emptyCard.UpdateData(itemId, isRare);
        emptyCard.title.text = data.Item1;
        emptyCard.description.text = data.Item2;
        emptyCard.gameObject.SetActive(true);
        if(isRare == false)
        {
            var cardData = MainSystem.Instance.DataManager.GetNormalItem(itemId);
            emptyCard.button.onClick.AddListener(() => OnClickUseItem(emptyCard, cardData, playerIndex));
        }
        else
        {
            var cardData = MainSystem.Instance.DataManager.GetRarityItem(itemId);
            emptyCard.button.onClick.AddListener(() => OnClickEquip(emptyCard, cardData, playerIndex));
        }
        return (true, false);
    }

    public void RemoveCard(int playerIndex, int id, bool isRare)
    {
        foreach(var data in itemList)
        {
            if(data.itemId == id && data.isRare == isRare)
            {
                data.gameObject.SetActive(false);
                break;
            }
        }
    }

    private void OnClickUseItem(CardButton btn, ItemData itemData, int playerIndex)
    {
        var ms = MainSystem.Instance;
        ms.UpdatePlayerData(playerIndex, itemData.addType, itemData.addValue);
        btn.Clear();
        MainSystem.Instance.RemovePlayerItem(playerIndex, false, itemData.id);
    }

    private void OnClickEquip(CardButton btn, RarityItemData cardData, int playerIndex)
    {
        var ms = MainSystem.Instance;
        var playerData = ms.PlayerDatas[playerIndex];
        switch (cardData.equipType)
        {
            case EquipType.Weapon:
                if(playerData.EquipWeaponId != 0)
                {
                    var equipWeapon = ms.DataManager.GetRarityItem(playerData.EquipWeaponId);
                    ms.UpdatePlayerData(playerIndex, equipWeapon.addType, -equipWeapon.addValue);
                    if (playerData.Classes == Classes.Warrior) playerData.AddPower(-1);
                    else if (playerData.Classes == Classes.Magician) playerData.AddIntelligence(-1);
                }
                if (playerData.Classes == Classes.Warrior) playerData.AddPower(1);
                else if (playerData.Classes == Classes.Magician) playerData.AddIntelligence(1);
                playerData.EquipWeaponId = cardData.id;
                break;
            case EquipType.Armor:
                if (playerData.EquipArmorId != 0)
                {
                    var equipArmor = ms.DataManager.GetRarityItem(playerData.EquipArmorId);
                    ms.UpdatePlayerData(playerIndex, equipArmor.addType, -equipArmor.addValue);
                    if (playerData.Exceed == Exceed.Elf) playerData.AddAgility(-1);
                    else if (playerData.Exceed == Exceed.DarkElf) playerData.AddIntelligence(-1);
                }
                playerData.EquipArmorId = cardData.id;
                if (playerData.Exceed == Exceed.Elf) playerData.AddAgility(1);
                else if (playerData.Exceed == Exceed.DarkElf) playerData.AddIntelligence(1);
                break;
        }
        ms.UpdatePlayerData(playerIndex, cardData.addType, cardData.addValue);
    }

    public void ChangeToSelectCard(int playerIndex, int itemId, bool isRare)
    {
        foreach (var card in itemList)
        {
            if (card.gameObject.activeSelf == false) continue;
            card.button.onClick.RemoveAllListeners();
            card.button.onClick.AddListener(() =>
            {
                var comfirmPanel = MainSystem.Instance.UIManager.GetPanel<ConfirmPanel>();
                comfirmPanel.ShowContent("is change the card?", () =>
                {
                    var data = GetContents(itemId, isRare);
                    card.Clear();
                    card.UpdateData(itemId, isRare);
                    card.title.text = data.Item1;
                    card.description.text = data.Item2;
                    card.gameObject.SetActive(true);
                    Events.OnCardChangeDone.Fire();
                });
            });
        }
    }

    private (string, string) GetContents(int id, bool isRare)
    {
        var ms = MainSystem.Instance;
        var name = string.Empty;
        var description = string.Empty;
        if (isRare == true)
        {
            var data = ms.DataManager.GetRarityItem(id);
            if(data == null) return (name, description);
            name = data.name;
            description = data.description;
        }
        else
        {
            var data = ms.DataManager.GetNormalItem(id);
            if (data == null) return (name, description);
            name = data.name;
            description = data.description;
        }
        return (name, description);
    }

    private CardButton GetEmptyCard()
    {
        //先從現有卡池找已關閉的物件
        foreach (var card in itemList)
        {
            if (card.gameObject.activeSelf == false)
                return card;
        }
        if(itemList.Count < 5)
        {
            var card = Instantiate(cardElement, srCards.content);
            return card;
        }
        return null;
    }

}
