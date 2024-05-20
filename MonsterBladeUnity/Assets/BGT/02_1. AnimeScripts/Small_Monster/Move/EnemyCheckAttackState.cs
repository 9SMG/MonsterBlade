using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckAttackState : EnemyState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(_enemy.transform.position, _enemy._player.transform.position);
        int randomNumber = Random.Range(1, 4);

        if (distance < _enemy._attackRange && randomNumber == 1)
        {
            animator.SetTrigger("isAttack_1");
        }
        if (distance < _enemy._attackRange && randomNumber == 2)
        {
            animator.SetTrigger("isAttack_2");
        }
        if (distance < _enemy._attackRange && randomNumber == 3)
        {
            animator.SetTrigger("isAttack_3");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
