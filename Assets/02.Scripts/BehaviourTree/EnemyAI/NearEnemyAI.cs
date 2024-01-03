using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class NearEnemyAI : Enemy
{
    Rigidbody2D _rigid;
    Collider2D[] _colliders;

    private float coolTime = 20f;
    private bool isSpecialAttacking = false;
    private Vector2 crashBoxSize = new Vector2(1, 2);


    private const string _CRASH_ANIM_TRIGGER_NAME = "IsCrash";

    public NearEnemyAI() : base()
    {
        this._detectDistance = 4;
        this._attackDistance = 1;
        this._movementSpeed = 3;
        this._isCoolTime = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _rigid = GetComponent<Rigidbody2D>();
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
                            new ActionNode(CheckEnemyWithineAttackRange), // 공격 범위 안?
                            new SelectorNode
                            (
                                new List<INode> ()
                                {
                                    new ActionNode(DoAttack),
                                    new ActionNode(SpecialAttack)
                                }
                            )
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

    private INode.ENodeState SpecialAttack()
    {
        // 특수 공격 로직
        // 코루틴 스타트
        if (_isCoolTime && _detectedPlayer != null)
        {
            StartCoroutine(CoolTime());
            StartCoroutine(CrashAttack());

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
                Debug.Log("접근");
                // 데미지를 주는 로직
                // 넉백 적용
            }
        }
    }
}
