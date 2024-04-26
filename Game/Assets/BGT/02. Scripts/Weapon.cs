using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Ranger };
    public Type type;
    public float damage;
    public float attackRate;
    public CapsuleCollider meleeArea;
    public TrailRenderer trailRenderer;
    public Transform ArrowPos;
    public GameObject Arrow;
    TargetManager target;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").GetComponent<TargetManager>();
    }

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.Ranger)
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

        if (target.myEnemyTarget != null)
        {
            Vector3 direction = target.myEnemyTarget.transform.position - ArrowPos.position;
            direction.y += 1.5f;

            Quaternion rotation = Quaternion.LookRotation(direction);
            instantArrow.transform.rotation = rotation;

            arrowRigid.velocity = direction.normalized * 50;
        }
        else
        {
            arrowRigid.velocity = ArrowPos.forward * 50;
        }

        yield return null;
    }
}
