using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public PlayerCtrl player;
    public ActiveSkillManager Active;
    public Transform fallPlayer;
    public float damageAmount = 10f;
    public float raycastDistance = 10f;
    public bool Hitcheck = false;

    ParticleSystem ps;

    void Awake()
    {
        fallPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ps = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        ps.trigger.SetCollider(0, fallPlayer);
    }

    void Update()
    {
        if (player == null)
        {
            Debug.Log("Null");
            player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        }
        if (fallPlayer == null)
        {
            Debug.Log("Null");
            fallPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            ps.trigger.SetCollider(0, fallPlayer);
        }
    }

    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
        int numParticlesInside = particles.Count;

        for (int i = 0; i < numParticlesInside; i++)
        {
            Vector3 particlePosition = particles[i].position;
            Vector3 particleRaycast = transform.position + new Vector3(0f, 1f, 0f);
            Vector3 playerPosition = fallPlayer.transform.position;
            Vector3 playerRaycast = fallPlayer.transform.position + new Vector3(0f, 1f, 0f);
            RaycastHit hit;

            if (Physics.Raycast(particleRaycast, (playerRaycast - particleRaycast).normalized, out hit, raycastDistance))
            {
                Debug.DrawLine(particleRaycast, hit.point, Color.red, 0.1f);

                PlayerCtrl player = hit.collider.GetComponent<PlayerCtrl>();
                if (player != null && Hitcheck == false)
                {
                    Hitcheck = true;
                    player.OnEnemySkillDamaged();
                    Debug.Log("PlayerHit!");
                    StartCoroutine(PlayerHitcheck());
                }
            }
        }
    }

    IEnumerator PlayerHitcheck()
    {
        yield return new WaitForSeconds(4f);
        Hitcheck = false;
    }
}















//public ActiveSkillManager Active;
//ParticleSystem ps;
//List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

//void Awake()
//{
//    ps = GetComponent<ParticleSystem>();    
//}

////void OnParticleCollision(GameObject other)
////{
////    PlayerCtrl player = other.GetComponent<PlayerCtrl>();
////    if (player != null && player.gameObject.layer == 9)
////    {
////        player.OnEnemySkillDamaged();
////        Debug.Log("PlayerHit");
////    }
////}

//void OnParticleTrigger()
//{
//    ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
//    foreach (var v in inside)
//    {
//        Collider collider = gameObject.GetComponent<Collider>();
//        if (collider != null)
//        {
//            Debug.Log("ColliderTrue!");
//            PlayerCtrl player = collider.GetComponent<PlayerCtrl>();
//            if (player != null)
//            {
//                player.OnEnemySkillDamaged();
//                Debug.Log("PlayerHit!");
//            }
//        }
//    }
//}

//IEnumerator PlayerHit()
//{
//    yield return new WaitForSeconds(5f);

//}
