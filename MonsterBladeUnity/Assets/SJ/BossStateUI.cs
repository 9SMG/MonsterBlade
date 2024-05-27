using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStateUI : MonoBehaviour
{
    public float curBossHpUI;
    public float maxBossHpUI;

    public Image BossStateBar;
    public Text BossStateText;

    // Start is called before the first frame update
    void Start()
    {
        curBossHpUI = maxBossHpUI;
        BossStateBar = GameObject.FindGameObjectWithTag("BossUI").GetComponent<Image>();
        BossStateText = GameObject.FindGameObjectWithTag("BossTxtUI").GetComponent<Text>();
    }

    public void BossSetHp(float curBossHpUI, float maxBossHpUI)
    {
        this.curBossHpUI = curBossHpUI;
        this.maxBossHpUI = maxBossHpUI;
        BossUpdateHpUI();
    }
    public void BossTakeDamage(float damage)
    {
        curBossHpUI -= damage;
        curBossHpUI = Mathf.Clamp(curBossHpUI, 0, maxBossHpUI);
        BossUpdateHpUI();
    }
    private void BossUpdateHpUI()
    {
        Image BossStateBarImg = GameObject.FindGameObjectWithTag("BossUI").GetComponent<Image>();

        if (BossStateBarImg != null)
        {
            float fillAmount = curBossHpUI / maxBossHpUI;
            BossStateBarImg.fillAmount = fillAmount;

            BossStateText.text = Mathf.RoundToInt(fillAmount * 100.0f) + "%";
        }
    }
}
