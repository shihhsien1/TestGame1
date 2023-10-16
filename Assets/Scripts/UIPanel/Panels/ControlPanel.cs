using System;
using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : BasePanel
{
    [Header("UI")]
    [SerializeField] private GameObject btnsPanel = null;
    [SerializeField] private List<Sprite> diceImgList = new List<Sprite>();
    [SerializeField] private Image imgShowDice = null;
    [SerializeField] private List<ButtonObjects> btnList = new List<ButtonObjects>();
    [SerializeField] private Text textTitle = null;
    [SerializeField] private Text textDescription = null;
    [Header("values")]
    [SerializeField] private float typeWriteSpeed = 0.1f;
    private Coroutine currentCoroutine = null;
    private WaitForSeconds waitSec = new WaitForSeconds(0.1f);
    private WaitForSeconds waitTypeWrite = null;
    private WaitForSeconds waitOneSec = new WaitForSeconds(1);
    private List<Action> btnClickCallbacks = new List<Action>();
    private List<string> btnTextList = new List<string>();
    private int diceValue = 0;
    private MonsterCardData battleMonster = null;
    private bool isPlayerDead = false;

    protected override void Start()
    {
        base.Start();
        EventHelper.On(this);
        waitTypeWrite = new WaitForSeconds(typeWriteSpeed);
        btnsPanel.SetActive(false);
        Hide();
    }

    private void OnDestroy()
    {
        EventHelper.Off(this);
    }

    private void OnClickThrowDice(Action callback = null)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ChangeDiceImage(callback));
    }

    public void StartNewGame()
    {
        Show();
        StartCoroutine(ShowDescription("", "A new journey is start", DrawMapCard));
    }

    public void DrawMapCard()
    {
        if (isPlayerDead == true) return;
        ClearBtns();
        btnsPanel.SetActive(true);
        var obj = btnList[0];
        obj.gameObject.SetActive(true);
        obj.Text.text = "draw the map card";
        obj.Button.onClick.AddListener(() => 
        {
            var dm = MainSystem.Instance.DataManager;
            var card = dm.GetCard();
            btnsPanel.SetActive(false);
            switch (card.cardType)
            {
                case CardType.Event:
                    var ec = dm.GetEventCard(card.id);
                    StartCoroutine(ShowDescription(ec.title, ec.description, () => ExcuteEvent(ec)));
                    break;
                case CardType.Destiny:
                    var dc = dm.GetDestinyCard(card.id);
                    StartCoroutine(ShowDescription(dc.title, dc.description, () => ExcuteDestiny(dc)));
                    break;
                case CardType.Monster:
                    battleMonster = dm.GetMonsterCard(card.id);
                    var enemyPanel = MainSystem.Instance.UIManager.GetPanel<EnemyPanel>();
                    enemyPanel.Initialize(battleMonster);
                    StartCoroutine(ShowDescription(battleMonster.title, battleMonster.description, SelectToBattle));
                    break;
                case CardType.Village:
                    var villagePanel = MainSystem.Instance.UIManager.GetPanel<VillagePanel>();
                    StartCoroutine(ShowDescription("Village", "You come to a village", () => villagePanel.ShowWindow(card.level, 0)));
                    break;
            }
        });
    }

    private void ExcuteEvent(EventCardData card)
    {
        btnClickCallbacks.Clear();
        btnTextList.Clear();
        var index = 0;
        for (var i = 0; i < card.conditionType.Count; i++)
        {
            index = i;
            var selectDes = string.Format(card.selectionDescription[index], card.conditionValue[index]);
            btnTextList.Add(selectDes);
            btnClickCallbacks.Add(() => OnClickThrowDice(() => 
            {
                var result = ConditionResult(card.conditionType[index], card.conditionValue[index]);
                btnsPanel.SetActive(false);
                var ms = MainSystem.Instance;
                var playerPanel = ms.UIManager.GetPanel<PlayerPanel>();
                var resultType = StatusType.None;
                var val = 0;
                var msg = string.Empty;
                if (result == true)
                {
                    resultType = card.successResultType;
                    val = card.successValue;
                    msg = string.Format(card.successDescription[index], val);
                    if (ms.PlayerDatas[0].Classes == Classes.Monk) ms.PlayerDatas[0].AddHP(1);
                }
                else
                {
                    resultType = card.failResultType;
                    val = card.failedValue;
                    if (ms.PlayerDatas[0].Exceed == Exceed.Dwarf) val++;
                    msg = string.Format(card.failDescription[index], val);
                }
                StartCoroutine(ShowDescription(
                        card.title,
                        msg,
                        () =>
                        {
                            ms.UpdatePlayerData(0, resultType, val);
                            ms.PlayerDatas[0].AddExp(card.exp);
                            playerPanel.UpdateHP(0);
                            playerPanel.UpdateMP(0);
                            DrawMapCard();
                        }));
            }));
        }
        CreateBtns();
    }

    private void ExcuteDestiny(DestinyCardData card)
    {
        btnClickCallbacks.Clear();
        btnTextList.Clear();
        var index = 0;
        for (var i = 0; i < card.conditionType.Count; i++)
        {
            index = i;
            var selectDes = string.Format(card.selectionDescription[index], card.conditionValue[index]);
            btnTextList.Add(selectDes);
            btnClickCallbacks.Add(() => OnClickThrowDice(() =>
            {
                var result = ConditionResult(card.conditionType[index], card.conditionValue[index]);
                btnsPanel.SetActive(false);
                var ms = MainSystem.Instance;
                var playerPanel = ms.UIManager.GetPanel<PlayerPanel>();
                if (result == true)
                {
                    if (ms.PlayerDatas[0].Classes == Classes.Monk) ms.PlayerDatas[0].AddHP(1);
                    var successMsg = string.Format(card.successDescription[index], card.successItemId);
                    StartCoroutine(ShowDescription(
                        card.title,
                        successMsg,
                        () => AddCard(card.successItemId, card.isRarity, DrawMapCard)));
                }
                else
                {
                    var failMsg = string.Format(card.failDescription[index], card.failValue);
                    StartCoroutine(ShowDescription(
                        card.title,
                        failMsg,
                        () => 
                        {
                            ms.UpdatePlayerData(0, card.failStatusType, card.failValue);
                            playerPanel.UpdateHP(0);
                            playerPanel.UpdateMP(0);
                            DrawMapCard();
                        }));
                }
                ms.PlayerDatas[0].AddExp(card.exp);
            }));
        }
        CreateBtns();
    }

    private bool ConditionResult(ConditionType conditionType, int passVal)
    {
        var playerData = MainSystem.Instance.PlayerDatas[0];
        var resultVal = 0;
        switch (conditionType)
        {
            case ConditionType.Power:
                resultVal = playerData.Power * diceValue;
                break;
            case ConditionType.Agility:
                resultVal = playerData.Agility * diceValue;
                if (playerData.Classes == Classes.Ranger) passVal -= 1;
                break;
            case ConditionType.Intelligence:
                resultVal = playerData.Intelligence * diceValue;
                break;
            case ConditionType.Luck:
                resultVal = playerData.Luck * diceValue;
                break;
        }
        var isSucess = resultVal >= passVal;
        return isSucess;
    }

    private IEnumerator ChangeDiceImage(Action callback = null)
    {
        var count = 0;
        while (count < 10)
        {
            count++;
            RandomDiceValue();
            yield return waitSec;
        }
        yield return waitOneSec;
        callback?.Invoke();
    }

    private void RandomDiceValue()
    {
        var index = UnityEngine.Random.Range(0, diceImgList.Count);
        imgShowDice.sprite = diceImgList[index];
        diceValue = index + 1;
    }

    private void SelectToBattle()
    {
        if (isPlayerDead == true) return;
        StartCoroutine(ShowDescription("Battle", "Now your turn, select your fight mothed", () =>
        {
            btnClickCallbacks.Clear();
            btnTextList.Clear();
            var enemyPanel = MainSystem.Instance.UIManager.GetPanel<EnemyPanel>();
            var playerData = MainSystem.Instance.PlayerDatas[0];
            btnTextList.Add("Use physic to battle, dice agility to hit");
            btnTextList.Add("Use magic to battle, dice agility to hit");
            btnClickCallbacks.Add(() => OnClickThrowDice(() =>
            {
                var hitVal = playerData.Agility * diceValue;
                var hitResult = enemyPanel.UpdateHP(-playerData.PhysicAtk, hitVal);
                btnsPanel.SetActive(false);
                //已擊殺
                if (hitResult.Item1 == true)
                {
                    StartCoroutine(ShowDescription("Battle", battleMonster.bootyDescription, 
                        () => 
                        {
                            AddCard(battleMonster.dropItemId, battleMonster.isRarity, DrawMapCard);
                            playerData.AddExp(battleMonster.exp);
                        }));
                }
                else
                {
                    //打中
                    var msg = hitResult.Item2 == true ? $"Hit!! {battleMonster.name} damaged {playerData.PhysicAtk}" : $"Miss!! {battleMonster.name} was dodged.";
                    StartCoroutine(ShowDescription("Battle", msg, MonsterAttack));
                }
            }));
            btnClickCallbacks.Add(() => OnClickThrowDice(() =>
            {
                var hitVal = playerData.Agility * diceValue;
                var hitResult = enemyPanel.UpdateHP(-playerData.MagicAtk, hitVal);
                btnsPanel.SetActive(false);
                //已擊殺
                if (hitResult.Item1 == true)
                {
                    StartCoroutine(ShowDescription("Battle", battleMonster.bootyDescription,
                        () => AddCard(battleMonster.dropItemId, battleMonster.isRarity, DrawMapCard)));
                }
                else
                {
                    //打中
                    var msg = hitResult.Item2 == true ? $"Hit!! {battleMonster.name} damaged {playerData.MagicAtk}" : $"Miss!! {battleMonster.name} was dodged.";
                    StartCoroutine(ShowDescription("Battle", msg, MonsterAttack));
                }
            }));
            CreateBtns();
        }));
    }

    private void MonsterAttack()
    {
        var playerData = MainSystem.Instance.PlayerDatas[0];
        StartCoroutine(ShowDescription(
            "Battle", 
            $"{battleMonster.name} is going to attack you",
            () => OnClickThrowDice(() => 
            {
                var hitVal = battleMonster.attack * diceValue;
                var hitResult = playerData.BattleHitCondition(-battleMonster.attack, hitVal);
                var playerPanel = MainSystem.Instance.UIManager.GetPanel<PlayerPanel>();
                playerPanel.UpdateHP(0);
                playerPanel.UpdateMP(0);
                var msg = hitResult ? $"You has been hit, damage : {battleMonster.attack}" : $"You dodge monster attack";
                if (isPlayerDead == true) msg = "You are dead";
                btnsPanel.SetActive(false);
                StartCoroutine(ShowDescription("Battle", msg, SelectToBattle));
            })));
    }

    public void AddCard(int id, bool isRare, Action callback = null)
    {
        var ms = MainSystem.Instance;
        var playerPanel = ms.UIManager.GetPanel<PlayerPanel>();
        var addResult = playerPanel.AddCard(0, id, isRare);
        if (addResult.Item1 == true)
        {
            if (addResult.Item2 == true)
            {
                var comfirmPanel = ms.UIManager.GetPanel<ConfirmPanel>();
                comfirmPanel.ShowContent(
                    "change card from your storage?",
                    () =>
                    {
                        playerPanel.ChangeToSelectCard(0, id, isRare);
                        callback?.Invoke();
                    },
                    callback);
            }
            else
            {
                callback?.Invoke();
            }
            ms.AddPlayerItem(0, isRare, id);
        }
        else
        {
            callback?.Invoke();
        }
    }

    public IEnumerator ShowDescription(string title, string content, Action callback = null)
    {
        textTitle.text = title;
        for(var i = 0; i <= content.Length; i++)
        {
            textDescription.text = content.Substring(0, i);
            yield return waitTypeWrite;
        }
        yield return waitOneSec;
        callback?.Invoke();
    }

    public void CreateBtns()
    {
        ClearBtns();
        btnsPanel.SetActive(true);
        for (var i = 0; i < btnClickCallbacks.Count; i++)
        {
            var obj = btnList[i];
            obj.gameObject.SetActive(true);
            var callback = btnClickCallbacks[i];
            obj.Button.onClick.AddListener(() => { callback?.Invoke(); });
            obj.Text.text = btnTextList[i];
        }
    }

    private void ClearBtns()
    {
        foreach (var btn in btnList)
        {
            btn.gameObject.SetActive(false);
            btn.Button.onClick.RemoveAllListeners();
        }
    }

    private void OnCardChangeDone(Events.OnCardChangeDone e)
    {
        DrawMapCard();
    }

    private void OnPlayerDead(Events.OnPlayDead e)
    {
        isPlayerDead = true;
    }

}
