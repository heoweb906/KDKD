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
        // UI 요소의 월드 좌표를 가져옵니다.
        RectTransform rt = scannerImage.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        Rect uiRect = new Rect(bottomLeft, topRight - bottomLeft);

        // 모든 KeyObject를 포함하여 검사할 리스트를 만듭니다.
        List<KeyObject> allKeyObjects = new List<KeyObject>();

        // "KeyObject" 태그를 가진 모든 오브젝트를 찾습니다.
        GameObject[] keyObjectRoots = GameObject.FindGameObjectsWithTag("KeyObject");

        foreach (GameObject root in keyObjectRoots)
        {
            // 각 루트 오브젝트에서 KeyObject 컴포넌트를 찾습니다.
            KeyObject rootKeyObject = root.GetComponent<KeyObject>();
            if (rootKeyObject != null)
            {
                allKeyObjects.Add(rootKeyObject);
            }

            // 루트 오브젝트의 자식 오브젝트도 검사합니다.
            foreach (Transform child in root.transform)
            {
                KeyObject childKeyObject = child.GetComponent<KeyObject>();
                if (childKeyObject != null)
                {
                    allKeyObjects.Add(childKeyObject);
                }
            }
        }

        KeyObject newClosestKeyObject = null;
        float closestDistance = float.MaxValue;

        foreach (KeyObject objKeyObject in allKeyObjects)
        {
            if (objKeyObject != null)
            {
                Collider objCollider = objKeyObject.GetComponent<Collider>();

                if (objCollider != null)
                {
                    // Collider의 경계와 UI 요소의 Rect가 겹치는지 확인합니다.
                    if (IsColliderIntersectingWithRect(objCollider, uiRect))
                    {
                        Vector3 screenPos = mainCamera.WorldToScreenPoint(objKeyObject.transform.position);
                        float distanceToCenter = Vector3.Distance(screenPos, rt.position);

                        if (distanceToCenter < closestDistance)
                        {
                            closestDistance = distanceToCenter;
                            newClosestKeyObject = objKeyObject;
                        }
                    }
                    else
                    {
                        // UI 영역 밖에 있는 KeyObject는 상호작용 끄기
                        objKeyObject.Off_Interaction();
                    }
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

        // UI 영역 밖에 있는 경우 모든 상호작용 끄기
        if (keyObject != null && !IsColliderIntersectingWithRect(keyObject.GetComponent<Collider>(), uiRect))
        {
            keyObject.Off_Interaction();
            keyObject = null;  // closestKeyObject 초기화
            UpdateKeyCapIcon(keyObject);
        }
    }

    private bool IsColliderIntersectingWithRect(Collider collider, Rect rect)
    {
        // Collider의 Bounds를 가져옵니다.
        Bounds bounds = collider.bounds;

        // Bounds의 8개 꼭짓점을 UI 요소의 화면 좌표로 변환합니다.
        Vector3[] colliderCorners = new Vector3[8];
        colliderCorners[0] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
        colliderCorners[1] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
        colliderCorners[2] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
        colliderCorners[3] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
        colliderCorners[4] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
        colliderCorners[5] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
        colliderCorners[6] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));
        colliderCorners[7] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));

        // Rect와 Bounding Box의 교차 여부를 검사합니다.
        foreach (Vector3 corner in colliderCorners)
        {
            if (rect.Contains(corner))
            {
                return true;
            }
        }

        // Collider의 중심점이 Rect에 포함되는지 확인합니다.
        Vector3 colliderCenter = mainCamera.WorldToScreenPoint(bounds.center);
        if (rect.Contains(colliderCenter))
        {
            return true;
        }

        return false;
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

        switch (funcNum)
        {
            case 0:
            case 1:
                if (!keyObject.bCanSizeControl) return;
                break;
            case 2: 
                if (!keyObject.bCanGrab) return;
                break;

        } 

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
