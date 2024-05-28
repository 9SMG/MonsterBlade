using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public bool targetSet;
    public float checkEnemy = 15.0f;
    public float EnemyTargetDistance = 15.0f;
    public float checkPickUp = 1f;
    public float pickUpTargetDistance = 1f;
    public List<GameObject> enemyTargets = new List<GameObject>();
    public GameObject myEnemyTarget;
    public GameObject enemyTargetImage;
    //public GameObject player;
    LayerMask enemyLayer;

    public List<GameObject> pickUpTargets = new List<GameObject>();
    public GameObject myPickUpTarget;
    public GameObject pickUpTargetImage;
    LayerMask pickUpLayer;

    void Awake()
    {
        targetSet = false;
        //player = GameObject.Find("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        pickUpLayer = LayerMask.NameToLayer("PickUp");
    }

    void Update()
    {
        StartCoroutine(TarGetSetting());
        SearchPickUpTarget();
        SearchEnemyTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkEnemy);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkPickUp);
    }

    IEnumerator TarGetSetting()
    {
        if (Input.GetKey(KeyCode.C) && targetSet == false)
        {
            yield return new WaitForSeconds(0.2f);
            targetSet = true;
        }
        else if (Input.GetKey(KeyCode.C) && targetSet == true)
        {
            yield return new WaitForSeconds(0.2f);
            targetSet = false;
        }
    }

    void SearchEnemyTarget()
    {
        if (targetSet == true)
        {
            enemyTargets.Clear();
            Collider[] Distance = Physics.OverlapSphere(transform.position, EnemyTargetDistance, 1 << enemyLayer);

            foreach (Collider col in Distance)
            {
                if (col.tag == "Player")
                {
                    continue;
                }
                enemyTargets.Add(col.transform.gameObject);
            }

            if (enemyTargets.Count != 0)
            {
                if (enemyTargetImage != null)
                {
                    enemyTargetImage.SetActive(false);
                }

                myEnemyTarget = enemyTargets[0];
                float curtarget = Vector3.Distance(transform.position, enemyTargets[0].transform.position);

                for (int i = 1; i < enemyTargets.Count; i++)
                {
                    float dist = Vector3.Distance(transform.position, enemyTargets[i].transform.position);

                    if (dist < curtarget)
                    {
                        myEnemyTarget = enemyTargets[i];
                        curtarget = dist;
                    }
                }

                enemyTargetImage = myEnemyTarget.transform.Find("Canvas").transform.gameObject;
                enemyTargetImage.SetActive(true);
            }

            else if (enemyTargets.Count == 0 && enemyTargetImage != null)
            {
                myEnemyTarget = null;
                enemyTargetImage.SetActive(false);
            }
        }
        else if (enemyTargetImage != null && targetSet == false)
        {
            enemyTargetImage.SetActive(false);
        }
    }

    void SearchPickUpTarget()
    {
        if (targetSet == true)
        {
            pickUpTargets.Clear();
            Collider[] Distance = Physics.OverlapSphere(transform.position, pickUpTargetDistance, 1 << pickUpLayer);

            foreach (Collider col in Distance)
            {
                if (col.tag == "Player")
                {
                    continue;
                }
                pickUpTargets.Add(col.transform.gameObject);
            }

            if (pickUpTargets.Count != 0)
            {
                if (pickUpTargetImage != null)
                {
                    pickUpTargetImage.SetActive(false);
                }

                myPickUpTarget = pickUpTargets[0];
                float curtarget = Vector3.Distance(transform.position, pickUpTargets[0].transform.position);

                for (int i = 1; i < pickUpTargets.Count; i++)
                {
                    float dist = Vector3.Distance(transform.position, pickUpTargets[i].transform.position);

                    if (dist < curtarget)
                    {
                        myPickUpTarget = pickUpTargets[i];
                        curtarget = dist;
                    }
                }

                pickUpTargetImage = myPickUpTarget.transform.Find("Canvas").transform.gameObject;
                pickUpTargetImage.SetActive(true);
            }

            else if (pickUpTargets.Count == 0 && pickUpTargetImage != null)
            {
                myPickUpTarget = null;
                pickUpTargetImage.SetActive(false);
            }
        }
        else if (pickUpTargetImage != null && targetSet == false)
        {
            pickUpTargetImage.SetActive(false);
        }
    }
}
