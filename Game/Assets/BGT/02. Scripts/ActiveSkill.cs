using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    private IEnumerator Active_Stop;

    public ActiveSkillManager Active;
    public PlayerCtrl player;
    public Image imgIcon;
    public Image imgCool;
    public Transform ArrowPos;
    public GameObject Skill;
    TargetManager target;

    void Start()
    {
        imgIcon.sprite = Active.skill_Icon;
        Active_Stop = Active_Cool();
        target = GameObject.FindWithTag("Player").GetComponent<TargetManager>();
    }

    void Update()
    {
        SkillActivated();
    }

    public void SkillActivated()
    {
        if (Input.GetKey("r") && imgCool.fillAmount == 1)
        {
            StopAllCoroutines();
            //StopCoroutine(Active_Stop);
            Debug.Log("Skill");
            StartCoroutine(Skill_Start());
            StartCoroutine(Active_Cool());
        }
    }

    IEnumerator Skill_Start()
    {
        GameObject instantSkill = Instantiate(Skill, ArrowPos.position, ArrowPos.rotation);
        Rigidbody skillRigid = instantSkill.GetComponent<Rigidbody>();

        if (target.myEnemyTarget != null)
        {
            Vector3 direction = target.myEnemyTarget.transform.position - ArrowPos.position;
            direction.y += 1.5f;

            Quaternion rotation = Quaternion.LookRotation(direction);
            instantSkill.transform.rotation = rotation;

            skillRigid.velocity = direction.normalized * 50;
        }
        else
        {
            skillRigid.velocity = ArrowPos.forward * 50;
        }

        yield return null;
    }

    IEnumerator Active_Cool()
    {
        float tick = 1f / Active.CoolTime;
        float time = 0;

        imgCool.fillAmount = 0;

        while (imgCool.fillAmount <= 1)
        {
            imgCool.fillAmount = Mathf.Lerp(0, 1, time);
            time += (Time.deltaTime * tick);
            yield return null;
        }
    }
}
