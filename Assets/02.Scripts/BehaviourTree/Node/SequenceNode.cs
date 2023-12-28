using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڽ� ��带 ���ʿ��� ���������� �����ϸ鼭 Failure���°� ���� ������ �����ϰ� �ȴ�.
// Evaluate() �޼ҵ� ����
// �ڽ� ���� : Running�� �� -> Running ��ȯ
// �ڽ� ���� : Success�� �� -> ���� �ڽ����� �̵�
// �ڽ� ���� : Failure�� �� -> Failure ��ȯ

// ������ �� : Running������ ��, �� ���¸� ��� �����ؾ� �ϱ� ������ ���� �ڽ� ���� �̵��ϸ� �� �ǰ�
// ���� ������ ���� �� �ڽĿ� ���� �򰡸� �����ؾ� �Ѵ�.
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
