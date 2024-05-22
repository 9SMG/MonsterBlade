using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResetPositionState : EnemyState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //resetPosition = _enemy.transform.position;
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float originDistance = Vector3.Distance(_enemy.transform.position, _enemy.originPos);

        Vector3 delta = _enemy.originPos - _enemy.transform.position;
        _enemy.transform.position += delta.normalized * _enemy._speed * Time.deltaTime;
        _enemy.charaterBody.forward = delta;
        if (originDistance < 0.5f)
        {
            animator.SetBool("isReset", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy._curHp = _enemy._maxHp;
    }
}
