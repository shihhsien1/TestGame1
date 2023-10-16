using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : BasePanel
{
    [SerializeField] private Text textLevelExp = null;
    [SerializeField] private Text textPoint = null;
    [SerializeField] private Text textPower = null;
    [SerializeField] private Text textIntelligence = null;
    [SerializeField] private Text textAgility = null;
    [SerializeField] private Text textLuck = null;
    [SerializeField] private Text textConstitution = null;
    [SerializeField] private Button btnHide = null;
    [SerializeField] private Button btnConfirm = null;
    [SerializeField] private Button btnAddPower = null;
    [SerializeField] private Button btnAddInteligence = null;
    [SerializeField] private Button btnAddAgility = null;
    [SerializeField] private Button btnAddLuck = null;
    [SerializeField] private Button btnAddConstitution = null;
    [SerializeField] private Button btnReducePower = null;
    [SerializeField] private Button btnReduceInteligence = null;
    [SerializeField] private Button btnReduceAgility = null;
    [SerializeField] private Button btnReduceLuck = null;
    [SerializeField] private Button btnReduceConstitution = null;
    private int addPower = 0;
    private int addIntelligence = 0;
    private int addAgility = 0;
    private int addLuck = 0;
    private int addConstitution = 0;
    private int point = 0;
    private PlayerData playerData = null;
    protected override void Start()
    {
        base.Start();
        Hide();
        btnHide.onClick.AddListener(Hide);
        btnConfirm.onClick.AddListener(OnClickConfirm);
        btnAddPower.onClick.AddListener(OnClickAddPower);
        btnAddInteligence.onClick.AddListener(OnClickAddIntelligence);
        btnAddAgility.onClick.AddListener(OnClickAddAgility);
        btnAddLuck.onClick.AddListener(OnClickAddLuck);
        btnAddConstitution.onClick.AddListener(OnClickAddConstitution);
        btnReducePower.onClick.AddListener(OnClickReducePower);
        btnReduceInteligence.onClick.AddListener(OnClickReduceIntelligence);
        btnReduceAgility.onClick.AddListener(OnClickReduceAgility);
        btnReduceLuck.onClick.AddListener(OnClickReduceLuck);
        btnReduceConstitution.onClick.AddListener(OnClickReduceConstitution);
    }
    public void ShowPanel()
    {
        Show();
        playerData = MainSystem.Instance.PlayerDatas[0];
        point = playerData.StatusPoint;
        textLevelExp.text = $"Lv : {playerData.Level}   Exp : {playerData.Exp}";
        textPower.text = $"Power : {playerData.Power}";
        textIntelligence.text = $"Inteligence : {playerData.Intelligence}";
        textAgility.text = $"Agility : {playerData.Agility}";
        textLuck.text = $"Luck : {playerData.Luck}";
        textConstitution.text = $"Constitution : {playerData.Constitution}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickAddPower()
    {
        if (point <= 0) return;
        addPower++;
        point--;
        textPower.text = $"Power : {playerData.Power + addPower}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickAddIntelligence()
    {
        if (point <= 0) return;
        addIntelligence++;
        point--;
        textIntelligence.text = $"Inteligence : {playerData.Intelligence + addIntelligence}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickAddAgility()
    {
        if (point <= 0) return;
        addAgility++;
        point--;
        textAgility.text = $"Agility : {playerData.Agility + addAgility}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickAddLuck()
    {
        if (point <= 0) return;
        addLuck++;
        point--;
        textLuck.text = $"Luck : {playerData.Luck + addLuck}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickAddConstitution()
    {
        if (point <= 0) return;
        addConstitution++;
        point--;
        textConstitution.text = $"Constitution : {playerData.Constitution + addConstitution}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickReducePower()
    {
        if (point >= playerData.StatusPoint) return;
        addPower--;
        point++;
        textPower.text = $"Power : {playerData.Power + addPower}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickReduceIntelligence()
    {
        if (point >= playerData.StatusPoint) return;
        addIntelligence--;
        point++;
        textIntelligence.text = $"Intelligence : {playerData.Intelligence + addIntelligence}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickReduceAgility()
    {
        if (point >= playerData.StatusPoint) return;
        addAgility--;
        point++;
        textAgility.text = $"Agility : {playerData.Agility + addAgility}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickReduceLuck()
    {
        if (point >= playerData.StatusPoint) return;
        addLuck--;
        point++;
        textLuck.text = $"Luck : {playerData.Luck + addLuck}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickReduceConstitution()
    {
        if (point >= playerData.StatusPoint) return;
        addConstitution--;
        point++;
        textConstitution.text = $"Constitution : {playerData.Constitution + addConstitution}";
        textPoint.text = $"Point : {point}";
    }
    private void OnClickConfirm()
    {
        playerData.AddPower(addPower);
        playerData.AddIntelligence(addIntelligence);
        playerData.AddAgility(addAgility);
        playerData.AddLuck(addLuck);
        playerData.AddConstitution(addConstitution);
        playerData.StatusPoint -= playerData.StatusPoint - point;
    }

}
