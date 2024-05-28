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
        RaycastHit rayInfo;
        Vector3 bossPosition = _boss.transform.position;
        Vector3 bossRaycast = _boss.transform.position + new Vector3(0f, 1f, 0f);
        Vector3 playerPosition = _boss._player.transform.position;
        Vector3 playerRaycast = _boss._player.transform.position + new Vector3(0f, 1f, 0f);

        if (originDistance > _boss._maxMoveRange)
        {
            _boss._player.isTarget = false;
            animator.SetBool("isTarget", false);
            animator.SetBool("isReset", true);
        }

        if (Physics.Raycast(bossRaycast, (playerRaycast - bossRaycast).normalized, out rayInfo, distance))
        {
            if (rayInfo.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                if (distance < _boss._searchRange)
                {
                    _boss._player.isTarget = false;
                    animator.SetBool("isTarget", false);
                }
            }
        }

        if (distance > _boss._searchRange)
        {
            _boss._player.isTarget = false;
            animator.SetBool("isTarget", false);
        }

        Vector3 dt = _boss._player.transform.position - _boss.transform.position;
        Vector3 delta = new Vector3(dt.x, 0f, dt.z);
        Vector3 moveDirection = delta.normalized * _boss._speed * Time.deltaTime;

        _boss.transform.Translate(moveDirection, Space.World);
        _boss.charaterBody.forward = delta;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}