using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : BasePanel
{
    [SerializeField] EnemyObject enemy = null;
    protected override void Start()
    {
        base.Start();
        Hide();
    }

    public void Initialize(MonsterCardData card)
    {
        enemy.Initialize(card);
    }

    public (bool, bool) UpdateHP(int val, int hitVal)
    {
        return enemy.UpdateHP(val, hitVal);
    }

}
