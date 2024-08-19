using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_KeyCap : MonoBehaviour
{
    public GameObject model;
    public int iKeyCapNum;

    public KeyCapCntAssist GetKeyCapCntAssist()
    {
        // ���� ��ü���� KeyCapCntAssist ������Ʈ�� ã��
        KeyCapCntAssist keyCapCntAssist = GetComponent<KeyCapCntAssist>();
        if (keyCapCntAssist != null)
        {
            return keyCapCntAssist;
        }

        // �ڽ� ��ü�鿡�� KeyCapCntAssist ������Ʈ�� ã��
        keyCapCntAssist = GetComponentInChildren<KeyCapCntAssist>();
        if (keyCapCntAssist != null)
        {
            return keyCapCntAssist;
        }

        // �θ� ��ü�鿡�� KeyCapCntAssist ������Ʈ�� ã��
        Transform parentTransform = transform.parent;
        while (parentTransform != null)
        {
            keyCapCntAssist = parentTransform.GetComponent<KeyCapCntAssist>();
            if (keyCapCntAssist != null)
            {
                return keyCapCntAssist;
            }
            parentTransform = parentTransform.parent;
        }

        // KeyCapCntAssist ������Ʈ�� ã�� ���� ��� null ��ȯ
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �±װ� "Player"���� �˻�
        if (other.CompareTag("Player"))
        {
            // �浹�� ������Ʈ�� ���� ������Ʈ���� KeyCapCntAssist ������Ʈ�� ã��
            KeyCapCntAssist keyCapCntAssist = other.GetComponentInChildren<KeyCapCntAssist>();
            if (keyCapCntAssist != null)
            {
                keyCapCntAssist.KeyCapGet(iKeyCapNum);
                Destroy(model);
            }
            else
            {
                Debug.Log("KeyCapCntAssist not found in Player's child object");
            }
        }
    }


}
