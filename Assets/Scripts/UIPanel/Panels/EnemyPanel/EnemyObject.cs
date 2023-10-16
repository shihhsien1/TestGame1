using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour
{
    [SerializeField] private Image imgMainTexture = null;
    [SerializeField] private Image imgHp = null;
    private int hitPoint = 0;
    private int maxHitPoint = 0;
    private int flee = 0;
    private int atk = 0;

    public void Initialize(MonsterCardData card, Sprite mainTexture = null)
    {
        maxHitPoint = card.hitPoint;
        hitPoint = card.hitPoint;
        atk = card.attack;
        flee = card.flee;
        imgHp.fillAmount = 1;
        if(mainTexture != null)
            imgMainTexture.sprite = mainTexture;
    }
    public (bool, bool) UpdateHP(int val, int hitVal)
    {
        var isDead = false;
        if (hitVal < flee) return (isDead, false);
        hitPoint += val;
        var lerpVal = Mathf.Lerp(0, maxHitPoint, hitPoint);
        imgHp.fillAmount = lerpVal;
        if (lerpVal <= 0) isDead = true;
        return (isDead, true);
    }
}
