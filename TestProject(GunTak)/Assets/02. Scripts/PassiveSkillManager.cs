using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Skill/PassiveSkill")]
public class PassiveSkillManager : ScriptableObject
{
    public enum Stat { HP, MP, SP, HpRegen, MpRegen, SpRegen, Atk, Def, CriChance, CriDamage, MoveSpeed, AtkSpeed, Range, ExpGain, SuperArmor }
    public Stat stat;
    public float FixedStat;
    public float PercentStat;
    public Sprite skill_Icon;
}
