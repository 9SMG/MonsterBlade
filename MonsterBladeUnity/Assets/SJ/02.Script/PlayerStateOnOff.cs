using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateOnOff : MonoBehaviour
{
    public GameObject Player;
    public GameObject VRplayer;

    void Awake()
    {
        Player = GameObject.Find("Player(Archer)");
        VRplayer = GameObject.Find("VRplayer");
    }

    void Start()
    {
        // 초기 상태 설정: VRplayer 활성화, Player 비활성화
        SetPlayerState(false);
    }

    void Update()
    {
        if (!Player.GetComponent<PlayerCtrl>().IsMine)
            return;
        // 키보드 입력을 통해 VRplayer와 Player를 토글합니다.
        if (Input.GetKeyDown(KeyCode.V))
        {
            // 현재 상태를 기준으로 토글
            SetPlayerState(!VRplayer.activeSelf);
        }
    }

    private void SetPlayerState(bool isPlay)
    {
        VRplayer.SetActive(isPlay);
        Player.SetActive(!isPlay);
    }
}
