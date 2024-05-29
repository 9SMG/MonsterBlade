using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFireBall : MonoBehaviour, IResetTarget
{
    public GameObject boss;
    public GameObject player;
    public GameObject fireBallLinePrefab;
    public GameObject particlePrefab;
    public float spawnRadius = 5.0f;
    public float drawingTime = 1.0f;
    public float spawnTime = 1.0f;
    public float setTime = 10.0f;


    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void FireBallStart()
    {
        StartCoroutine(SpawnFireBall(setTime));
    }

    IEnumerator SpawnFireBall(float time)
    {
        float startTime = Time.time;
        while (Time.time - startTime < time)
        {
            if (boss != null && player != null)
            {
                Vector3 bossPosition = boss.transform.position;
                FireBall fireBall = FireBall.NewFireBallLine(fireBallLinePrefab);
                fireBall.CreateFireBall(bossPosition, player.transform, drawingTime, particlePrefab);
            }
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void ResetTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
