using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatrolState : BossState
{
    Vector3 _direction;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _direction = new Vector3(Random.Range(-10.0f, 10.0f), 0f, Random.Range(-10.0f, 10.0f)).normalized;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Physics.Raycast(_boss.transform.position, _direction, 10f))
        {
            _direction = new Vector3(Random.Range(-10.0f, 10.0f), 0f, Random.Range(-10.0f, 10.0f)).normalized;
            return;
        }
        _boss.charaterBody.forward = _direction;
        _boss.transform.position += _direction * _boss._speed * Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
