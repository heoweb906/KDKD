using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class KeyCapCntAssist : MonoBehaviour
{
    [Header("���� ��� Ű ȹ�� ����")]
    public bool bHaveKey_Keyboard;
    public bool bHaveKey_Plus;
    public bool bHaveKey_Minus;
    public bool bHaveKey_J;


    [Header("�Ҹ� Ű ���� ����")]
    public int iKeyCnt_Z;
    public int iKeyCnt_X;
    public int iKeyCnt_T;


    // #. ���� ����Ϸ��� �ϴ� Űĸ�� �����ϰ� �ִ��� �˻��ϴ� �Լ�
    public bool CheckKeyCapCnt(int funcNum)
    {
        if(funcNum == 0 && bHaveKey_Plus) return true;
        if (funcNum == 1 && bHaveKey_Minus) return true;
        if (funcNum == 2 && bHaveKey_J) return true;

        if (funcNum == 3 && iKeyCnt_Z >= 1)
        {
            iKeyCnt_Z--;
            return true;
        }
        if (funcNum == 4 && iKeyCnt_X >= 1)
        {
            iKeyCnt_X--;
            return true;
        }
        if (funcNum == 5 && iKeyCnt_T >= 1)
        {
            iKeyCnt_T--;
            return true;
        }

        return false;
    }


    // #. Ű ȹ�� ��
    public void KeyCapGet(int funcNum)
    {
        if (funcNum == 999) bHaveKey_Keyboard = true;

        if (funcNum == 0) bHaveKey_Plus = true;
        if (funcNum == 1) bHaveKey_Minus = true;
        if (funcNum == 2) bHaveKey_J = true;

        if (funcNum == 3) iKeyCnt_Z++;
        if (funcNum == 4) iKeyCnt_X++;
        if (funcNum == 5) iKeyCnt_T++;

        return;
    }

}
