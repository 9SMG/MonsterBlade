using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterBlade.MyPhoton;

[RequireComponent(typeof(CharacterController))]

public class PlayerCtrl : PunCharactor //MonoBehaviour
{
    [SerializeField] public Transform charaterBody;
    Vector3 moveDirection;
    Vector3 MoveDir;
    Vector3 diveDirection;
    Animator animator;
    public Camera camera;
    CharacterController controller;
    Ray ray;
    RaycastHit hitInfo;
    Weapon equipWeapon;
    SpriteRenderer _spriteRenderer;
    TargetManager target;
    ParticleDamage particle;

    public GameObject SpawnPos;
    public StatInfo statInfo;
    public GameObject[] pickUpItem;
    public bool[] checkPickUp = new bool[3];
    public float speed = 2f;
    public float runSpeed = 8f;
    public float finalSpeed;
    public float rotationSpeed;
    public float smoothness = 10f;
    public float diveSpeed;
    public float gravity;
    public float MAXHP;
    public float CURRHP;
    public int LV;
    public bool togglecameraRotation;
    public bool run;
    public bool diveRoll;
    public bool shotReady;
    public bool attackHit;
    public bool enablePickUp;
    public bool dieCheck;
    public bool open;
    public bool shotD;
    public bool isTarget;
    public GameObject[] weapons;
    public GameObject crossHair;
    public GameObject Inven;
    public GameObject Look;

    float fireDelay;
    bool fireStart;
    bool groundCheck;
    public bool isMove;
    int weaponsIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        SetPhotonViewDataLen(3);

        particle = GameObject.FindWithTag("EnemySkill").GetComponent<ParticleDamage>();
        animator = GetComponent<Animator>();
        animatorPun = animator; // Photon AnimatorRPC
        //camera = Camera.main;
        controller = GetComponent<CharacterController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        target = GetComponent<TargetManager>();
        dieCheck = false;
        open = false;
        shotD = false;
        isTarget = false;
        isMove = false;
        SetBoolRPC("isAlive", true); //animator.SetBool("isAlive", true);

        if (SpawnPos == null)
            SpawnPos = GameObject.Find("PlayerSpwanPos");
    }

    void Start()
    {
        if (!IsMine)
        {
            Camera[] _cams = GetComponentsInChildren<Camera>();
            foreach (Camera cam in _cams)
            {
                cam.gameObject.SetActive(false);
            }
            GetComponentInChildren<CameraCtrl>().gameObject.SetActive(false);

            return;
        }

        //Camera[] cams = transform.root.GetComponentsInChildren<Camera>();
        //foreach (Camera cam in cams)
        //{
        //    if (cam.CompareTag("MainCamera"))
        //    {
        //        camera = cam;
        //        break;
        //    }
        //}

        diveSpeed = 10.0f;
        gravity = 5.0f;
        MoveDir = Vector3.zero;
        attackHit = false;
    }

    void Update()
    {
        if (!IsMine)
        {
            UpdateSync();
            return;
        }

        CameraRotation();
        GetInput();
        ShotCheck();
        StartCoroutine(OpenInven());

        //Vector3 enemyPosition = target.myEnemyTarget.transform.position;

        //RaycastHit hit;

        //if (Physics.Raycast(enemyPosition, (transform.position - enemyPosition).normalized, out hit, 10f))
        //{
        //    Debug.DrawLine(enemyPosition, hit.point, Color.red, 0.1f);

        //    PlayerCtrl player = hit.collider.GetComponent<PlayerCtrl>();
        //    if (player != null)
        //    {
        //        player.Damage();
        //        Debug.Log("PlayerHit!");
        //    }
        //}
    }

    void FixedUpdate()
    {
        if (!IsMine)
        {
            return;
        }

        GroundCheck();
        InputMoveMent();
        Fall();
        DiveRoll();
        Attack();
       
        speed = statInfo.MovementSpeed;
    }

    void LateUpdate()
    {
        if (!IsMine)
        {
            return;
        }

        CameraRotation();
        //Interaction();
    }

    //void Interaction()
    //{
    //    if (Input.GetKey(KeyCode.E) && target.myPickUpTarget.gameObject != null && enablePickUp == false)
    //    {
    //        if (target.myPickUpTarget.tag == "PickUp")
    //        {
    //            ItemManager item = target.myPickUpTarget.gameObject.GetComponent<ItemManager>();
    //            int pickUpIndex = item.value;
    //            checkPickUp[pickUpIndex] = true;
    //            animator.SetTrigger("isPickUp");
    //            Destroy(target.myPickUpTarget);
    //        }
    //        enablePickUp = true;
    //    }
    //    else
    //    {
    //        enablePickUp = false;
    //    }
    //}

    IEnumerator Hit()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    //void OnControllerColliderHit(ControllerColliderHit col)
    //{
    //    if (col.gameObject.tag == "EnemyWeapon" && attackHit == false)
    //    {
    //        Weapon weapon = col.gameObject.GetComponent<Weapon>();
    //        _curHp -= weapon.damage;
    //        //Debug.Log("PlayerHit");
    //        attackHit = true;
    //        StartCoroutine(HitDamage());
    //    }
    //}

    public void Damage(float damage)
    {
        if (!attackHit)
        {
            StartCoroutine(HitDamage(damage));
        }
    }

    IEnumerator HitDamage(float damage)
    {
        float hitDamage = damage - (statInfo.Defense + statInfo._eqDef) ;
        if (hitDamage > 0)
        {
            statInfo._curHP -= hitDamage;
            GameUIManager.instance.playStateUI.TakeDamage(hitDamage);
            if (statInfo._curHP < hitDamage)
            {
                statInfo._curHP = 0;
            }
        }
        else if(hitDamage <= 0)
        {
            statInfo._curHP -= 1;
        }
        attackHit = true;
        SetTriggerRPC("isHit"); //animator.SetTrigger("isHit");
        Debug.Log("현재 체력: " + statInfo._curHP);
        if (statInfo._curHP <= 0 && !dieCheck)
        {
            StartCoroutine(Die());
        }
        yield return new WaitForSeconds(1f);
        attackHit = false;
    }

    IEnumerator Die()
    {
        PlayerCtrl playerCtrl = this.GetComponent<PlayerCtrl>();

        if(shotReady)
        {
            crossHair.SetActive(false);
            SetBoolRPC("isShotReady", false); //animator.SetBool("isShotReady", false);
            shotReady = false;
        }
        dieCheck = true;
        SetBoolRPC("isAlive", false); //animator.SetBool("isAlive", false);
        SetTriggerRPC("isDie"); //animator.SetTrigger("isDie");
        if (playerCtrl != null)
        {
            playerCtrl.enabled = false;
            this.tag = "Untagged";
        }
        yield return new WaitForSeconds(5f);

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        PlayerCtrl playerCtrl = this.GetComponent<PlayerCtrl>();
        PlayerCtrl[] scripts = FindObjectsOfType<PlayerCtrl>();
        SetBoolRPC("isAlive", true); //animator.SetBool("isAlive", true);

        controller.enabled = false;
        this.transform.position = SpawnPos.transform.position;
        controller.enabled = true;

        foreach (PlayerCtrl script in scripts)
        {
            if (script.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(script);
            }
        }

        if (playerCtrl != null)
        {
            playerCtrl.enabled = true;
            this.tag = "Player";
            dieCheck = false;
            statInfo._curHP = 10;
        }
        open = InventoryManager.instance.isOpen; // 살아났을 때 인벤토리랑 이동에서 생기는 문제 해결을 위한 코드
        yield return null;
    }

    IEnumerator OpenInven()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.B))
        {
            if (open == false)
            {
                SetFloatRPC("Blend", 0); //animator.SetFloat("Blend", 0);
				open = true;
				yield return new WaitForSeconds(0.2f);
            }
			else if (open == true)
			{
				open = false;
				yield return new WaitForSeconds(0.2f);
			}
		}
    }

    public void OnEnemySkillDamaged()
    {
        float particleDamage = particle.Active.damage - (statInfo.Defense + statInfo._eqDef);
        if (particleDamage > 0)
        {
            statInfo._curHP -= particleDamage;
            if (statInfo._curHP < particleDamage)
            {
                statInfo._curHP = 0;
            }
        }
        else if (particleDamage <= 0)
        {
            statInfo._curHP -= 1;
        }
        if (statInfo._curHP <= 0 && !dieCheck)
        {
            StartCoroutine(Die());
        }
    }

    void ShotCheck()
    {
        crossHair = transform.Find("CrossHair").transform.gameObject;
        if (Input.GetMouseButtonDown(1) && open == false && !diveRoll)
        {
            //if (target.myEnemyTarget != null)
            //{
            //    Vector3 direction = target.myEnemyTarget.transform.position - transform.position;
            //    Quaternion rotation = Quaternion.LookRotation(direction);
            //    transform.rotation = rotation;
            //}
            crossHair.SetActive(true);
            SetBoolRPC("isShotReady", true); //animator.SetBool("isShotReady", true);
            shotReady = true;
            if (target.myEnemyTarget == null)
            {
                charaterBody.LookAt(Look.transform.position);
            }
            else
            {
                charaterBody.LookAt(target.myEnemyTarget.transform.position);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            crossHair.SetActive(false);
            SetBoolRPC("isShotReady", false); //animator.SetBool("isShotReady", false);
            shotReady = false;
        }
    }

    void Attack()
    {
        equipWeapon = weapons[weaponsIndex].GetComponent<Weapon>();

        if (equipWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        fireStart = equipWeapon.attackRate < fireDelay;

        if (Input.GetMouseButton(0) && shotReady && fireStart && !shotD)
        {
            equipWeapon.Use();
            SetBoolRPC("isShot", true); //animator.SetBool("isShot", true);
            fireDelay = 0;
            shotD = true;
            if (target.myEnemyTarget == null)
            {
                charaterBody.LookAt(Look.transform.position);
            }
            else
            {
                charaterBody.LookAt(target.myEnemyTarget.transform.position);
            }
            //SoundManager.Instance.PlaySound2D("Voice " + SoundManager.Range(1, 5, true));
            StartCoroutine(ShotDelay());
        }
    }

    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(equipWeapon.attackRate);
        SetBoolRPC("isShot", false); //animator.SetBool("isShot", false);
        shotD = false;
    }

    void Fall()
    {
        if (!groundCheck)
        {
            MoveDir.y -= gravity * Time.deltaTime;
            controller.Move(MoveDir * Time.deltaTime);
        }
    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitInfo, 0.1f))
        {
            if (groundCheck == false)
            {
                SetBoolRPC("isGround", true); //animator.SetBool("isGround", true);
                SetBoolRPC("isFall", false); //animator.SetBool("isFall", false);
            }
            groundCheck = true;
        }
        else
        {
            if (groundCheck == true)
            {
                SetBoolRPC("isGround", false); //animator.SetBool("isGround", false);
                SetBoolRPC("isFall", true); //animator.SetBool("isFall", true);
            }
            groundCheck = false;
        }
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.LeftAlt)) { togglecameraRotation = true; }
        else { togglecameraRotation = false; }

        if (Input.GetKey(KeyCode.LeftShift)) { run = true; }
        else { run = false; }
    }
    void CameraRotation()
    {
        if (togglecameraRotation != true && !open)
        {
            Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    void DiveRoll()
    {
        if (Input.GetKey(KeyCode.Space) && !diveRoll && groundCheck && open == false && !shotReady && statInfo._curMP >= 10f)
        {
            statInfo._curMP -= 10f;
            SoundManager.Instance.PlaySound2D("30_Jump_03_out", 0f, false, SoundType.EFFECT);
            diveDirection = moveDirection;
            SetBoolRPC("isDiveRoll", true); //animator.SetBool("isDiveRoll", true);
            diveRoll = true;
            StartCoroutine(DiveCheck());
            StartCoroutine(DiveRemovePlayerTag());
        }
    }
    IEnumerator DiveCheck()
    {
        yield return new WaitForSeconds(1f);
        SetBoolRPC("isDiveRoll", false); //animator.SetBool("isDiveRoll", false);
        diveRoll = false;
    }

    IEnumerator DiveRemovePlayerTag()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.tag = "Untagged";
            yield return new WaitForSeconds(0.5f);
            player.tag = "Player";
        }
    }

    void InputMoveMent()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        isMove = moveInput.magnitude != 0;
        runSpeed = speed * 4;

        if (open == false )
        {
            if (isMove)
            {
                finalSpeed = (run) ? runSpeed : speed;
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);

                moveDirection = (forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal")).normalized;

                if (diveRoll)
                {
                    controller.Move(charaterBody.forward * (runSpeed * 3f) * Time.deltaTime);
                    moveDirection = diveDirection;
                }
                else
                {
                    controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
                }
                if (!shotReady)
                {
                    charaterBody.forward = moveDirection * Time.deltaTime;
                }


                float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
                SetFloatRPC("Blend", percent, true); //animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
            }

            else if (!isMove)
            {
                if (diveRoll)
                {
                    controller.Move(charaterBody.forward * (runSpeed * 3f) * Time.deltaTime);
                    moveDirection = diveDirection;
                }
                SetFloatRPC("Blend", 0, true); //animator.SetFloat("Blend", 0, 0.1f, Time.deltaTime);
            }
        }
    }


    /// 
    /// Photon Multi
    /// 
    protected override void PhotonSerializeViewData(bool bSend, object[] pvData)
    {
        base.PhotonSerializeViewData(bSend, pvData);
        if (bSend)
        {
            pvData[2] = charaterBody.rotation;
        }
        else
        {
            charaterBody.rotation = (Quaternion)pvData[2];
        }
    }
}