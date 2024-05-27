using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Ranger };
    public Type type;
    public float damage;
    public float attackRate;
    public float shotSpeed;
    public BoxCollider meleeArea;
    public TrailRenderer trailRenderer;
    public Transform ArrowPos;
    public GameObject Arrow;
    
    

    /// temp ///
    public Camera myCam;
    public TargetManager target;
    public GameObject targetObject;

    void Awake()
    {
        //targetObject = GameObject.FindGameObjectWithTag("Target");
        //target = GameObject.FindWithTag("Player").GetComponent<TargetManager>();
        
        //Camera[] cams = transform.root.GetComponentsInChildren<Camera>();
        //foreach (Camera cam in cams)
        //{
        //    if (cam.CompareTag("MainCamera"))
        //    {
        //        myCam = cam;
        //        break;
        //    }
        //}
    }

    [ContextMenu("DebugLogCam")]
    void DebugLogMainCam()
    {
        Debug.Log("Camera.main.root: " + myCam.transform.root.name);
    }

    void FixedUpdate()
    {
        MoveTargetToCameraCenter();
    }

    void MoveTargetToCameraCenter()
    {
        Vector3 cameraPosition = myCam.transform.position;
        Vector3 cameraForward = myCam.transform.forward;
        Vector3 targetPosition = cameraPosition + cameraForward * 50f;

        targetObject.transform.position = targetPosition;
    }

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Ranger)
        {
            StartCoroutine("Shot");
        }
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailRenderer.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailRenderer.enabled = false;
    }

    IEnumerator Shot()
    {
        GameObject instantArrow = Instantiate(Arrow, ArrowPos.position, ArrowPos.rotation);
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();

        if (target.myEnemyTarget != null && target.targetSet == true)
        {
            Vector3 direction = target.myEnemyTarget.transform.position - ArrowPos.position;
            direction.y += 1.5f;

            Quaternion rotation = Quaternion.LookRotation(direction);
            instantArrow.transform.rotation = rotation;

            arrowRigid.velocity = direction.normalized * shotSpeed;
        }
        else
        {
            Vector3 direction = targetObject.transform.position - ArrowPos.transform.position;

            arrowRigid.velocity = direction.normalized * shotSpeed;
        }

        yield return null;
    }
}
