using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڽ� ��� �߿��� ó������ Succes�� Running���¸� ���� ��尡 �߻��ϸ� �� ������ �����ϰ� �����.
// Evaluate() �޼����� ����
// �ڽ� ���� : Running�� �� -> Running ��ȯ
// �ڽ� ���� : Success�� �� -> Success ��ȯ
// �ڽ� ���� : Failure �� -> ���� �ڽ����� �̵�
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
