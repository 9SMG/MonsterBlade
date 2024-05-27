using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    StatManager statManager;
    ActiveSkill skill;
    public float damage;
    public float deadtime;

    void Awake()
    {
        statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();
    }

    void Start()
    {
        Destroy(this.gameObject, deadtime);
        skill = GameObject.FindWithTag("Player").GetComponent<ActiveSkill>();
        damage = skill.Active.damage + statManager.statInfo.Attack;
    }
}
