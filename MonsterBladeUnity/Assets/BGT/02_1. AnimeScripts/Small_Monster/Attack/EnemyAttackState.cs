using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //_enemy.Attack();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 enemyPosition = _enemy.transform.position;
        Vector3 enemyRaycast = _enemy.transform.position + new Vector3(0f, 1f, 0f);
        Vector3 playerPosition = _enemy._player.transform.position;
        Vector3 playerRaycast = _enemy._player.transform.position + new Vector3(0f, 1f, 0f);

        RaycastHit hit;

        if (Physics.Raycast(enemyRaycast, (playerRaycast - enemyRaycast).normalized, out hit, _enemy._attackRange))
        {
            Debug.DrawLine(enemyRaycast, hit.point, Color.red, 0.1f);

            PlayerCtrl player = hit.collider.GetComponent<PlayerCtrl>();
            if (player != null)
            {
                player.Damage(_enemy._damage);
            }
        }
    }
}
