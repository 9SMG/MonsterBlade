using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    //[SerializeField] private Image mExpBar;
    StatManager statManager;

    //public float _curExp;
    //public float _curMaxExp;

    private Coroutine cUpdateExpBarFill;

    void Awake()
    {
        //_curExp = statManager.statInfo._curEXP;
        //_curMaxExp = statManager.statInfo._curMaxEXP;
        statManager = GetComponent<StatManager>();
    }

    void Update()
    {
        
    }

    public void AddExp(float value)
    {
        float expPrev = statManager.statInfo._curEXP;
        statManager.statInfo._curEXP += value;
        GameUIManager.instance.playStateUI.GainExperience(value);


        if (cUpdateExpBarFill != null)
            StopCoroutine(cUpdateExpBarFill);
        cUpdateExpBarFill = StartCoroutine(UpdateExpBarFill(expPrev));
    }

    private IEnumerator UpdateExpBarFill(float expPrev)
    {
        float process = 0f;

        while (process < 1f)
        {
            process += Time.deltaTime;

            expPrev = Mathf.Lerp(expPrev, statManager.statInfo._curEXP, process);
            //mExpBar.fillAmount = expPrev / statManager.statInfo._curMaxEXP;

            if (expPrev / statManager.statInfo._curMaxEXP >= 1f)
            {
                expPrev = 0f;
                process = 0f;
                statManager.statInfo._curEXP -= statManager.statInfo._curMaxEXP;
                statManager.statInfo._curMaxEXP *= 2.0f;

                statManager.LevelUp();
            }

            yield return null;
        }
    }
}