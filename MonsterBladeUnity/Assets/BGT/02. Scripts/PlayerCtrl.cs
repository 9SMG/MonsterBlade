using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] public Transform charaterBody;
    Vector3 moveDirection;
    Vector3 MoveDir;
    Vector3 diveDirection;
    Animator animator;  
    Camera camera;
    CharacterController controller;
    Ray ray;
    RaycastHit hitInfo;
    Weapon equipWeapon;
    SpriteRenderer _spriteRenderer;
    TargetManager target;
    ParticleDamage particle;

    public GameObject[] pickUpItem;
    public bool[] checkPickUp = new bool[3];
    public float speed = 5f; 
    public float runSpeed = 8f;  
    public float finalSpeed;
    public float rotationSpeed;
    public float smoothness = 10f;
    public float diveSpeed;
    public float gravity;
    public float _curHp;
    public float _maxHp;
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

    float fireDelay;
    bool fireStart;
    bool groundCheck;
    bool isMove;
    int weaponsIndex = 0;

    void Awake()
    {
        particle = GameObject.FindWithTag("EnemySkill").GetComponent<ParticleDamage>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
        controller = GetComponent<CharacterController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        target = GetComponent<TargetManager>();
        dieCheck = false;
        open = false;
        shotD = false;
        isTarget = false;
        isMove = false;
        animator.SetBool("isAlive", true);
    }

    void Start()
    {
        diveSpeed = 10.0f;
        gravity = 5.0f;
        MoveDir = Vector3.zero;
        _curHp = _maxHp;
        attackHit = false;
    }

    void Update()
    {
        CameraRotation();
        GetInput();
        ShotCheck();
        StartCoroutine(OpenInven());

        if(Input.GetKey(KeyCode.Keypad2))
        {
            
        }

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
        GroundCheck();
        InputMoveMent();
        Fall();
        DiveRoll();
        Attack();
    }

    void LateUpdate()
    {
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
        StartCoroutine(HitDamage(damage));
    }

    IEnumerator HitDamage(float damage)
    {
        animator.SetTrigger("isHit");
        _curHp -= damage;
        if (_curHp <= 0 && !dieCheck)
        {
            StartCoroutine(Die());
        }
        Debug.Log("Hp - !!");
        yield return new WaitForSeconds(1f);
        attackHit = false;
    }

    IEnumerator Die()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerCtrl playerCtrl = player.GetComponent<PlayerCtrl>();
        dieCheck = true;
        animator.SetBool("isAlive", false);
        animator.SetTrigger("isDie");
        if (playerCtrl != null)
        {
            playerCtrl.enabled = false;
            player.tag = "Untagged";
        }
        yield return null;
    }

    IEnumerator OpenInven()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (open == false)
            {
                Inven.SetActive(true);
                Cursor.visible = true;
                yield return new WaitForSeconds(0.2f);
                open = true;
            }
            else if(open == true)
            {
                Inven.SetActive(false);
                Cursor.visible = false;
                yield return new WaitForSeconds(0.2f);
                open = false;
            }
        }
    }

    public void OnEnemySkillDamaged()
    {
        _curHp -= particle.Active.damage;
        if (_curHp <= 0 && !dieCheck)
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
            animator.SetBool("isShotReady", true);
            shotReady = true;
        }
        else if(Input.GetMouseButtonUp(1) && open == false)
        {
            crossHair.SetActive(false);
            animator.SetBool("isShotReady", false);
            shotReady = false;
        }
    }

    void Attack()
    {
        equipWeapon = weapons[weaponsIndex].GetComponent<Weapon>();

        if(equipWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        fireStart = equipWeapon.attackRate < fireDelay;

        if(Input.GetMouseButton(0) && shotReady && fireStart && !shotD && !isMove)
        {
            equipWeapon.Use();
            animator.SetBool("isShot",true);
            fireDelay = 0;
            shotD = true;
            //SoundManager.Instance.PlaySound2D("Voice " + SoundManager.Range(1, 5, true));
            StartCoroutine(ShotDelay());
        }
    }

    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(equipWeapon.attackRate);
        animator.SetBool("isShot", false);
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
            groundCheck = true;
            animator.SetBool("isGround", true);
            animator.SetBool("isFall", false);
        }
        else
        {
            groundCheck = false;
            animator.SetBool("isGround", false);
            animator.SetBool("isFall", true);
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
        if (togglecameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    void DiveRoll()
    {
        if (Input.GetKey(KeyCode.Space) && !diveRoll && groundCheck)
        {
            diveDirection = moveDirection;
            animator.SetBool("isDiveRoll", true);
            diveRoll = true;
            StartCoroutine(DiveCheck());
            StartCoroutine(DiveRemovePlayerTag());
        }
    }
    IEnumerator DiveCheck()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isDiveRoll", false);
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
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0 , Input.GetAxisRaw("Vertical"));
        isMove = moveInput.magnitude != 0;

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
            charaterBody.forward = moveDirection * Time.deltaTime;


            float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
            animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);

        }

        else if(!isMove)
        {
            animator.SetFloat("Blend", 0, 0.1f, Time.deltaTime);
            if (diveRoll)
            {
                controller.Move(charaterBody.forward * (runSpeed * 3f) * Time.deltaTime);
                moveDirection = diveDirection;
            }
        }
    }
}