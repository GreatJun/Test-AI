using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BT�� �����ϱ� ���ؼ� BehaviourTreeRunner clsss ����

public class BehaviourTreeRunner
{
    INode _rootNode;

    public BehaviourTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}
