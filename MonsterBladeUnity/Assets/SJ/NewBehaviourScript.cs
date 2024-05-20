using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{
    private Transform cube;
    private RectTransform img;
    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.Find("CubeParent").transform.Find("Cube");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            cube.gameObject.SetActive(false);
        }
        if(Input.GetMouseButtonDown(1))
        {
            GameObject.Find("CubeParent").transform.Find("Cube").gameObject.SetActive(true);
        }
    }
}
