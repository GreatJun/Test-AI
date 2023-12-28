using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BT를 실행하기 위해서 BehaviourTreeRunner clsss 구현

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
