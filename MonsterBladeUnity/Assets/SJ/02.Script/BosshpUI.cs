using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BosshpUI : MonoBehaviour
{
    public Image BossHp_Border;
    public GameObject player;
    public float activationDistance = 5f; // 활성화 거리

    // Start is called before the first frame update
    void Awake()
    {
        // 이미지 컴포넌트를 찾아 BossHp_Border에 할당
        BossHp_Border = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // ui 끄기
    void Start()
    {
        BossHp_Border.gameObject.SetActive(false);
    }

    // 다시활성
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("GetKeyDown(KeyCode.Z)");
            Debug.Log("BossHp_Border: " + BossHp_Border.gameObject.activeSelf);
            BossHp_Border.gameObject.SetActive(true);
            Debug.Log("BossHp_Border: " + BossHp_Border.gameObject.activeSelf);
        }
    }
}
