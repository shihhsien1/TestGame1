using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus
{
    public int HitPoint { get; protected set; } = 0;
    public int MaxHitPoint { get; protected set; } = 0;
    public int ManaPoint { get; protected set; } = 0;
    public int MaxManaPoint { get; protected set; } = 0;
    public int Power { get; protected set; } = 0;
    public int Intelligence { get; protected set; } = 0;
    public int Agility { get; protected set; } = 0;
    public int Luck { get; protected set; } = 0;
    public int Constitution { get; protected set; } = 0;
    public int PhysicAtk { get; protected set; } = 0;
    public int MagicAtk { get; protected set; } = 0;
    public int Flee { get; protected set; } = 0;

}
