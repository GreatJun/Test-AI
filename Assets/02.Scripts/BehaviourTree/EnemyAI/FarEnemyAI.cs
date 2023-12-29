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
            // (수정필요) 플레이어 위치 + 사정거리 포지션으로 이동해야함 (플레이어 위치로 이동하는게 아니라)

            transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);

            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Failure;
    }
}
