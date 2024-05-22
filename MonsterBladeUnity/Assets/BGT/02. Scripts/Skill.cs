using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    ActiveSkill skill;
    public float damage;
    public float deadtime;

    void Start()
    {
        Destroy(this.gameObject, deadtime);
        skill = GameObject.FindWithTag("Player").GetComponent<ActiveSkill>();
        damage = skill.Active.damage;
    }
}
