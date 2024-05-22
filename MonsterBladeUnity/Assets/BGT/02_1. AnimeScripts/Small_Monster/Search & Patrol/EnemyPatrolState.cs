using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
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
        if (Physics.Raycast(_enemy.transform.position, _direction, 3f))
        {
            _direction = new Vector3(Random.Range(-10.0f, 10.0f), 0f, Random.Range(-10.0f, 10.0f)).normalized;
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(_direction);
        _enemy.charaterBody.rotation = Quaternion.Slerp(_enemy.charaterBody.rotation, targetRotation, _enemy._rotationSpeed * Time.deltaTime);
        _enemy.transform.Translate(_direction * _enemy._speed * Time.deltaTime, Space.World);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
