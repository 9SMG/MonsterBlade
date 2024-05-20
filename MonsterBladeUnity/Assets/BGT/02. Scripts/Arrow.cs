using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float deadtime;
    Boss bossHP;
  
    void Start()
    {
        bossHP = GameObject.FindWithTag("Boss").GetComponent<Boss>();
        Destroy(this.gameObject, deadtime);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(col.gameObject.tag == "BossHeadColl")
            {
                Debug.Log("Head !!!");
                bossHP._headHp -= damage;
            }
            if (col.gameObject.tag == "BossLegColl")
            {
                Debug.Log("Leg !!!");
                bossHP._legHp -= damage;
            }
            if (col.gameObject.tag == "BossWingColl")
            {
                Debug.Log("Wing !!!");
                bossHP._wingHp -= damage;
            }
            Destroy(this.gameObject);
        }
    }
}
