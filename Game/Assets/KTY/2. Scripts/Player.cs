using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public bool isGrounded;  //sh! 원본이 public 이라 private 를 바꿔줌
	public bool isSprinting;

	private Transform cam;

	public float walkSpeed = 3f; // 이동 속도
	public float sprintSpeed = 6f; // 달리기 속도
	public float jumpForce = 5f; // 점프 힘
	public float gravity = -9.8f; // 중력

	public float playerWidth = 0.15f; // 플레이어 너비?
	public float boundsTolerance = 0.1f; // 바운스 허용 오차?

	private float horizontal;
	private float vertical;
	private float mouseHorizontal;
	private float mouseVertical;
	public float mouseRate=10.0f;
	private Vector3 velocity; // 강체 속도

	private float verticalMomemtum = 0; // 수직 운동량?
	private bool jumpRequest;

	//private PhotonView pv;
	//private CinemachineVirtualCamera virtualCamera;

	//private new Transform transform;

	private void Awake()
	{
		cam = GameObject.Find("Main Camera").transform;
	}
    //void Start()
    //{
    //    transform = GetComponent<Transform>();
    //    pv = GetComponent<PhotonView>();
    //    virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

    //     자신의 캐릭터일 경우 시네머신 카메라를 연결
    //    if (pv.isMine)
    //    {
    //        virtualCamera.Follow = transform;
    //        virtualCamera.LookAt = transform;

    //    }
    //}

    private void Update()
	{
		GetPlayerInputs();
	}

	private void FixedUpdate()
	{
		CalculateVelocity();

		if (jumpRequest)
			Jump();

		transform.Rotate(Vector3.up * mouseHorizontal* mouseRate); // 마우스 수평 이동 시 y축 회전
		cam.Rotate(Vector3.right * -mouseVertical*mouseRate);

		// Space.~: 이동을 적용할 기준 공간을 나타내는 열거형
		// Space.World: 해당 오브젝트를 속도(velocity)와 방향에 따라 월드 공간에서 이동
		// Space.Self: 해당 오브젝트의 로컬 공간에서의 방향과 속도에 따라 이동
		transform.Translate(velocity, Space.World);
	}

	void Jump()
	{
		verticalMomemtum = jumpForce; // 수직 운동량은 점프 힘과 같다.
		isGrounded = false;
		jumpRequest = false;
	}

	private void CalculateVelocity()
	{
		//// 플레이어의 입력에 따라 전후방 및 좌우 방향으로의 이동속도를 계산.
		//velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.deltaTime * 3f;
		//velocity += Vector3.up * gravity * Time.deltaTime;

		//velocity.y = CheckDownSpeed(velocity.y);


		// Affect vertical momentum with gravity
		// 중력이 수직 운동량에 주는 영향 (대충 뛸 때 감속되고 떨어질 때 가속되는 효과?)
		if (verticalMomemtum > gravity)
			verticalMomemtum += Time.fixedDeltaTime * gravity; // Time.fixedDeltaTime : 고정된 델타타임

		if (isSprinting)
			velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * sprintSpeed;
		else
			velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * walkSpeed;

		// 수직 운동량 적용
		velocity += Vector3.up * verticalMomemtum * Time.fixedDeltaTime;
    }

	// 플레이어 입력
	private void GetPlayerInputs()
	{
		horizontal = Input.GetAxis("Horizontal");	// 키보드 수평 이동
		vertical = Input.GetAxis("Vertical");		// 키보드 수직 이동
		mouseHorizontal = Input.GetAxis("Mouse X"); // 마우스 수평 이동
		mouseVertical = Input.GetAxis("Mouse Y");   // 마우스 수직 이동

		if (Input.GetButtonDown("Sprint"))
			isSprinting = true;
		if (Input.GetButtonUp("Sprint"))
			isSprinting = false;

		if (isGrounded && Input.GetButtonDown("Jump"))
			jumpRequest = true;
	}

}
