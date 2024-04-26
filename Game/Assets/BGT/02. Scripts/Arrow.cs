using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float deadtime;

    void Start()
    {
        Destroy(this.gameObject, deadtime);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            Debug.Log("Hit");
        }
    }
}
