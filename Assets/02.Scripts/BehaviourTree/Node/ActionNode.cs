using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ � ������ �ϴ� ���.
// Func() ��������Ʈ�� ���� ������ ���� �޾� ����.
public sealed class ActionNode : INode // sealed : �ٸ� Ŭ������ ������� ���ϵ��� �Ѵ�.
{
    Func<INode.ENodeState> _onUpdate = null;    // Func : Atcion�� ��������� �������� ��ȯ���� �ִ� �Լ���� ��.

    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.ENodeState Evaluate() => _onUpdate?.Invoke() ?? INode.ENodeState.ENS_Failure; // ?? : ���� ���� Null�̶�� ������ ������ ó��
}
