using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateOnOff : MonoBehaviour
{
    public GameObject Player;
    public GameObject VRPlayer;

    void Awake()
    {
        Player = GameObject.Find ("Player").GetComponent<GameObject>();
        VRPlayer = GameObject.Find("VRPlayer").GetComponent<GameObject>();

    }
    // Start is called before the first frame update
    void Start()
    {
        VRPlayer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
