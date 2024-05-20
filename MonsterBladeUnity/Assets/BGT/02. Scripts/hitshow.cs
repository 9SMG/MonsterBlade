using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitshow : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Arrow")
        {
            Debug.Log("Hit");
        }
    }
}
