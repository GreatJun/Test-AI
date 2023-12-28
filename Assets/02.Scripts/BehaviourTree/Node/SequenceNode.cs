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

    public INode.ENodeState Evaluate()
    {
        if (_childs == null || _childs.Count == 0)
            return INode.ENodeState.ENS_Failure;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.ENS_Running:
                    return INode.ENodeState.ENS_Running;
                case INode.ENodeState.ENS_Success:
                    continue;
                case INode.ENodeState.ENS_Failure:
                    return INode.ENodeState.ENS_Failure;
            }
        }

        return INode.ENodeState.ENS_Failure;
    }
}
