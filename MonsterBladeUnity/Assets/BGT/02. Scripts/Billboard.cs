using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform myTr;
    Transform mainCameraTr;
    // Start is called before the first frame update
    void Start()
    {
        myTr = GetComponent<Transform>();
        mainCameraTr = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTr.LookAt(mainCameraTr);
    }
}
