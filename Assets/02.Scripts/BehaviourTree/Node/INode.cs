using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� ���ϼ��� ���ؼ� �������̽� INode ����
// �������̽��� Node�� ���¿� ��尡 � ���������� ��ȯ�ϴ� Evaluate() �޼���
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
