using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/ActiveSkill")]
public class ActiveSkillManager : ScriptableObject
{
    public enum Element { normal, fire, water, dark, light }
    public enum StatusEffect { normal, burn, stun, slow, anger, fear, bleed, laceration, sleep }
    public Element element;
    public StatusEffect statusEffect;
    public float damage;
    public float range;
    public int manaCost;
    public int castTime;
    public Sprite skill_Icon;

}
