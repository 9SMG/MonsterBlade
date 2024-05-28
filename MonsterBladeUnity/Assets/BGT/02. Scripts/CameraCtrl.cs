using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform objectToFollow;
    public float followSpeed = 10f;
    private float sensitivity = 100f;
    public float clampAngle = 70f;
    public float zoomSpeed = 5f;

    float rotX;
    float rotY;
    float baseDistance = 4f;

    public Transform mainCamera;
    public Vector3 dirNormalized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10.0f;

    bool lockCheck=false;

    public PlayerCtrl player;
    Enemy enemy;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = mainCamera.localPosition.normalized;
        finalDistance = mainCamera.localPosition.magnitude;

        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CameraMove();
        if(Input.GetKeyDown(KeyCode.F1) && !lockCheck)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            lockCheck = true;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && lockCheck)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            lockCheck = false;
        }
    }

    void LateUpdate()
    {
        CameraZoom();
        ShotCheck();
        CameraDisturbance();
    }

    void CameraDisturbance()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                finalDistance = maxDistance;
            }
            else
            {
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
        }
        else
        {
            finalDistance = maxDistance;
        }
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

    void CameraMove()
    {
        if (player.open == false)
        {
            rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
            rotY += (Input.GetAxis("Mouse X")) * sensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            transform.rotation = rot;
        }
    }
    void CameraZoom()
    {
        if (player.open == false && !player.isTarget)
        {
            float scrollWheelInput = -(Input.GetAxis("Mouse ScrollWheel"));

            if ((maxDistance >= 5f && scrollWheelInput > 0) || (maxDistance <= 0.7f && scrollWheelInput < 0))
            {
                return;
            }

            maxDistance += scrollWheelInput * zoomSpeed;
        }
    }
    void ShotCheck()
    {
        if (player.open == false && !player.diveRoll)
        {
            if (Input.GetMouseButtonDown(1))
            {
                maxDistance = 1.5f;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                maxDistance = baseDistance;
            }
        }
    }
}
