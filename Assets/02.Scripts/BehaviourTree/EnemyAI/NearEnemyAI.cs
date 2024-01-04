using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class NearEnemyAI : Enemy
{
    

    private float coolTime = 20f;
    private bool isSpecialAttacking = false;


    private const string _CRASH_ANIM_TRIGGER_NAME = "IsCrash";

    public NearEnemyAI() : base()
    {
        this._detectDistance = 4;
        this._attackDistance = 1;
        this._movementSpeed = 3;
        this._actionDistance = 3;
        this._isCoolTime = true;
    }

    protected override INode SettingBT()
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
                            new InverterNode
                            (
                                new List<INode>()
                                {
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(CheckCoolTime),  // 쿨타임 체크
                                            new ActionNode(CheckSpecialAttackDistance), // 범위 안?
                                            new ActionNode(SpecialAttack) // 특수 공격

                                        }
                                    )
                                }
                            ),
                            new ActionNode(CheckEnemyWithineAttackRange), // 공격 범위 안?
                            new ActionNode(DoAttack) // 일반 공격
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

    #region SpecialAttack_Node
    private INode.ENodeState CheckCoolTime()
    {
        if (_isCoolTime)
        {
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }

    private INode.ENodeState CheckSpecialAttackDistance()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_actionDistance * _actionDistance))
            {
                return INode.ENodeState.ENS_Success;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    private INode.ENodeState SpecialAttack()
    {
        if (_isCoolTime && _detectedPlayer != null)
        {
            CheckPlayerRay();
            if (hitData.Length > 1 && hitData[1].collider.CompareTag("Player"))
            {
                StartCoroutine(CoolTime());
                StartCoroutine(CrashAttack());
            }

            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Near_Attack Node
    protected override INode.ENodeState CheckAttacking()
    {
        if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME) || isSpecialAttacking)
        {
            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Success;
    }

    protected override INode.ENodeState DoAttack()
    {
        if (_detectedPlayer != null)
        {
            if (_isCoolTime)
            {
                return INode.ENodeState.ENS_Failure;
            }

            _animator.SetTrigger(_ATTACK_ANIM_TIRGGER_NAME);
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region SpecialAttack_Logic
    private IEnumerator CoolTime()
    {
        _isCoolTime = false;

        WaitForFixedUpdate waitFrame = new WaitForFixedUpdate();

        while (coolTime > 0.1f)
        {
            coolTime -= Time.deltaTime;
            yield return waitFrame;
        }

        coolTime = 20f;
        _isCoolTime = true;
    }

    private IEnumerator CrashAttack()
    {
        isSpecialAttacking = true;

        // Crash Ready
        _animator.SetTrigger(_CRASH_ANIM_TRIGGER_NAME);
        yield return new WaitForSeconds(1.5f);

        _rigid.AddForce((_detectedPlayer.position - transform.position) * 5, ForceMode2D.Impulse);

        // Stun
        _animator.SetBool("IsStun", true);
        yield return new WaitForSeconds(3f);
        _animator.SetBool("IsStun", false);

        isSpecialAttacking = false;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSpecialAttacking)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                _rigid.velocity = new Vector2(0, 0);
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                _rigid.velocity = new Vector2(0, 0);
                // 데미지를 주는 로직
                // 넉백 적용
            }
        }
    }
}
