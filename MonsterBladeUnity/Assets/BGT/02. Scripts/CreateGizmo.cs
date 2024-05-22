using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGizmo : MonoBehaviour
{
    public Color Mycolor = Color.red;
    public float Myradius = 0.05f;

    void OnDrawGizmos()
    {

        Gizmos.color = Mycolor;
        Gizmos.DrawSphere(transform.position, Myradius);
    }
}
