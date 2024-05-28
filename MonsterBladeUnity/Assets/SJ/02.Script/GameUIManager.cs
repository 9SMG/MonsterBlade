using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance = null; // �̱���

    // [�÷��̾� �������ͽ�]
    public PlayerStateUI playStateUI;
    // [���� �������ͽ�]
    public BossStateUI bossStateUI; 
    // [��Ƽ �������ͽ�]
    public PartyStateUI partyStateUI;

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
    }
    // Start is called before the first frame update
    void Start()
    { 

        bossStateUI.BossSetHp(bossStateUI.curBossHpUI, bossStateUI.maxBossHpUI);

        partyStateUI.PartySetHp(partyStateUI.mCurHpUI, partyStateUI.mMaxHpUI);
    }

    // Update is called once per frame
    void Update()
    {
        //// ������ ü�°��� 

        //if (Input.GetKeyDown(KeyCode.F9))
        //{
        //    playStateUI.TakeDamage(10);
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    playStateUI.UseSkill(20f); // ���÷� ���� 20�� �Ҹ��ϴ� ��ų
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    bossStateUI.BossTakeDamage(15f); // ���÷� ���� 20�� �Ҹ��ϴ� ��ų
        //}
        if(Input.GetKeyDown(KeyCode.F2))
        {
            playStateUI.GainExperience(10f);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            playStateUI.TakeStamina(20f);
        }
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    partyStateUI.PartyTakeDamage(20f); // ���÷� ���� 20�� �Ҹ��ϴ� ��ų
        //}
    }
}
