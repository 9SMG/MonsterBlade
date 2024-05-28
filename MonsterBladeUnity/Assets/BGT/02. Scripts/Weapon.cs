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
    public GameObject targetObject;
    TargetManager target;

    void Awake()
    {
        targetObject = GameObject.FindGameObjectWithTag("Target");
        target = GameObject.FindWithTag("Player").GetComponent<TargetManager>();
    }

    void FixedUpdate()
    {
        MoveTargetToCameraCenter();
    }

    void MoveTargetToCameraCenter()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
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
        Arrow arrow = instantArrow.GetComponent<Arrow>();
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();

        arrow.damage = damage;

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
