using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
    public RuntimeAnimatorController anim1;
    public RuntimeAnimatorController anim2;
    public ActiveSkillManager skill;
    public MeshRenderer lifeBar;
    public PlayerCtrl _player;
    public Transform charaterBody;
    public ParticleSystem particleGround;
    public ParticleSystem particleFly;
    public Transform firePosition;
    public CreateFireBall createFireBall;
    Animator anime;
    public TextMeshPro totalHp;
    public TextMeshPro headHp;
    public TextMeshPro legHp;
    public TextMeshPro wingHp;
    public Transform fireEnd;
    public GameObject _particleRange;
    public GameObject _rushRange;
    public GameObject _legDestroy;
    public GameObject randomFallPrefab;

    [field: SerializeField]
    public float _searchRange { get; private set; }
    public float _maxMoveRange;
    public float _attackRange;
    public float _rangespeed;
    public float _speed;
    public float _curHp;
    public float _maxHp;
    public float _headHp;
    public float _legHp;
    public float _wingHp;
    public float destroyDelay;
    public float spawnRadius = 10f;
    public float _rotationSpeed;
    public int numberOfRandomFalls = 10;
    public int checkNum = 0;
    public bool headDestroy;
    public bool legDestroy;
    public bool wingDestroy;
    public bool rushStart;
    public bool ground;

    public Vector3 originPos;

    void Awake()
    {
        rushStart = false;
        headDestroy = false;
        legDestroy = false;
        wingDestroy = false;
        ground = true;
        anime = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Start()
    {
        originPos = transform.position;
        _curHp = _maxHp;
        _headHp = _maxHp / 3;
        _legHp = _maxHp / 3;
        _wingHp = _maxHp / 3;
        anime.SetBool("isAlive", true);
    }

    void Update()
    {
        if (_player == null)
        {
            Debug.Log("Null");
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        }
        if (Input.GetKey(KeyCode.Keypad1))
        {
            _curHp = 170;
        }
    }

    void FixedUpdate()
    {
        totalHp.text = $"Total Hp : {Mathf.RoundToInt(_curHp)} / {_maxHp}";
        headHp.text = $"Head Hp : {Mathf.RoundToInt(_headHp)}";
        legHp.text = $"Leg Hp : {Mathf.RoundToInt(_legHp)}";
        wingHp.text = $"Wing Hp : {Mathf.RoundToInt(_wingHp)}";
        lifeBar.material.SetFloat("_Progress", _curHp / _maxHp);
        if (_curHp / _maxHp <= 0.3)
        {
            if (ground)
            {
                SoundManager.Instance.PlaySound2D("Monster screams - 57", 0f, false, SoundType.VOICE);
            }
            ground = false;
            anime.runtimeAnimatorController = anim2;
        }
        if (_curHp / _maxHp > 0.3)
        {
            ground = true;
            anime.runtimeAnimatorController = anim1;
        }
    }

    public void ActivateParticle()
    {
        StartCoroutine(StartDelay());
    }

    public void ActivateRush()
    {
        StartCoroutine(StartRush());
    }

    public void ActivateRandomFalls()
    {
        for (int i = 0; i < numberOfRandomFalls; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject randomFallInstance = Instantiate(randomFallPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;
        return randomPosition;
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
    }

    //public void Attack()
    //{
    //    _player.Damage();
    //}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player" && rushStart)
        {
            _player.Damage(20f);
            Debug.Log("rushHit!!!");
        }
    }

    IEnumerator StartRush()
    {
        _rushRange = transform.Find("Rush").transform.gameObject;
        rushStart = true;
        _rushRange.SetActive(true);
        SoundManager.Instance.PlaySound2D("Monster screams - 56", 0f, false, SoundType.VOICE);
        yield return new WaitForSeconds(4.4f);
        rushStart = false;
        _rushRange.SetActive(false);
    }

    IEnumerator StartDelay()
    {
        if (ground)
        {
            _particleRange = transform.Find("Breath(Ground)").transform.gameObject;
            _particleRange.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            Vector3 particlePosition = firePosition.position;
            Vector3 particleDirection = (fireEnd.position - particlePosition).normalized;
            Quaternion rotationEnd = Quaternion.LookRotation(particleDirection);
            ParticleSystem newParticle = Instantiate(particleGround, particlePosition, rotationEnd, firePosition);
            newParticle.Play();
            SoundManager.Instance.PlaySound2D("Dragon+Spit+Fire+1_out",  0f, false, SoundType.EFFECT);
            Destroy(newParticle.gameObject, destroyDelay);
            yield return new WaitForSeconds(destroyDelay);
            _particleRange.SetActive(false);
        }
        else if (!ground)
        {
            _particleRange = transform.Find("Breath(Fly)").transform.gameObject;
            _particleRange.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            Vector3 particlePosition = firePosition.position;
            Vector3 particleDirection = (fireEnd.position - particlePosition).normalized;
            Quaternion rotationEnd = Quaternion.LookRotation(particleDirection);
            ParticleSystem newParticle = Instantiate(particleFly, particlePosition, rotationEnd, firePosition);
            newParticle.Play();
            SoundManager.Instance.PlaySound2D("Dragon+Spit+Fire+1_out",  0f, false, SoundType.EFFECT);
            Destroy(newParticle.gameObject, destroyDelay);
            yield return new WaitForSeconds(destroyDelay);
            _particleRange.SetActive(false);
        }
    }

    public void OnParticleTrigger()
    {
        Debug.Log("PlayerHit!");
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound3D("61_Hit_03", this.transform, 0f, false, SoundType.EFFECT);
        if (_curHp <= 0)
        {
            StartCoroutine(Die());
        }
        if (_headHp <= 0 && !headDestroy)
        {
            headDestroy = true;
            anime.SetTrigger("isHit");
            Debug.Log("HeadDestroy!!!");
        }
        if (_legHp <= 0 && !legDestroy)
        {
            _legDestroy = transform.Find("LegDestroy").transform.gameObject;
            legDestroy = true;
            _legDestroy.SetActive(true);
            anime.SetTrigger("isHit");
            Debug.Log("LegDestroy!!!");
        }
        if (_wingHp <= 0 && !wingDestroy)
        {
            wingDestroy = true;
            anime.SetTrigger("isHit");
            Debug.Log("WingDestroy!!!");
        }
    }

    IEnumerator Die()
    {
        anime.SetBool("isAlive", false);
        anime.SetBool("isDie", true);
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
