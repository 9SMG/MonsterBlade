using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatInfo", menuName = "Stats/StatInfo")]
public class StatInfo : ScriptableObject
{
    public int level = 1;
    public float hpMax;
    public float mpMax;
    public float staminaMax;
    public float Attack;
    public float MovementSpeed;
    public float Defense;

    public float _curHP;
    public float _curMP;
    public float _curStamina;
    public float _curEXP;
    public float _curMaxEXP;


    public void InitStatData()
    {
        _curHP = hpMax;
        _curMP = mpMax;
        _curStamina = staminaMax;
    }

    public void UpgradeBaseStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.LEVEL:
                level++;
                break;
            case StatType.HP:
                hpMax += 50;
                _curHP += 50;
                break;
            case StatType.MP:
                mpMax += 50;
                _curMP += 50;
                break;
            case StatType.STAMINA:
                staminaMax += 50;
                _curStamina += 50;
                break;
            case StatType.ATTACK:
                Attack += 5;
                break;
            case StatType.MOVEMENT_SPEED:
                MovementSpeed += 1;
                break;
            case StatType.DEFENSE:
                Defense += 2.5f;
                break;
        }
    }
}
