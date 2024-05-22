using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossResetPositionState : BossState
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
        float originDistance = Vector3.Distance(_boss.transform.position, _boss.originPos);

        Vector3 delta = _boss.originPos - _boss.transform.position;
        _boss.transform.position += delta.normalized * _boss._speed * Time.deltaTime;
        _boss.charaterBody.forward = delta;
        if (originDistance < 0.5f)
        {
            animator.SetBool("isReset", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _boss._curHp = _boss._maxHp;
        _boss._legHp = _boss._maxHp / 3;
        _boss._wingHp = _boss._maxHp / 3;
        _boss._headHp = _boss._maxHp / 3;
    }
}
