using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossUI_OnOff : MonoBehaviour, IResetTarget
{
    public GameObject Boss_UI;
    public GameObject player;
    public GameObject Point;
    public float activationDistance = 200.0f;


    private void Awake()
    {
        Boss_UI = GameObject.FindGameObjectWithTag("Boss_UI");
        player = GameObject.FindGameObjectWithTag("Player");
        Point = GameObject.FindGameObjectWithTag("Point");
    }
    // Start is called before the first frame update
    void Start()
    {
        Boss_UI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && Point != null)
        {
            float distance = Vector3.Distance(player.transform.position, Point.transform.position);
            if (distance <= activationDistance)
            {
                Boss_UI.SetActive(true);
            }
            else
            {
                Boss_UI.SetActive(false);
            }
        }
    }
    public void ResetTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
