using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자식 노드를 왼쪽에서 오른쪽으로 진행하면서 Failure상태가 나올 때까지 진행하게 된다.
// Evaluate() 메소드 구현
// 자식 상태 : Running일 때 -> Running 반환
// 자식 상태 : Success일 때 -> 다음 자식으로 이동
// 자식 상태 : Failure일 때 -> Failure 반환

// 주의할 점 : Running상태일 때, 그 상태를 계속 유지해야 하기 때문에 다음 자식 노드로 이동하면 안 되고
// 다음 프레임 때도 그 자식에 대한 평가를 진행해야 한다.
public sealed class SequenceNode : INode
{
    List<INode> _childs;

    public SequenceNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.EnodeState Evaluate()
    {
        if (_childs == null || _childs.Count == 0)
            return INode.EnodeState.ENS_Failure;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.EnodeState.ENS_Runnung:
                    return INode.EnodeState.ENS_Runnung;
                case INode.EnodeState.ENS_Success:
                    continue;
                case INode.EnodeState.ENS_Failure:
                    return INode.EnodeState.ENS_Failure;
            }
        }

        return INode.EnodeState.ENS_Failure;
    }
}
