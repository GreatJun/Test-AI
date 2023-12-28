using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarEnemyAI : MonoBehaviour
{
    [Header("Range")]
    [SerializeField]
    float _detectRange = 10f;
    [SerializeField]
    float _AttackRange = 5f;

    [Header("Movement")]
    [SerializeField]
    float _movementSpeed = 5f;

    BehaviourTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Vector3 _originPos = default;
    Animator _animator = null;

    private void Awake()
    {
        _BTRunner = new BehaviourTreeRunner(SettingBT());
        _animator = GetComponent<Animator>();
        _originPos = transform.position;
    }

    private void Update()
    {
        _BTRunner.Operate();
    }

    INode SettingBT()
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

    #region Attack Node
    INode.EnodeState CheckAttacking()
    {
        return INode.EnodeState.ENS_Success;
    }

    INode.EnodeState CheckEnemyWithineAttackRange()
    {
        return INode.EnodeState.ENS_Failure;
    }

    INode.EnodeState DoAttack()
    {
        return INode.EnodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    INode.EnodeState CheckDetectEnemy()
    {
        return INode.EnodeState.ENS_Failure;
    }

    INode.EnodeState MoveToDetectEnemy()
    {
        return INode.EnodeState.ENS_Failure;
    }
    #endregion

    #region Move Origin Position Node
    INode.EnodeState MoveToOriginPosition()
    {
        return 0;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _AttackRange);
    }
}
