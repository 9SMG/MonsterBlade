using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ActiveSkillManager skill;
    public MeshRenderer lifeBar;
    public PlayerCtrl _player;
    public Transform charaterBody;
    Animator anime;

    [field:SerializeField]
    public float _searchRange { get; private set; }
    public float _maxMoveRange;
    public float _attackRange;
    public float _rangespeed;
    public float _speed;
    public float _curHp;
    public float _maxHp;
    public float _damage;
    public float _rotationSpeed;
    public bool playerHit;

    public Vector3 originPos;

    void Awake()
    {
        anime = GetComponent<Animator>();
        originPos = transform.position;
        _curHp = _maxHp;
    }

    void Start()
    {
        anime.SetBool("isAlive", true);
    }

    void Update()
    {
        if(_player == null)
        {
            Debug.Log("Null");
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Arrow" && _curHp > 0)
        {
            Arrow arrow = col.GetComponent<Arrow>();
            _curHp -= arrow.damage;
            lifeBar.material.SetFloat("_Progress", _curHp / _maxHp);
            StartCoroutine(Damage());
        }

        if (col.gameObject.tag == "Skill" && _curHp > 0)
        {
            Skill skill = col.GetComponent<Skill>();
            _curHp -= skill.damage;
            lifeBar.material.SetFloat("_Progress", _curHp / _maxHp);
            StartCoroutine(Damage());
        }
    }

    public void PlayerHitCheck()
    {
        StartCoroutine(PlayerHit());
    }

    IEnumerator PlayerHit()
    {
        yield return new WaitForSeconds(0.5f);
        playerHit = false;
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
