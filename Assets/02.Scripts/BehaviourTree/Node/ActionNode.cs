using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ � ������ �ϴ� ���.
// Func() ��������Ʈ�� ���� ������ ���� �޾� ����.
public sealed class ActionNode : INode // sealed : �ٸ� Ŭ������ ������� ���ϵ��� �Ѵ�.
{
    Func<INode.EnodeState> _onUpdate = null;    // Func : Atcion�� ��������� �������� ��ȯ���� �ִ� �Լ���� ��.

    public ActionNode(Func<INode.EnodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.EnodeState Evaluate() => _onUpdate?.Invoke() ?? INode.EnodeState.ENS_Failure;
}
