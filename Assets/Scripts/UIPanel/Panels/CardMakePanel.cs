using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Tables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static CardMakePanel;

public class CardMakePanel : BasePanel
{
    [Serializable]
    public class EventCardUI
    {
        public GameObject panel = null;
        public Dropdown ddSuccessResultType = null;
        public Dropdown ddFailResultType = null;
        public InputField ifSuccessValue = null;
        public InputField ifFailValue = null;
        public Dropdown ddConditionType1 = null;
        public InputField ifConditionValue1 = null;
        public InputField ifSuccessDescription1 = null;
        public InputField ifFailDescription1 = null;
        public Dropdown ddConditionType2 = null;
        public InputField ifConditionValue2 = null;
        public InputField ifSuccessDescription2 = null;
        public InputField ifFailDescription2 = null;
        public Dropdown ddConditionType3 = null;
        public InputField ifConditionValue3 = null;
        public InputField ifSuccessDescription3 = null;
        public InputField ifFailDescription3 = null;
    }
    [Serializable]
    public class DestinyCardUI
    {
        public GameObject panel = null;
        public Toggle toggleEquit = null;
        public InputField ifItemId = null;
        public Dropdown ddFailType = null;
        public InputField ifFailValue = null;
        public Dropdown ddConditionType1 = null;
        public InputField ifConditionValue1 = null;
        public InputField ifSuccessDescription1 = null;
        public InputField ifFailDescription1 = null;
        public Dropdown ddConditionType2 = null;
        public InputField ifConditionValue2 = null;
        public InputField ifSuccessDescription2 = null;
        public InputField ifFailDescription2 = null;
        public Dropdown ddConditionType3 = null;
        public InputField ifConditionValue3 = null;
        public InputField ifSuccessDescription3 = null;
        public InputField ifFailDescription3 = null;
    }
    [Serializable]
    public class MonsterCardUI
    {
        public GameObject panel = null;
        public InputField ifHitPoint = null;
        public InputField ifAtk = null;
        public InputField ifAgility = null;
        public InputField ifFlee = null;
        public Toggle toggleEquit = null;
        public InputField ifItemId = null;
        public InputField ifBootyDescription = null;
    }
    [SerializeField] private InputField ifCardName = null;
    [SerializeField] private Dropdown ddCardType = null;
    [SerializeField] private Dropdown ddLevel = null;
    [SerializeField] private InputField ifExp = null;
    [SerializeField] private InputField ifDescription = null;
    [SerializeField] private Button btnSaveCard = null;
    [SerializeField] private Button btnCancelCard = null;
    [SerializeField] private EventCardUI eventCardUI = null;
    [SerializeField] private DestinyCardUI destinyCardUI = null;
    [SerializeField] private MonsterCardUI monsterCardUI = null;

    private GameObject lastPanel = null;
    private EventCardData currentEventData = new EventCardData();
    private DestinyCardData currentDestinyData = new DestinyCardData();
    private MonsterCardData currentMonsterData = new MonsterCardData();
    private CardType currentCardType = CardType.None;
    protected override void Start()
    {
        base.Start();
        //common ui event
        ifCardName?.onValueChanged.AddListener(OnNameChanged);
        ddCardType?.onValueChanged.AddListener(OnCardTypeChange);
        btnSaveCard?.onClick.AddListener(OnClickSaveToDeck);
        btnCancelCard?.onClick.AddListener(OnClickCancel);
        ddLevel?.onValueChanged.AddListener(OnLevelChange);
        ifDescription?.onValueChanged.AddListener(OnDescriptionChanged);
        ifExp?.onValueChanged.AddListener(OnExpChanged);
        //event card
        eventCardUI.ddSuccessResultType?.onValueChanged.AddListener(OnEventSuccessResultTypeChanged);
        eventCardUI.ddFailResultType?.onValueChanged.AddListener(OnEventSFailResultTypeChanged);
        eventCardUI.ifSuccessValue?.onValueChanged.AddListener(OnEventSuccessValueChanged);
        eventCardUI.ifFailValue?.onValueChanged.AddListener(OnEventFailValueChanged);
        eventCardUI.ddConditionType1?.onValueChanged.AddListener(i => OnEventConditionTypeChanged(i, 0));
        eventCardUI.ifConditionValue1?.onValueChanged.AddListener(i => OnEventConditionValueChanged(i, 0));
        eventCardUI.ifSuccessDescription1?.onValueChanged.AddListener(s => OnEventSuccessDescriptionChanged(s, 0));
        eventCardUI.ifFailDescription1?.onValueChanged.AddListener(s => OnEventFailDescriptionChanged(s, 0));
        eventCardUI.ddConditionType2?.onValueChanged.AddListener(i => OnEventConditionTypeChanged(i, 1));
        eventCardUI.ifConditionValue2?.onValueChanged.AddListener(i => OnEventConditionValueChanged(i, 1));
        eventCardUI.ifSuccessDescription2?.onValueChanged.AddListener(s => OnEventSuccessDescriptionChanged(s, 1));
        eventCardUI.ifFailDescription2?.onValueChanged.AddListener(s => OnEventFailDescriptionChanged(s, 1));
        eventCardUI.ddConditionType3?.onValueChanged.AddListener(i => OnEventConditionTypeChanged(i, 2));
        eventCardUI.ifConditionValue3?.onValueChanged.AddListener(i => OnEventConditionValueChanged(i, 2));
        eventCardUI.ifSuccessDescription3?.onValueChanged.AddListener(s => OnEventSuccessDescriptionChanged(s, 2));
        eventCardUI.ifFailDescription3?.onValueChanged.AddListener(s => OnEventFailDescriptionChanged(s, 2));
        //destiny card
        destinyCardUI.toggleEquit?.onValueChanged.AddListener(OnDestinyToggled);
        destinyCardUI.ifItemId?.onValueChanged.AddListener(OnDestinyItemIdChanged);
        destinyCardUI.ddFailType?.onValueChanged.AddListener(OnDestinyFailTypeChanged);
        destinyCardUI.ifFailValue?.onValueChanged.AddListener(OnDestinyFailValueChanged);
        destinyCardUI.ddConditionType1?.onValueChanged.AddListener(i => OnDestinyConditionTypeChanged(i, 0));
        destinyCardUI.ifConditionValue1?.onValueChanged.AddListener(i => OnDestinyConditionValueChanged(i, 0));
        destinyCardUI.ifSuccessDescription1?.onValueChanged.AddListener(s => OnDestinySuccessDescriptionChanged(s, 0));
        destinyCardUI.ifFailDescription1?.onValueChanged.AddListener(s => OnDestinyFailDescriptionChanged(s, 0));
        destinyCardUI.ddConditionType2?.onValueChanged.AddListener(i => OnDestinyConditionTypeChanged(i, 1));
        destinyCardUI.ifConditionValue2?.onValueChanged.AddListener(i => OnDestinyConditionValueChanged(i, 1));
        destinyCardUI.ifSuccessDescription2?.onValueChanged.AddListener(s => OnDestinySuccessDescriptionChanged(s, 1));
        destinyCardUI.ifFailDescription2?.onValueChanged.AddListener(s => OnDestinyFailDescriptionChanged(s, 1));
        destinyCardUI.ddConditionType3?.onValueChanged.AddListener(i => OnDestinyConditionTypeChanged(i, 2));
        destinyCardUI.ifConditionValue3?.onValueChanged.AddListener(i => OnDestinyConditionValueChanged(i, 2));
        destinyCardUI.ifSuccessDescription3?.onValueChanged.AddListener(s => OnDestinySuccessDescriptionChanged(s, 2));
        destinyCardUI.ifFailDescription3?.onValueChanged.AddListener(s => OnDestinyFailDescriptionChanged(s, 2));
        //monster card
        monsterCardUI.ifHitPoint?.onValueChanged.AddListener(OnMonsterHPChanged);
        monsterCardUI.ifAtk?.onValueChanged.AddListener(OnMonsterAtkChanged);
        monsterCardUI.ifAgility?.onValueChanged.AddListener(OnMonsterAgilityChanged);
        monsterCardUI.ifFlee?.onValueChanged.AddListener(OnMonsterFleeChanged);
        monsterCardUI.toggleEquit?.onValueChanged.AddListener(OnMonsterIsEquit);
        monsterCardUI.ifItemId?.onValueChanged.AddListener(OnMonsterItemIdChanged);
        monsterCardUI.ifBootyDescription?.onValueChanged.AddListener(OnMonsterBootyDescriptionChanged);

        Hide();
    }
    #region common callbacks
    private void OnCardTypeChange(int index)
    {
        if(lastPanel != null) lastPanel.SetActive(false);
        switch (index)
        {
            case 0:
                currentCardType = CardType.Event;
                lastPanel = eventCardUI.panel;
                lastPanel.SetActive(true);
                currentEventData.ClearData();
                currentEventData.level = 1;
                currentEventData.successResultType = StatusType.HitPoint;
                currentEventData.failResultType = StatusType.HitPoint;
                break;
            case 1:
                currentCardType = CardType.Destiny;
                lastPanel = destinyCardUI.panel;
                lastPanel.SetActive(true);
                currentDestinyData.ClearData();
                currentDestinyData.level = 1;
                currentDestinyData.failStatusType = StatusType.HitPoint;
                break;
            case 2:
                currentCardType = CardType.Monster;
                lastPanel = monsterCardUI.panel;
                lastPanel.SetActive(true);
                currentMonsterData.ClearData();
                currentMonsterData.level = 1;
                break;
        }
    }
    private void OnClickSaveToDeck()
    {
        var cardDeckPanel = MainSystem.Instance.UIManager.GetPanel<CardDeckPanel>();
        AddSelectDescription();
        switch (currentCardType)
        {
            case CardType.Event:
                if(CanEventDataSave) cardDeckPanel.AddEventCard(currentEventData);
                break;
            case CardType.Destiny:
                if(CanDestinyDataSave) cardDeckPanel.AddDestinyCard(currentDestinyData);
                break;
            case CardType.Monster:
                if(CanMonsterDataSave) cardDeckPanel.AddMonsterCard(currentMonsterData);
                break;
        }
        cardDeckPanel.Show();
        Hide();
    }
    private void OnClickCancel()
    {
        var cardDeckPanel = MainSystem.Instance.UIManager.GetPanel<CardDeckPanel>();
        cardDeckPanel.Show();
        Hide();
    }
    private void OnLevelChange(int index)
    {
        switch(currentCardType)
        {
            case CardType.Event:
                currentEventData.level = index + 1;
                break;
            case CardType.Destiny:
                currentDestinyData.level = index + 1;
                break;
            case CardType.Monster:
                currentMonsterData.level = index + 1;
                break;
        }
    }
    private void OnDescriptionChanged(string contents)
    {
        switch (currentCardType)
        {
            case CardType.Event:
                currentEventData.description = contents;
                break;
            case CardType.Destiny:
                currentDestinyData.description = contents;
                break;
            case CardType.Monster:
                currentMonsterData.description = contents;
                break;
        }
    }
    private void OnExpChanged(string val)
    {
        if (int.TryParse(val, out var cVal) == false) return;
        switch (currentCardType)
        {
            case CardType.Event:
                currentEventData.exp = cVal;
                break;
            case CardType.Destiny:
                currentDestinyData.exp = cVal;
                break;
            case CardType.Monster:
                currentMonsterData.exp = cVal;
                break;
        }
    }
    private void OnNameChanged(string val)
    {
        switch (currentCardType)
        {
            case CardType.Event:
                currentEventData.name = val;
                break;
            case CardType.Destiny:
                currentDestinyData.name = val;
                break;
            case CardType.Monster:
                currentMonsterData.name = val;
                break;
        }
    }
    #endregion
    #region Event Card callbacks
    private void OnEventSuccessResultTypeChanged(int index)
    {
        currentEventData.successResultType = (StatusType)index + 1;
    }
    private void OnEventSFailResultTypeChanged(int index)
    {
        currentEventData.failResultType = (StatusType)index + 1;
    }
    private void OnEventSuccessValueChanged(string val)
    {
        if(int.TryParse(val, out var cVal))
            currentEventData.successValue = cVal;
    }
    private void OnEventFailValueChanged(string val)
    {
        if(int.TryParse(val, out var cVal))
            currentEventData.failedValue = cVal;
    }
    private void OnEventConditionTypeChanged(int optionIndex, int index)
    {
        var selectVal = (ConditionType)optionIndex;
        if (currentEventData.conditionType.Count < index + 1)
        {
            if (selectVal == ConditionType.None) return;
            currentEventData.conditionType.Add(selectVal);
        }
        else
        {
            if (selectVal == ConditionType.None) 
                currentEventData.conditionType.RemoveAt(index);
            else
                currentEventData.conditionType[index] = (ConditionType)optionIndex;
        }
    }
    private void OnEventConditionValueChanged(string val, int index)
    {
        if(int.TryParse(val, out var cVal))
        {
            if (currentEventData.conditionValue.Count < index + 1)
                currentEventData.conditionValue.Add(cVal);
            else
                currentEventData.conditionValue[index] = cVal;
        }
    }
    private void OnEventSuccessDescriptionChanged(string content, int index)
    {
        if (currentEventData.successDescription.Count < index + 1)
            currentEventData.successDescription.Add(content);
        else
            currentEventData.successDescription[index] = content;
    }
    private void OnEventFailDescriptionChanged(string content, int index)
    {
        if (currentEventData.failDescription.Count < index + 1)
            currentEventData.failDescription.Add(content);
        else
            currentEventData.failDescription[index] = content;
    }
    #endregion
    #region Destiny Card callbacks
    private void OnDestinyToggled(bool isOn)
    {
        currentDestinyData.isRarity = isOn;
    }
    private void OnDestinyItemIdChanged(string id)
    {
        if(int.TryParse(id, out var cVal))
            currentDestinyData.successItemId = cVal;
    }
    private void OnDestinyFailTypeChanged(int index)
    {
        currentDestinyData.failStatusType = (StatusType)(index + 1);
    }
    private void OnDestinyFailValueChanged(string val)
    {
        if(int.TryParse(val, out var cVal))
            currentDestinyData.failValue= cVal;
    }
    private void OnDestinyConditionTypeChanged(int optionIndex, int index)
    {
        var selectVal = (ConditionType)optionIndex;
        if (currentDestinyData.conditionType.Count < index + 1)
        {
            if (selectVal == ConditionType.None) return;
            currentDestinyData.conditionType.Add(selectVal);
        }
        else
        {
            if (selectVal == ConditionType.None)
                currentDestinyData.conditionType.RemoveAt(index);
            else
                currentDestinyData.conditionType[index] = (ConditionType)optionIndex;
        }
    }
    private void OnDestinyConditionValueChanged(string val, int index)
    {
        if (int.TryParse(val, out var cVal))
        {
            if (currentDestinyData.conditionValue.Count < index + 1)
                currentDestinyData.conditionValue.Add(cVal);
            else
                currentDestinyData.conditionValue[index] = cVal;
        }
    }
    private void OnDestinySuccessDescriptionChanged(string content, int index)
    {
        if (currentDestinyData.successDescription.Count < index + 1)
            currentDestinyData.successDescription.Add(content);
        else
            currentDestinyData.successDescription[index] = content;
    }
    private void OnDestinyFailDescriptionChanged(string content, int index)
    {
        if (currentDestinyData.failDescription.Count < index + 1)
            currentDestinyData.failDescription.Add(content);
        else
            currentDestinyData.failDescription[index] = content;
    }
    #endregion
    #region Monster Card callbacks
    private void OnMonsterHPChanged(string val)
    {
        if (int.TryParse(val, out var cVal))
            currentMonsterData.hitPoint = cVal;
    }
    private void OnMonsterAtkChanged(string val)
    {
        if (int.TryParse(val, out var cVal))
            currentMonsterData.attack = cVal;
    }
    private void OnMonsterAgilityChanged(string val)
    {
        if (int.TryParse(val, out var cVal))
            currentMonsterData.agility = cVal;
    }
    private void OnMonsterFleeChanged(string val)
    {
        if (int.TryParse(val, out var cVal))
            currentMonsterData.flee = cVal;
    }
    private void OnMonsterIsEquit(bool isOn)
    {
        currentMonsterData.isRarity = isOn;
    }
    private void OnMonsterItemIdChanged(string val)
    {
        currentMonsterData.dropItemId = Convert.ToInt32(val);
    }
    private void OnMonsterBootyDescriptionChanged(string val)
    {
        currentMonsterData.bootyDescription = val;
    }
    #endregion
    private bool CanEventDataSave
    { 
        get 
        {
            var canSave = currentEventData.conditionType.Count > 0 &&
                currentEventData.conditionValue.Count > 0 &&
                currentEventData.successDescription.Count > 0 &&
                currentEventData.failDescription.Count > 0 &&
                string.IsNullOrEmpty(currentEventData.description) == false &&
                currentEventData.successResultType != StatusType.None &&
                currentEventData.failResultType != StatusType.None;
            return true;
        }
    }
    private bool CanDestinyDataSave
    {
        get
        {
            var canSave = currentDestinyData.conditionType.Count > 0 &&
                currentDestinyData.conditionValue.Count > 0 &&
                currentDestinyData.successDescription.Count > 0 &&
                currentDestinyData.failDescription.Count > 0 &&
                string.IsNullOrEmpty(currentDestinyData.description) == false &&
                currentDestinyData.failStatusType != StatusType.None;
            return true;
        }
    }
    private bool CanMonsterDataSave
    {
        get
        {
            var canSave = string.IsNullOrEmpty(currentMonsterData.description) == false &&
                string.IsNullOrEmpty(currentMonsterData.name) == false &&
                string.IsNullOrEmpty(currentMonsterData.bootyDescription) == false &&
                currentMonsterData.hitPoint > 0;
            return true;
        }
    }
    public void ShowDefault()
    {
        OnCardTypeChange(0);
        Show();
    }
    public void ShowByEvent(EventCardData data)
    {
        currentEventData = data;
        currentCardType = CardType.Event;
        ifCardName.text = data.name;
        ddCardType.value = 0;
        ddLevel.value = data.level - 1;
        ifExp.text = data.exp.ToString();
        ifDescription.text = data.description;
        eventCardUI.ddSuccessResultType.value = ((int)data.successResultType) - 1;
        eventCardUI.ddFailResultType.value = ((int)data.failResultType) - 1;
        eventCardUI.ifSuccessValue.text = data.successValue.ToString();
        eventCardUI.ifFailValue.text = data.failedValue.ToString();
        eventCardUI.ddConditionType1.value = (int)data.conditionType[0];
        eventCardUI.ifConditionValue1.text = data.conditionValue[0].ToString();
        eventCardUI.ifSuccessDescription1.text = data.successDescription[0];
        eventCardUI.ifFailDescription1.text = data.failDescription[0];
        if(data.conditionType.Count > 1) eventCardUI.ddConditionType2.value = (int)data.conditionType[1];
        if (data.conditionValue.Count > 1) eventCardUI.ifConditionValue2.text = data.conditionValue[1].ToString();
        if(data.successDescription.Count > 1) eventCardUI.ifSuccessDescription2.text = data.successDescription[1];
        if (data.failDescription.Count > 1) eventCardUI.ifFailDescription2.text = data.failDescription[1];
        if(data.conditionType.Count > 2) eventCardUI.ddConditionType3.value = (int)data.conditionType[2];
        if(data.conditionValue.Count > 2) eventCardUI.ifConditionValue3.text = data.conditionValue[2].ToString();
        if(data.successDescription.Count > 2) eventCardUI.ifSuccessDescription3.text = data.successDescription[2];
        if (data.failDescription.Count > 2) eventCardUI.ifFailDescription3.text = data.failDescription[2];
        Show();
    }
    public void ShowByDestiny(DestinyCardData data)
    {
        currentDestinyData = data;
        currentCardType = CardType.Destiny;
        ifCardName.text = data.name;
        ddCardType.value = 0;
        ddLevel.value = data.level - 1;
        ifExp.text = data.exp.ToString();
        ifDescription.text = data.description;
        destinyCardUI.toggleEquit.isOn = data.isRarity;
        destinyCardUI.ifItemId.text = data.successItemId.ToString();
        destinyCardUI.ddFailType.value = ((int)data.failStatusType) - 1;
        destinyCardUI.ifFailValue.text = data.failValue.ToString();
        destinyCardUI.ddConditionType1.value = (int)data.conditionType[0];
        destinyCardUI.ifConditionValue1.text = data.conditionValue[0].ToString();
        destinyCardUI.ifSuccessDescription1.text = data.successDescription[0];
        destinyCardUI.ifFailDescription1.text = data.failDescription[0];
        if (data.conditionType.Count > 1) destinyCardUI.ddConditionType2.value = (int)data.conditionType[1];
        if (data.conditionValue.Count > 1) destinyCardUI.ifConditionValue2.text = data.conditionValue[1].ToString();
        if (data.successDescription.Count > 1) destinyCardUI.ifSuccessDescription2.text = data.successDescription[1];
        if (data.failDescription.Count > 1) destinyCardUI.ifFailDescription2.text = data.failDescription[1];
        if (data.conditionType.Count > 2) destinyCardUI.ddConditionType3.value = (int)data.conditionType[2];
        if (data.conditionValue.Count > 2) destinyCardUI.ifConditionValue3.text = data.conditionValue[2].ToString();
        if (data.successDescription.Count > 2) destinyCardUI.ifSuccessDescription3.text = data.successDescription[2];
        if (data.failDescription.Count > 2) destinyCardUI.ifFailDescription3.text = data.failDescription[2];
        Show();
    }
    public void ShowByMonster(MonsterCardData data)
    {
        currentMonsterData = data;
        currentCardType = CardType.Monster;
        ifCardName.text = data.name;
        ddCardType.value = 0;
        ddLevel.value = data.level - 1;
        ifExp.text = data.exp.ToString();
        ifDescription.text = data.description;
        monsterCardUI.ifHitPoint.text = data.hitPoint.ToString();
        monsterCardUI.ifAtk.text = data.attack.ToString();
        monsterCardUI.ifAgility.text = data.agility.ToString();
        monsterCardUI.ifFlee.text = data.flee.ToString();
        monsterCardUI.toggleEquit.isOn = data.isRarity;
        monsterCardUI.ifItemId.text = data.dropItemId.ToString();
        monsterCardUI.ifBootyDescription.text = data.bootyDescription;
        Show();
    }
    private void AddSelectDescription()
    {
        var msgTable = MainSystem.Instance.Tables.MessageTable;
        switch(currentCardType)
        {
            case CardType.Event:
                if(currentEventData.selectionDescription.Count > 0)
                {
                    for (var i = 0; i < currentEventData.selectionDescription.Count; i++)
                    {
                        if(i < currentEventData.conditionType.Count)
                        {
                            if (currentEventData.conditionType[i] == ConditionType.Power)
                                currentEventData.selectionDescription[i] = msgTable.GetMessageParameter(1, currentEventData.conditionValue[i]);
                            else if (currentEventData.conditionType[i] == ConditionType.Intelligence)
                                currentEventData.selectionDescription[i] = msgTable.GetMessageParameter(2, currentEventData.conditionValue[i]);
                            else if (currentEventData.conditionType[i] == ConditionType.Agility)
                                currentEventData.selectionDescription[i] = msgTable.GetMessageParameter(3, currentEventData.conditionValue[i]);
                            else if (currentEventData.conditionType[i] == ConditionType.Luck)
                                currentEventData.selectionDescription[i] = msgTable.GetMessageParameter(4, currentEventData.conditionValue[i]);
                            else if (currentEventData.conditionType[i] == ConditionType.Constitution)
                                currentEventData.selectionDescription[i] = msgTable.GetMessageParameter(5, currentEventData.conditionValue[i]);
                        }
                        else
                        {
                            currentEventData.selectionDescription.RemoveAt(i);
                        }
                    }
                }
                var diff = currentEventData.conditionType.Count - currentEventData.selectionDescription.Count;
                if (diff > 0)
                {
                    for(var i = currentEventData.selectionDescription.Count; i < currentEventData.conditionValue.Count; i++)
                    {
                        if (currentEventData.conditionType[i] == ConditionType.Power)
                            currentEventData.selectionDescription.Add(msgTable.GetMessageParameter(1, currentEventData.conditionValue[i]));
                        else if (currentEventData.conditionType[i] == ConditionType.Intelligence)
                            currentEventData.selectionDescription.Add(msgTable.GetMessageParameter(2, currentEventData.conditionValue[i]));
                        else if (currentEventData.conditionType[i] == ConditionType.Agility)
                            currentEventData.selectionDescription.Add(msgTable.GetMessageParameter(3, currentEventData.conditionValue[i]));
                        else if (currentEventData.conditionType[i] == ConditionType.Luck)
                            currentEventData.selectionDescription.Add(msgTable.GetMessageParameter(4, currentEventData.conditionValue[i]));
                        else if (currentEventData.conditionType[i] == ConditionType.Constitution)
                            currentEventData.selectionDescription.Add(msgTable.GetMessageParameter(5, currentEventData.conditionValue[i]));
                    }
                }
                break;
            case CardType.Destiny:
                if (currentDestinyData.selectionDescription.Count > 0)
                {
                    for (var i = 0; i < currentDestinyData.selectionDescription.Count; i++)
                    {
                        if (i < currentDestinyData.conditionType.Count)
                        {
                            if (currentDestinyData.conditionType[i] == ConditionType.Power)
                                currentDestinyData.selectionDescription[i] = msgTable.GetMessageParameter(1, currentDestinyData.conditionValue[i]);
                            else if (currentDestinyData.conditionType[i] == ConditionType.Intelligence)
                                currentDestinyData.selectionDescription[i] = msgTable.GetMessageParameter(2, currentDestinyData.conditionValue[i]);
                            else if (currentDestinyData.conditionType[i] == ConditionType.Agility)
                                currentDestinyData.selectionDescription[i] = msgTable.GetMessageParameter(3, currentDestinyData.conditionValue[i]);
                            else if (currentDestinyData.conditionType[i] == ConditionType.Luck)
                                currentDestinyData.selectionDescription[i] = msgTable.GetMessageParameter(4, currentDestinyData.conditionValue[i]);
                            else if (currentDestinyData.conditionType[i] == ConditionType.Constitution)
                                currentDestinyData.selectionDescription[i] = msgTable.GetMessageParameter(5, currentDestinyData.conditionValue[i]);
                        }
                        else
                        {
                            currentDestinyData.selectionDescription.RemoveAt(i);
                        }
                    }
                }
                var diff2 = currentDestinyData.conditionType.Count - currentDestinyData.selectionDescription.Count;
                if (diff2 > 0)
                {
                    for (var i = currentDestinyData.selectionDescription.Count; i < currentEventData.conditionValue.Count; i++)
                    {
                        if (currentDestinyData.conditionType[i] == ConditionType.Power)
                            currentDestinyData.selectionDescription.Add(msgTable.GetMessageParameter(1, currentDestinyData.conditionValue[i]));
                        else if (currentDestinyData.conditionType[i] == ConditionType.Intelligence)
                            currentDestinyData.selectionDescription.Add(msgTable.GetMessageParameter(2, currentDestinyData.conditionValue[i]));
                        else if (currentDestinyData.conditionType[i] == ConditionType.Agility)
                            currentDestinyData.selectionDescription.Add(msgTable.GetMessageParameter(3, currentDestinyData.conditionValue[i]));
                        else if (currentDestinyData.conditionType[i] == ConditionType.Luck)
                            currentDestinyData.selectionDescription.Add(msgTable.GetMessageParameter(4, currentDestinyData.conditionValue[i]));
                        else if (currentDestinyData.conditionType[i] == ConditionType.Constitution)
                            currentDestinyData.selectionDescription.Add(msgTable.GetMessageParameter(5, currentDestinyData.conditionValue[i]));
                    }
                }
                break;
        }
        
    }

}
