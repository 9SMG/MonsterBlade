using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : BossState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float originDistance = Vector3.Distance(_boss.transform.position, _boss.originPos);
        float distance = Vector3.Distance(_boss.transform.position, _boss._player.transform.position);

        if (originDistance > _boss._maxMoveRange)
        {
            _boss._player.isTarget = false;
            animator.SetBool("isTarget", false);
            animator.SetBool("isReset", true);
        }

        if (distance > _boss._searchRange)
        {
            _boss._player.isTarget = false;
            animator.SetBool("isTarget", false);
        }

        Vector3 delta = _boss._player.transform.position - _boss.transform.position;
        _boss.transform.position += delta.normalized * _boss._speed * Time.deltaTime;
        _boss.charaterBody.forward = delta;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
