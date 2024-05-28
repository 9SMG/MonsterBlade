using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IResetTarget
{
    public ActiveSkillManager skill;
    public MeshRenderer lifeBar;
    public PlayerCtrl _player;
    public Transform charaterBody;
    DropCtrl dropCtrl;
    Animator anime;
    ExpManager expManager;
    StatManager statManager;

    [field: SerializeField]
    public float _searchRange { get; private set; }
    public float _maxMoveRange;
    public float _attackRange;
    public float _rangespeed;
    public float _speed;
    public float _curHp;
    public float _maxHp;
    public float _damage;
    public float _rotationSpeed;
    public float _moveSpeed;
    public bool playerHit;

    public Vector3 originPos;

    void Awake()
    {
        dropCtrl = GetComponentInChildren<DropCtrl>();
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
        if (_player == null)
        {
            Debug.Log("Null");
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
            expManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ExpManager>();
            statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();
        }
    }

    void FixedUpdate()
    {
        lifeBar.material.SetFloat("_Progress", _curHp / _maxHp);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Arrow" && _curHp > 0)
        {
            Arrow arrow = col.GetComponent<Arrow>();
            _curHp -= arrow.damage;
            StartCoroutine(Damage());
        }

        if (col.gameObject.tag == "Skill" && _curHp > 0)
        {
            Skill skill = col.GetComponent<Skill>();
            _curHp -= skill.damage;
            StartCoroutine(Damage());
        }
        if (col.gameObject.tag == "VRSword" && _curHp > 0)
        {
            VRSword vrsword = col.GetComponent<VRSword>();
            _curHp -= vrsword.damage;
            lifeBar.material.SetFloat("_Progress", _curHp / 100.0f);
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
        SoundManager.Instance.PlaySound2D("61_Hit_03", 0f, false, SoundType.EFFECT);
        anime.SetTrigger("isHit");

        if (_curHp <= 0)
        {
            StartCoroutine(Die());
        }

        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator Die()
    {
        anime.SetBool("isAlive", false);
        anime.SetBool("isDie", true);
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        dropCtrl.DropItem();
        expManager.AddExp(100);
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }

    public void ResetTarget()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        expManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ExpManager>();
        statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();
    }
}
