using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노드의 통일성을 위해서 인터페이스 INode 생성
// 인터페이스의 Node의 상태와 노드가 어떤 상태인지를 반환하는 Evaluate() 메서드
public interface INode
{
    public enum EnodeState
    {
        ENS_Runnung,
        ENS_Success,
        ENS_Failure
    }

    public EnodeState Evaluate();
}
