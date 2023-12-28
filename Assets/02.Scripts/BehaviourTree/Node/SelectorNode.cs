using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자식 노드 중에서 처음으로 Succes나 Running상태를 가진 노드가 발생하면 그 노드까지 진행하고 멈춘다.
// Evaluate() 메서드의 구현
// 자식 상태 : Running일 때 -> Running 반환
// 자식 상태 : Success일 때 -> Success 반환
// 자식 상태 : Failure 때 -> 다음 자식으로 이동
public sealed class SelectorNode : INode
{
    List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.EnodeState Evaluate()
    {
        if (_childs == null)
            return INode.EnodeState.ENS_Failure;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.EnodeState.ENS_Runnung:
                    return INode.EnodeState.ENS_Runnung;
                case INode.EnodeState.ENS_Success:
                    return INode.EnodeState.ENS_Success;
            }
        }

        return INode.EnodeState.ENS_Failure;
    }
}
