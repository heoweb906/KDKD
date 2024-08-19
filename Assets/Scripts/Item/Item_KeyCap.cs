using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_KeyCap : MonoBehaviour
{
    public GameObject model;
    public int iKeyCapNum;

    public KeyCapCntAssist GetKeyCapCntAssist()
    {
        // 현재 객체에서 KeyCapCntAssist 컴포넌트를 찾음
        KeyCapCntAssist keyCapCntAssist = GetComponent<KeyCapCntAssist>();
        if (keyCapCntAssist != null)
        {
            return keyCapCntAssist;
        }

        // 자식 객체들에서 KeyCapCntAssist 컴포넌트를 찾음
        keyCapCntAssist = GetComponentInChildren<KeyCapCntAssist>();
        if (keyCapCntAssist != null)
        {
            return keyCapCntAssist;
        }

        // 부모 객체들에서 KeyCapCntAssist 컴포넌트를 찾음
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

        // KeyCapCntAssist 컴포넌트를 찾지 못한 경우 null 반환
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체의 태그가 "Player"인지 검사
        if (other.CompareTag("Player"))
        {
            // 충돌한 오브젝트의 하위 오브젝트에서 KeyCapCntAssist 컴포넌트를 찾음
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
