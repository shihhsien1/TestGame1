using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    public static MainSystem Instance = null;
    public TableDatas Tables { get; set; } = null;
    public UIManager UIManager { get; private set; } = null;
    public DataManager DataManager { get; private set; } = null;
    public List<PlayerData> PlayerDatas { get; private set; } = new List<PlayerData>();
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        Tables = new TableDatas();
        UIManager = new UIManager();
        DataManager = new DataManager();
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void UpdatePlayerData(int playerIndex, StatusType resultStatus, int val)
    {
        var playData = PlayerDatas[playerIndex];
        switch (resultStatus)
        {
            case StatusType.Power:
                playData.AddPower(val);
                break;
            case StatusType.Agility:
                playData.AddAgility(val);
                break;
            case StatusType.Intelligence:
                playData.AddIntelligence(val);
                break;
            case StatusType.Luck:
                playData.AddLuck(val);
                break;
            case StatusType.Constitution:
                playData.AddConstitution(val);
                break;
            case StatusType.MaxHitPoint:
                playData.AddMaxHP(val);
                break;
            case StatusType.HitPoint:
                playData.AddHP(val);
                break;
            case StatusType.ManaPoint:
                playData.AddMP(val);
                break;
            case StatusType.MaxManaPoint:
                playData.AddMaxMP(val);
                break;
        }
    }

    public void AddPlayerItem(int playerIndex, bool isRare, int id)
    {
        var playerData = PlayerDatas[playerIndex];
        if(isRare == true)
            playerData.RareItemIdList.Add(id);
        else
            playerData.ItemIdList.Add(id);
    }

    public void RemovePlayerItem(int playerIndex, bool isRare, int id) 
    {
        var playerData = PlayerDatas[playerIndex];
        if (isRare == true)
        {
            if (playerData.RareItemIdList.Contains(id) == false) return;
            if(playerData.EquipWeaponId == id)
            {
                playerData.EquipWeaponId = 0;
                var cardData = DataManager.GetRarityItem(id);
                UpdatePlayerData(playerIndex, cardData.addType, -cardData.addValue);
            }
            if (playerData.EquipArmorId == id)
            {
                playerData.EquipArmorId = 0;
                var cardData = DataManager.GetRarityItem(id);
                UpdatePlayerData(playerIndex, cardData.addType, -cardData.addValue);
            }
            playerData.RareItemIdList.Remove(id);
        }
        else
        {
            if (playerData.ItemIdList.Contains(id) == false) return;
            playerData.ItemIdList.Remove(id);
        }
    }

}
