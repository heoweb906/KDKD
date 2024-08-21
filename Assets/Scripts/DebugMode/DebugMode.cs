using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DebugMode : MonoBehaviour
{

    [SerializeField] private KeyObject keyObject;
    [SerializeField] private List<KeyObject> keyObjectArr = new List<KeyObject>();

    [Header("스크립트 관련")]
    public KeyCapCntAssist keycapCntAssist;
    public bool bDebugMode;

    [Header("UI Canvas 관련")]
    public Image scannerImage;
    public Camera mainCamera;
    public GameObject DebugModeImage;
    public GameObject keycapIconLayOut;
    public GameObject[] keycapIcons;

    public int iTestNum;


    // #.KeyObject 감지 함수
    public void CheckKeyObject()
    {
        RectTransform rt = scannerImage.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        Rect screenRect = new Rect(bottomLeft, topRight - bottomLeft);

        GameObject[] keyObjects = GameObject.FindGameObjectsWithTag("KeyObject");

        KeyObject newClosestKeyObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in keyObjects)
        {
            KeyObject objKeyObject = obj.GetComponent<KeyObject>();
            if (objKeyObject != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(obj.transform.position);

                if (screenRect.Contains(screenPos))
                {
                    // 이미지 안에 있는 KeyObject 찾기
                    float distanceToCenter = Vector3.Distance(screenPos, rt.position);

                    if (distanceToCenter < closestDistance)
                    {
                        closestDistance = distanceToCenter;
                        newClosestKeyObject = objKeyObject;
                    }
                }
                else
                {
                    // 이미지 밖에 있는 KeyObject는 상호작용 끄기
                    objKeyObject.Off_Interaction();
                    UpdateKeyCapIcon(keyObject);
                }
            }
        }

        // 가장 가까운 KeyObject만 활성화하기
        if (newClosestKeyObject != keyObject)
        {
            // 기존의 closestKeyObject의 상호작용 끄기
            if (keyObject != null)
            {
                keyObject.Off_Interaction();
            }

            // 새로운 closestKeyObject의 상호작용 켜기
            keyObject = newClosestKeyObject;
            if (keyObject != null)
            {
                keyObject.On_Interaction(mainCamera.transform.forward);
                UpdateKeyCapIcon(keyObject);
                // Debug.Log("KeyObject detected: " + keyObject.gameObject.name);
            }
        }

        // 이미지 밖에 있는 경우 모든 상호작용 끄기
        if (keyObject != null && !screenRect.Contains(mainCamera.WorldToScreenPoint(keyObject.transform.position)))
        {
            keyObject.Off_Interaction();
            keyObject = null;  // closestKeyObject 초기화
            UpdateKeyCapIcon(keyObject);
        }
    }
    public void CheckKeyObjectOff()
    {
        if (keyObject != null)
        {
            keyObject.Off_Interaction();
            keyObject = null;
        }
    }


    // #.KeyObject 함수 추가 및 제거
    private void InputKeyCapCode()
    {
        if (keyObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Equals)) PlusKeyObjcetFunc(keyObject, 0);
            if (Input.GetKeyDown(KeyCode.Minus)) PlusKeyObjcetFunc(keyObject, 1);
            if (Input.GetKeyDown(KeyCode.J)) PlusKeyObjcetFunc(keyObject, 2);
            if (Input.GetKeyDown(KeyCode.Z)) PlusKeyObjcetFunc(keyObject, 3);
            if (Input.GetKeyDown(KeyCode.T)) PlusKeyObjcetFunc(keyObject, 4);
            if (Input.GetKeyDown(KeyCode.X)) PlusKeyObjcetFunc(keyObject, 5);

            if (Input.GetMouseButtonDown(1))
            {
                if (keyObject.keycapFuncNum.Count <= 0) return;

                iTestNum = keyObject.MinusKeyCapFunc();
                keycapCntAssist.KeyCapGet(iTestNum);

                UpdateKeyCapIcon(keyObject);

                if (keyObject.keycapFuncNum.Count <= 1 && keyObjectArr.Contains(keyObject)) keyObjectArr.Remove(keyObject);
            }
        }
    }
    // #. Key 오브젝트에 실행할 함수를 추가하는 함수
    private void PlusKeyObjcetFunc(KeyObject keyObj, int funcNum)
    {
        if (keyObject.keycapFuncNum.Count >= 3) return; // 이미 3개 이상의 함수를 담고 있다면
        if (!(keycapCntAssist.CheckKeyCapCnt(funcNum))) return; // 만약 키를 소지하고 있지 않다면

        keyObj.PlusKeyCapFunc(funcNum);
        keyObj.positionPlayer = transform.position;
        UpdateKeyCapIcon(keyObject);
        if (!keyObjectArr.Contains(keyObj)) keyObjectArr.Add(keyObj);
    }



    public void DebugFuncStart()
    {
        foreach (KeyObject keyObject in keyObjectArr)
        {
            keyObject.ImplementKeyCapFunc();
        }

        keyObjectArr.Clear();

        if (keyObject != null)
        {
            keyObject.Off_Interaction();
            keyObject = null;
        }
    }



    public void AddDebugFunc()
    {
        SetAimImage(true);
        InputManager.instance.keyaction += CheckKeyObject;
        InputManager.instance.keyaction += InputKeyCapCode;
    }
    public void DeleteDebugFunc()
    {
        InputManager.instance.keyaction -= CheckKeyObject;
        InputManager.instance.keyaction -= InputKeyCapCode;
        StartCoroutine(C_DeleteDebugFunc());
    }

    private IEnumerator C_DeleteDebugFunc()
    {
        yield return null;
        DebugFuncStart();
        CheckKeyObjectOff();
        SetAimImage(false);
    }





    #region // 디버그 모드 UI 관련

    // #. 디버그 모드 Aim Image 활성화여부
    public void SetAimImage(bool _bool)
    {
        DebugModeImage.SetActive(_bool);
    }


    // #. 키캡 함수 아이콘 활성화
    private void UpdateKeyCapIcon(KeyObject keyObj = null)
    {
        // 기존 자식들 제거
        foreach (Transform child in keycapIconLayOut.transform)
        {
            Destroy(child.gameObject);
        }

        // KeyObject가 null이 아닌 경우에만 새로운 아이콘 생성 및 추가
        if (keyObj != null)
        {
            foreach (int i in keyObj.keycapFuncNum)
            {
                GameObject newIcon = Instantiate(keycapIcons[i], keycapIconLayOut.transform);
                newIcon.transform.localPosition = Vector3.zero;
                newIcon.transform.localScale = Vector3.one;
            }
        }
    }


    #endregion
}
