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






























    //GameObject _target;
    //Vector3 _direction;
    //float _timer = 0.0f;

    //void Start()
    //{
    //    SetState(new SearchState(this));
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (_curState != null)
    //    {
    //        _curState.OnUpdate();
    //    }
    //}

    //State _curState;
    //void SetState(State state)
    //{
    //    if (_curState != null)
    //    {
    //        _curState.OnExit();
    //    }

    //    _curState = state;

    //    if (_curState != null)
    //    {
    //        _curState.OnEnter();
    //    }
    //}
    //abstract public class State
    //{
    //    protected Enemy _enemy;
    //    public State(Enemy enemy)
    //    {
    //        _enemy = enemy;
    //    }
    //    abstract public void OnEnter();
    //    abstract public void OnUpdate();
    //    abstract public void OnExit();
    //}
    //// SearchState =============================================================================================================================
    //public class SearchState : State
    //{
    //    float _timer = 0.0f;
    //    public SearchState(Enemy enemy) : base(enemy)
    //    {

    //    }

    //    override public void OnEnter()
    //    {
    //        _timer = 0.0f;
    //    }
    //    override public void OnUpdate()
    //    {
    //        _timer += Time.deltaTime;
    //        if (_timer > 1.0f)
    //        {
    //            _enemy.SetState(new SearchAndMoveState(_enemy));
    //            return;
    //        }

    //        float distance = Vector2.Distance(_enemy.transform.position, _enemy._player.transform.position);

    //        if (distance < _enemy._range)
    //        {
    //            _enemy.SetState(new MoveState(_enemy));
    //        }
    //    }
    //    override public void OnExit()
    //    {

    //    }
    //}
    //// MoveState ===============================================================================================================================
    //public class MoveState : State
    //{
    //    public MoveState(Enemy enemy) : base(enemy)
    //    {

    //    }

    //    override public void OnEnter()
    //    {

    //    }
    //    override public void OnUpdate()
    //    {
    //        float distance = Vector2.Distance(_enemy.transform.position, _enemy._player.transform.position);

    //        if (distance > _enemy._range)
    //        {
    //            _enemy.SetState(new SearchState(_enemy));
    //            return;
    //        }

    //        Vector3 delta = _enemy._player.transform.position - _enemy.transform.position;
    //        _enemy.transform.position += delta.normalized * _enemy._speed * Time.deltaTime;
    //    }
    //    override public void OnExit()
    //    {

    //    }
    //}
    //// SearchAndMoveState ======================================================================================================================
    //public class SearchAndMoveState : State
    //{
    //    float _timer = 0.0f;
    //    Vector3 _direction;

    //    public SearchAndMoveState(Enemy enemy) : base(enemy)
    //    {

    //    }

    //    override public void OnEnter()
    //    {
    //        _timer = 0.0f;
    //        _direction = Random.insideUnitCircle.normalized;
    //    }
    //    override public void OnUpdate()
    //    {
    //        _timer += Time.deltaTime;
    //        if (_timer > 1.0f)
    //        {
    //            _enemy.SetState(new SearchState(_enemy));
    //            return;
    //        }

    //        float distance = Vector2.Distance(_enemy.transform.position, _enemy._player.transform.position);

    //        if (distance < _enemy._range)
    //        {
    //            _enemy.SetState(new MoveState(_enemy));
    //        }

    //        _enemy.transform.position += _direction * _enemy._speed * Time.deltaTime;
    //    }
    //    override public void OnExit()
    //    {

    //    }
    //}
}
