using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    [SerializeField] private Image mExpBar;
    StatManager statManager;

    public float ExpCurrent { private set; get; }
    public float ExpMax { private set; get; } = 100;

    private Coroutine cUpdateExpBarFill;

    void Awake()
    {
        statManager = GetComponent<StatManager>();
    }
    public void AddExp(float value)
    {
        float expPrev = ExpCurrent;
        ExpCurrent += value;

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

            expPrev = Mathf.Lerp(expPrev, ExpCurrent, process);
            mExpBar.fillAmount = expPrev / ExpMax;

            if (expPrev / ExpMax > 1f)
            {
                expPrev = 0f;
                process = 0f;
                ExpCurrent -= ExpMax;
                ExpMax *= 2.0f;

                statManager.LevelUp();
            }

            yield return null;
        }
    }
}