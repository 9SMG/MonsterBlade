using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance = null; // 싱글톤
    // [플레이어 스테이터스]
    public PlayerStateUI playStateUI;          // 플레이어 스테이트

    // 플레이어 스킬
    [SerializeField] private PlayerSkillUI playerSkiUI; // 플레이어 스킬 1번

    // 파티 스킬
    [SerializeField] private PartyHpUI_1 partyHpUI_1; // 파티 체력1번
    //[SerializeField] private PartyHpUI_2 partyHpUI_2; // 파티 체력2번

    // 보스 UI
    [SerializeField] private BossUI bossUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        playStateUI = GameObject.Find("Player").GetComponent<PlayerStateUI>();
        #region 플레이어 UI
        playerSkiUI = GameObject.Find("BuffDelay").GetComponent<PlayerSkillUI>();
        #endregion

        #region 파티 UI
        partyHpUI_1 = GameObject.Find("pHp").GetComponent<PartyHpUI_1>();
        //partyHpUI_2 = GameObject.Find("pHp1").GetComponent<PartyHpUI_2>();
        #endregion

        #region 보스 UI
        bossUI = GameObject.Find("BossHpUI").GetComponent<BossUI>();
        #endregion
    }
    // Start is called before the first frame update
    void Start()
    {
        #region 플레이어 UI Start
        //GameUIManager.instance.playStateUI.PlayerHpBar();
        GameUIManager.instance.playerSkiUI.PlayerSkiUIBar();
        #endregion

        #region 파티 UI Start
        GameUIManager.instance.partyHpUI_1.PartyHpBar();
        //UIManager.instance.partyHpUI_2.PartyHpBar();
        #endregion

        #region 보스 UI Start
        GameUIManager.instance.bossUI.BossUIHpBar();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerHpUI(float curHpUI, float maxHpUI)
    {
        playStateUI.curHpUI = curHpUI;
        playStateUI.maxHpUI = maxHpUI;
    }
    public void PlayerMpUI(float curMpUI, float maxMpUI)
    {
        playStateUI.curMpUI = curMpUI;
        playStateUI.maxMpUI = maxMpUI;
    }
    public void PlayerSpUI(float curSpUI, float maxSpUI)
    {
        playStateUI.curSpUI = curSpUI;
        playStateUI.maxSpUI = maxSpUI;
    }
    public void PlayerExpUI(float curExpUI, float maxExpUI)
    {
        playStateUI.curExpUI = curExpUI;
        playStateUI.maxExpUI = maxExpUI;
    }
}
