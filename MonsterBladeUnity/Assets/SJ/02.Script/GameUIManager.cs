using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance = null; // 싱글톤
    // [플레이어 스테이터스]
    [SerializeField] private PlayerHpUI playerHpUI;          // 체력
    [SerializeField] private Text HpPercent; // 체력 텍스트
    [SerializeField] public PlayerMpUI  playerMpUI;          // 마나
    [SerializeField] private PlayerSpUI  playerSpUI;          // 스태미너
    [SerializeField] private PlayerExpUI playerExpUI;         // 경험치

    // 플레이어 스킬
    [SerializeField] private PlayerSkillUI playerSkiUI; // 플레이어 스킬 1번

    // 파티 스킬
    [SerializeField] private PartyHpUI_1 partyHpUI_1; // 파티 체력1번
    //[SerializeField] private PartyHpUI_2 partyHpUI_2; // 파티 체력2번

    // 보스 UI
    [SerializeField] private BossUI bossUI;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        #region 플레이어 UI
        playerHpUI = GameObject.Find("Hp").GetComponent<PlayerHpUI>();
        HpPercent = GameObject.Find("HpPercent").GetComponent<Text>();
        playerMpUI  = GameObject.Find("Mp").GetComponent<PlayerMpUI>();
        playerSpUI  = GameObject.Find("SpImg").GetComponent<PlayerSpUI>();
        playerExpUI = GameObject.Find("Prograss").GetComponent<PlayerExpUI>();
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
        GameUIManager.instance.playerHpUI.PlayerHpBar();
        GameUIManager.instance.playerMpUI.PlayerMpBar();

        GameUIManager.instance.playerSpUI.PlayerSpBar();

        GameUIManager.instance.playerExpUI.PlayerExpBar();

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
}
