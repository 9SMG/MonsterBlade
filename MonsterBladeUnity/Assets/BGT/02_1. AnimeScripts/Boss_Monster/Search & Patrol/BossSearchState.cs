using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSearchState : BossState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_boss == null)
        {
            Debug.LogError("_enemy object is not initialized.");
            return;
        }

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

        if (distance < _boss._searchRange)
        {
            if (Physics.Raycast(bossRaycast, (playerRaycast - bossRaycast).normalized, out rayInfo, distance))
            {
                if (rayInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.DrawLine(bossPosition, rayInfo.point, Color.green, 0.1f);
                    _boss._player.isTarget = true;
                    animator.SetBool("isTarget", true);
                }

                else
                {
                    Debug.DrawLine(bossRaycast, rayInfo.point, Color.red, 0.1f);
                    _boss._player.isTarget = false;
                    animator.SetBool("isTarget", false);
                }
            }
        }
        //Vector3 delta = _enemy.originPos - _enemy.transform.position;
        //_enemy.transform.position += delta.normalized * _enemy._speed * Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
