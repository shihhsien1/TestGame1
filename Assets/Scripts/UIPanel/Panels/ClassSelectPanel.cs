using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectPanel : BasePanel
{
    [SerializeField] private InputField ifName = null;
    [SerializeField] private Toggle btnWarrior = null;
    [SerializeField] private Toggle btnRanger = null;
    [SerializeField] private Toggle btnMagician = null;
    [SerializeField] private Toggle btnMonk = null;
    [SerializeField] private Toggle btnMerchant = null;
    [SerializeField] private Text textPower = null;
    [SerializeField] private Text textIntelligence = null;
    [SerializeField] private Text textAgility = null;
    [SerializeField] private Text textLuck = null;
    [SerializeField] private Text textConstitution = null;
    [SerializeField] private Button btnConfirm = null;

    private PlayerData playerData = null;
    private int power = 0;
    private int intelligence = 0;
    private int agility = 0;
    private int luck = 0;
    private int constitution = 0;
    private Classes classes = Classes.None;
    private Exceed exceed = Exceed.None;
    protected override void Start()
    {
        base.Start();
        btnWarrior.onValueChanged.AddListener(OnClickWarrior);
        btnRanger.onValueChanged.AddListener(OnClickRanger);
        btnMagician.onValueChanged.AddListener(OnClickMagician);
        btnMonk.onValueChanged.AddListener(OnClickMonk);
        btnMerchant.onValueChanged.AddListener(OnClickMerchant);
        btnConfirm.onClick.AddListener(OnClickConfirm);
        OnClickWarrior(true);
    }

    private void OnClickWarrior(bool isOn)
    {
        if (isOn == false) return;
        power = 3;
        intelligence = 1;
        agility = 2;
        luck = 1;
        constitution = 3;
        classes = Classes.Warrior;
        UpdateUI();
    }

    private void OnClickRanger(bool isOn)
    {
        if (isOn == false) return;
        power = 2;
        intelligence = 1;
        agility = 3;
        luck = 2;
        constitution = 2;
        classes = Classes.Ranger;
        UpdateUI();
    }

    private void OnClickMagician(bool isOn)
    {
        if (isOn == false) return;
        power = 1;
        intelligence = 4;
        agility = 2;
        luck = 1;
        constitution = 2;
        classes = Classes.Magician;
        UpdateUI();
    }

    private void OnClickMonk(bool isOn)
    {
        if (isOn == false) return;
        power = 1;
        intelligence = 3;
        agility = 2;
        luck = 1;
        constitution = 3;
        classes = Classes.Monk;
        UpdateUI();
    }

    private void OnClickMerchant(bool isOn)
    {
        if (isOn == false) return;
        power = 2;
        intelligence = 2;
        agility = 2;
        luck = 2;
        constitution = 2;
        classes = Classes.Merchant;
        UpdateUI();
    }

    private void OnClickConfirm()
    {
        Hide();
        var ms = MainSystem.Instance;
        playerData = new PlayerData(ifName.text, power, intelligence, agility, luck, constitution, classes, exceed);
        ms.PlayerDatas.Add(playerData);
        var playerPanel = ms.UIManager.GetPanel<PlayerPanel>();
        playerPanel.Name = ifName.text;
        playerPanel.UpdateHP(0);
        playerPanel.UpdateMP(0);
        var controlPanel = ms.UIManager.GetPanel<ControlPanel>();
        controlPanel.StartNewGame();
    }

    private void UpdateUI()
    {
        textPower.text = $"Power : {power}";
        textIntelligence.text = $"Intelligence : {intelligence}";
        textAgility.text = $"Agility : {agility}";
        textLuck.text = $"Luck : {luck}";
        textConstitution.text = $"Constitution : {constitution}";
    }

}
