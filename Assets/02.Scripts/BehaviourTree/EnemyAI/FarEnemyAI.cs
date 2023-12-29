using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarEnemyAI : Enemy
{
    protected override INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackDistance * _attackDistance))
            {
                return INode.ENodeState.ENS_Running;
            }
            // (�����ʿ�) �÷��̾� ��ġ + �����Ÿ� ���������� �̵��ؾ��� (�÷��̾� ��ġ�� �̵��ϴ°� �ƴ϶�)

            transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);

            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Failure;
    }
}
