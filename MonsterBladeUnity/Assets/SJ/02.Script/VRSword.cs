using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSword : MonoBehaviour
{
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
        }
        if (col.gameObject.tag == "Boss")
        {
            Debug.Log("BossHit");
        }
    }

}

