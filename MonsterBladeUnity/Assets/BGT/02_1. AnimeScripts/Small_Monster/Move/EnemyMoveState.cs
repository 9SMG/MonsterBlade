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

        if (originDistance > _enemy._maxMoveRange)
        {
            _enemy._player.isTarget = false;
            animator.SetBool("isTarget", false);
            animator.SetBool("isReset", true);
        }

        if (distance < _enemy._searchRange)
        {
            if (!_enemy._player.dieCheck)
            {
                _enemy._player.isTarget = true;
                animator.SetBool("isTarget", true);
            }
            else
            {
                _enemy._player.isTarget = false;
                animator.SetBool("isTarget", false);
            }
        }
        Vector3 delta = _enemy._player.transform.position - _enemy.transform.position;
        Vector3 direction = new Vector3(delta.x, 0f, delta.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _enemy.charaterBody.rotation = Quaternion.Slerp(_enemy.charaterBody.rotation, targetRotation, _enemy._rotationSpeed * Time.deltaTime);

        // �̵��� ���� ����
        Vector3 moveDirection = direction * _enemy._speed * Time.deltaTime;

        // ���Ͱ� �̵��� ��ġ ���
        Vector3 newPosition = _enemy.transform.position + moveDirection;

        // �̵��� ��ġ�� Raycast�� ������ �ٸ� ���Ϳ� �浹�ϴ��� �˻�
        RaycastHit hit;
        if (Physics.Raycast(_enemy.transform.position, moveDirection, out hit, 1f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                _enemy._player.isTarget = false;
                animator.SetBool("isTarget", false);
                Vector3 newDirection = Vector3.RotateTowards(_enemy.transform.forward, _enemy.transform.right, Mathf.PI / 4, 0.0f);
                _enemy.transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
        else
        {
            // �浹�� ������ �̵��մϴ�.
            _enemy.transform.Translate(moveDirection, Space.World);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
