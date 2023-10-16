using System.Collections.Generic;
using UnityEngine;

public class PlayerData : BaseStatus
{
    public PlayerData(string name = "", int power = 0, int intelligence = 0, int agility = 0, int luck = 0, int constitution = 0, Classes classes = Classes.None, Exceed exceed = Exceed.None)
    {
        Name = name;
        AddPower(power);
        AddIntelligence(intelligence);
        AddAgility(agility);
        AddLuck(luck);
        AddConstitution(constitution);
        Classes = classes;
        Exceed = exceed;
    }
    public int Level { get; set; } = 1;
    public int MaxLevel { get; private set; } = 10;
    public string Name { get; set; } = string.Empty;
    public int Exp { get; private set; } = 0;
    public Exceed Exceed { get; private set; } = Exceed.None;
    public Classes Classes { get; private set; } = Classes.None;
    public List<int> ItemIdList { get; private set; } = new List<int>();
    public List<int> RareItemIdList { get; private set; } = new List<int>();
    public int StatusPoint { get; set; } = 0;
    public int Coin { get; set; } = 0;
    public int EquipWeaponId { get; set; } = 0;
    public int EquipArmorId { get; set; } = 0;
    public void AddHP(int add)
    {
        if(HitPoint + add > MaxHitPoint)
        {
            HitPoint = MaxHitPoint;
        }
        else if(HitPoint + add <= 0)
        {
            HitPoint = 0;
            Events.OnPlayDead.Fire();
        }
        else
        {
            HitPoint += add;
        }
    }
    public void AddMaxHP(int add)
    {
        if(Constitution > 0) MaxHitPoint = Constitution * 8;
        if (MaxHitPoint + add <= 0) MaxHitPoint = 1;
        else MaxHitPoint += add;
    }
    public void AddMP(int add)
    {
        if (ManaPoint + add > MaxManaPoint)
        {
            ManaPoint = MaxManaPoint;
        }
        else if (ManaPoint + add <= 0)
        {
            ManaPoint = 0;
        }
        else
        {
            ManaPoint += add;
        }
    }
    public void AddMaxMP(int add)
    {
        if(Intelligence > 0) MaxManaPoint = Intelligence * 4;
        if (MaxManaPoint + add <= 0) MaxManaPoint = 0;
        else MaxManaPoint += add;
    }
    public void AddExp(int add)
    {
        Exp += add;
        if(Exp >= 100 && Level < MaxLevel)
        {
            Level += 1;
            StatusPoint += 1;
            Exp = 0;
        }
    }
    public void AddPower(int add)
    {
        Power += add;
        UpdatePhysicAtk();
    }
    public void AddIntelligence(int add)
    {
        Intelligence += add;
        AddMaxMP(0);
        AddMP(Intelligence * 4);
        UpdateMagicAtk();
    }
    public void AddAgility(int add)
    {
        Agility += add;
        if(Agility > 0) Flee = Agility * 3;
    }
    public void AddLuck(int add)
    {
        Luck += add;
        UpdatePhysicAtk();
        UpdateMagicAtk();
    }
    public void AddConstitution(int add)
    {
        Constitution += add;
        AddMaxHP(0);
        AddHP(Constitution * 8);
    }
    private void UpdatePhysicAtk()
    {
        PhysicAtk = 1;
        if(Power > 0) PhysicAtk += Power * 2;
        if(Luck > 0) PhysicAtk += Mathf.FloorToInt(Luck * 0.5f);
    }
    private void UpdateMagicAtk()
    {
        MagicAtk = 1;
        if (Intelligence > 0) MagicAtk += Intelligence * 2;
        if (Luck > 0) MagicAtk += Mathf.FloorToInt(Luck * 0.5f);
    }
    public bool BattleHitCondition(int damage, int hitVal)
    {
        if (hitVal < Flee) return false;
        AddHP(damage);
        return true;
    }

}