using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 실제로 어떤 행위를 하는 노드.
// Func() 델리게이트를 통해 행위를 전달 받아 실행.
public sealed class ActionNode : INode // sealed : 다른 클래스에 상속하지 못하도록 한다.
{
    Func<INode.ENodeState> _onUpdate = null;    // Func : Atcion과 비슷하지만 차이점이 반환값이 있는 함수라는 것.

    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.ENodeState Evaluate() => _onUpdate?.Invoke() ?? INode.ENodeState.ENS_Failure; // ?? : 완쪽 값이 Null이라면 오른쪽 값으로 처리
}
