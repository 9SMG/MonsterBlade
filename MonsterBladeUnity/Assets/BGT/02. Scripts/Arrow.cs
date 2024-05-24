using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float deadtime;
    Boss bossHP;
    StatManager statManager;

	private void Awake()
	{
        statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();
    }

	void Start()
    {
        bossHP = GameObject.FindWithTag("Boss").GetComponent<Boss>();
        Destroy(this.gameObject, deadtime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (col.gameObject.tag == "BossHeadColl")
            {
                Debug.Log("Head !!!");
                bossHP._headHp -= statManager.statInfo.Attack; ;
            }
            if (col.gameObject.tag == "BossLegColl")
            {
                Debug.Log("Leg !!!");
                bossHP._legHp -= statManager.statInfo.Attack; ;
            }
            if (col.gameObject.tag == "BossWingColl")
            {
                Debug.Log("Wing !!!");
                bossHP._wingHp -= statManager.statInfo.Attack; ;
            }
            Destroy(this.gameObject);
        }
    }
}
