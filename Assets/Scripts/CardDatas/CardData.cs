using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tables
{
    [Serializable]
    public class EventCardData
    {
        public int id = 0;
        public int level = 0;
        public string title = string.Empty;
        public string name = string.Empty;
        public string description = string.Empty;
        public List<string> selectionDescription = new List<string>();
        public List<ConditionType> conditionType = new List<ConditionType>();
        public List<int> conditionValue = new List<int>();
        public StatusType successResultType = StatusType.None;
        public StatusType failResultType = StatusType.None;
        public int successValue = 0;
        public int failedValue = 0;
        public int exp = 0;
        public List<string> successDescription = new List<string>();
        public List<string> failDescription = new List<string>();

        public void ClearData()
        {
            id = 0;
            level = 0;
            title = string.Empty;
            name = string.Empty;
            description = string.Empty;
            selectionDescription.Clear();
            conditionType.Clear();
            conditionValue.Clear();
            successResultType = StatusType.None;
            failResultType = StatusType.None;
            successValue = 0;
            failedValue = 0;
            exp = 0;
            successDescription.Clear();
            failDescription.Clear();
        }
    }
    [Serializable]
    public class MonsterCardData
    {
        public int id = 0;
        public int level = 0;
        public string title = string.Empty;
        public string name = string.Empty;
        public int hitPoint = 0;
        public int attack = 0;
        public int agility = 0;
        public int flee = 0;
        public int physicalResistance = 0;
        public int magicalResistance = 0;
        public bool isRarity = false;
        public int dropItemId = 0;
        public int exp = 0;
        public string description = string.Empty;
        public string bootyDescription = string.Empty;

        public void ClearData()
        {
            id = 0;
            level = 0;
            title = string.Empty;
            name = string.Empty;
            hitPoint = 0;
            attack = 0;
            agility = 0;
            flee = 0;
            physicalResistance = 0;
            magicalResistance = 0;
            isRarity = false;
            dropItemId = 0;
            exp = 0;
            description = string.Empty;
            bootyDescription = string.Empty;
        }
    }
    [Serializable]
    public class DestinyCardData
    {
        public int id = 0;
        public int level = 0;
        public string title = string.Empty;
        public string name = string.Empty;
        public List<ConditionType> conditionType = new List<ConditionType>();
        public List<int> conditionValue = new List<int>();
        public bool isRarity = false;
        public int successItemId = 0;
        public StatusType failStatusType = StatusType.None;
        public int failValue = 0;
        public int exp = 0;
        public List<string> selectionDescription = new List<string>();
        public string description = string.Empty;
        public List<string> successDescription = new List<string>();
        public List<string> failDescription = new List<string>();

        public void ClearData()
        {
            id = 0;
            level = 0;
            title = string.Empty;
            name = string.Empty;
            conditionType.Clear();
            conditionValue.Clear();
            isRarity = false;
            successItemId = 0;
            failStatusType = StatusType.None;
            failValue = 0;
            exp = 0;
            selectionDescription.Clear();
            description = string.Empty;
            successDescription.Clear();
            failDescription.Clear();
        }
    }
    [Serializable]
    public class ItemData
    {
        public int id = 0;
        public int level = 0;
        public string name = string.Empty;
        public string description = string.Empty;
        public StatusType addType = StatusType.None;
        public int addValue = 0;
        public int buy = 0;
        public int sell = 0;
    }
    [Serializable]
    public class RarityItemData
    {
        public int id = 0;
        public int level = 0;
        public string name = string.Empty;
        public string description = string.Empty;
        public int rarity = 0;
        public StatusType addType = StatusType.None;
        public int addValue = 0;
        public int buy = 0;
        public int sell = 0;
        public EquipType equipType = EquipType.None;
    }

}

