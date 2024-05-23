using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    [SerializeField] private Image mExpBar;
    StatManager statManager;

    public float _curExp;
    public float _curMaxExp = 100;

    private Coroutine cUpdateExpBarFill;

    void Awake()
    {
        statManager = GetComponent<StatManager>();
    }

    void Update()
    {
        
    }

    public void AddExp(float value)
    {
        float expPrev = _curExp;
        _curExp += value;

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

            expPrev = Mathf.Lerp(expPrev, _curExp, process);
            mExpBar.fillAmount = expPrev / _curMaxExp;

            if (expPrev / _curMaxExp >= 1f)
            {
                expPrev = 0f;
                process = 0f;
                _curExp -= _curMaxExp;
                _curMaxExp *= 2.0f;

                statManager.LevelUp();
            }

            yield return null;
        }
    }
}