using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Range")]
    [SerializeField]
    protected float _detectDistance;   // 인식 거리
    [SerializeField]
    protected float _attackDistance;    // 공격 거리

    [Header("Movement")]
    [SerializeField]
    protected float _movementSpeed;

    protected BehaviourTreeRunner _BTRunner = null;
    protected Transform _detectedPlayer = null;
    protected Vector3 _originPos = default;
    protected Animator _animator = null;

    protected const string _ATTACK_ANIM_STATE_NAME = "Attack";
    protected const string _ATTACK_ANIM_TIRGGER_NAME = "IsAttack";

    protected void Awake()
    {
        _BTRunner = new BehaviourTreeRunner(SettingBT());
        _animator = GetComponentInChildren<Animator>();
        _originPos = transform.position;
    }

    protected void Update()
    {
        _BTRunner.Operate();
    }

    protected INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckAttacking), // 공격중?
                            new ActionNode(CheckEnemyWithineAttackRange), // 공격 범위 안?
                            new ActionNode(DoAttack) // 공격
                        }
                    ),
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(CheckDetectEnemy), // 적 발견?
                            new ActionNode(MoveToDetectEnemy) // 적한테 이동
                        }
                    ),
                    new ActionNode(MoveToOriginPosition) // 원래 자리로
                }
            );
    }

    protected bool IsAnimationRunning(string stateName)
    {
        if (_animator != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) // (stateName) 애니메이션이 진행중인가?
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }

        return false;
    }

    #region Attack Node
    protected INode.ENodeState CheckAttacking()
    {
        if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
        {
            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Success;
    }

    protected INode.ENodeState CheckEnemyWithineAttackRange()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackDistance * _attackDistance)) // 제곱근(피타고라스)
            {
                return INode.ENodeState.ENS_Success;
            }
        }

        return INode.ENodeState.ENS_Failure;
    }

    protected INode.ENodeState DoAttack()
    {
        if (_detectedPlayer != null)
        {
            _animator.SetTrigger(_ATTACK_ANIM_TIRGGER_NAME);
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    protected INode.ENodeState CheckDetectEnemy()
    {
        // var overlapColliders = Physics.OverlapSphere(transform.position, _detectDistance, LayerMask.GetMask("Player")); // OverlapSphere : 구 형태로 "주변 콜라이더" 감지 <= 3D
        var overlapColliders = Physics2D.OverlapCircleAll(transform.position, _detectDistance, LayerMask.GetMask("Player"));

        if (overlapColliders != null && overlapColliders.Length > 0)
        {
            _detectedPlayer = overlapColliders[0].transform;

            return INode.ENodeState.ENS_Success;
        }

        _detectedPlayer = null;

        return INode.ENodeState.ENS_Failure;
    }

    protected virtual INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
/*          if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackDistance * _attackDistance))
            {
                transform.position = Vector3.MoveTowards(transform.position, -_detectedPlayer.position, Time.deltaTime * _movementSpeed);
                return INode.ENodeState.ENS_Running;
            }*/

            transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);

            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Move Origin Position Node
    protected INode.ENodeState MoveToOriginPosition()
    {
        if (Vector3.SqrMagnitude(_originPos - transform.position) < float.Epsilon * float.Epsilon) // Epsilon : 수학에서 매우 작은 수를 의미하는 기호
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _originPos, Time.deltaTime * _movementSpeed);
            return INode.ENodeState.ENS_Running;
        }
    }
    #endregion

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _attackDistance);
    }
}
