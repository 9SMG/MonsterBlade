using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public MeshRenderer lifeBar;
    public PlayerCtrl _player;
    public Transform charaterBody;
    Animator anime;

    [field:SerializeField]
    public float _range { get; private set; }
    public float _rangespeed;
    public float _speed;
    public float _curHp;
    public float _maxHp;

    public Vector3 originPos;

    void Awake()
    {
        anime = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Start()
    {
        originPos = transform.position;
        _curHp = _maxHp;
        anime.SetBool("isAlive", true);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Arrow" && _curHp > 0)
        {
            Arrow arrow = col.GetComponent<Arrow>();
            _curHp -= arrow.damage;
            lifeBar.material.SetFloat("_Progress", _curHp / 100.0f);
            StartCoroutine(Damage());
        }

        if (col.gameObject.tag == "Skill" && _curHp > 0)
        {
            Debug.Log("SkillHit");
            Skill skill = col.GetComponent<Skill>();
            _curHp -= skill.damage;
            lifeBar.material.SetFloat("_Progress", _curHp / 100.0f);
            StartCoroutine(Damage());
        }
    }
    //public void Attack()
    //{
    //    _player.Damage();
    //}

    IEnumerator Damage()
    {
        anime.SetTrigger("isHit");
        yield return new WaitForSeconds(0.2f);

        if (_curHp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        anime.SetBool("isAlive", false);
        anime.SetBool("isDie", true);
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
