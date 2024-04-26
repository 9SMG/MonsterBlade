using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    GameObject[] Enemys;
    Transform[] EnemySpawnPoints;
    PlayerCtrl player;
    public GameObject enemyPrefab;

    void Awake()
    {
        EnemySpawnPoints = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Start()
    {
        if (!player.dieCheck)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    void InstantiateEnemy(Vector3 position, Quaternion rotation)
    {
        GameObject enemyObject = Instantiate(enemyPrefab, position, rotation);
    }

    IEnumerator CreateEnemy()
    {
        while (!player.dieCheck)
        {
            yield return new WaitForSeconds(5.0f);

            Enemys = GameObject.FindGameObjectsWithTag("Enemy");

            if (Enemys.Length < 10)
            {
                for (int i = 0; i < EnemySpawnPoints.Length; i++)
                {
                    InstantiateEnemy(EnemySpawnPoints[i].position, EnemySpawnPoints[i].rotation);
                }
            }
        }
    }
}
