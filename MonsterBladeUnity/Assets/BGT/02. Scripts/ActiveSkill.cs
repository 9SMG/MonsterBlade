using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    private IEnumerator Active_Stop;

    public bool skillCheck;
    public ActiveSkillManager Active;
    public PlayerCtrl player;
    public Image imgIcon;
    public Image imgCool;
    public Transform ArrowPos;
    public GameObject Skill;
    public TargetManager target;

    //public PlayerStateUI playerStateUI;
    void Awake()
    {
        //playerStateUI = GameObject.FindGameObjectWithTag("bbbb").GetComponent<PlayerStateUI>();
        //target = GameObject.FindWithTag("Player").GetComponent<TargetManager>();
    }

    void Start()
    {
        skillCheck = false;
    }

    void Update()
    {
        InputSkill();
        SkillActivated();
    }

    public void InputSkill()
    {
        if (Input.GetKey("r") && imgCool.fillAmount == 1)
        {
            GameUIManager.instance.playStateUI.UseSkill(Active.manaCost);
            //Debug.Log(playerStateUI.curMpUI);
            //Debug.Log("Skill");

            StopAllCoroutines();
            if (imgCool == null)
            {
                Debug.Log("imgCool = null");
            }
            else
            {
                Debug.Log("imgCool = notNull");
            }
            skillCheck = true;
        }
    }

    public void SkillActivated()
    {
        if (skillCheck == true)
        {
            skillCheck = false;
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

        if (target.myEnemyTarget != null && target.targetSet == true)
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
