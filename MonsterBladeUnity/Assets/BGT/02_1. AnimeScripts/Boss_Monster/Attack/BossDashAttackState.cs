using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashAttackState : BossState
{
    Vector3 _Dash;
    public float speedRate;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _Dash = (_boss._player.transform.position - _boss.transform.position).normalized;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector2.Distance(_boss.transform.position, _boss._player.transform.position);

        if (distance > _boss._searchRange)
        {
            animator.SetBool("hasTarget", false);
        }

        _boss.transform.Translate(_Dash * _boss._speed * Time.deltaTime * speedRate);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
