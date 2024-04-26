using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float originDistance = Vector3.Distance(_enemy.transform.position, _enemy.originPos);
        float distance = Vector3.Distance(_enemy.transform.position, _enemy._player.transform.position);

        if (originDistance > 15f)
        {
            animator.SetBool("isTarget", false);
            animator.SetBool("isReset", true);
        }

        if (distance > _enemy._range)
        {
            animator.SetBool("isTarget", false);
        }

        Vector3 delta = _enemy._player.transform.position - _enemy.transform.position;
        _enemy.transform.position += delta.normalized * _enemy._speed * Time.deltaTime;
        _enemy.charaterBody.forward = delta;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
