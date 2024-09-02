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

    [Header("��ũ��Ʈ ����")]
    public KeyCapCntAssist keycapCntAssist;
    public bool bDebugMode;

    [Header("UI Canvas ����")]
    public Image scannerImage;
    public Camera mainCamera;
    public GameObject DebugModeImage;
    public GameObject keycapIconLayOut;
    public GameObject[] keycapIcons;

    public int iTestNum;


    // #.KeyObject ���� �Լ�
    public void CheckKeyObject()
    {
        // UI ����� ���� ��ǥ�� �����ɴϴ�.
        RectTransform rt = scannerImage.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        Rect uiRect = new Rect(bottomLeft, topRight - bottomLeft);

        // ��� KeyObject�� �����Ͽ� �˻��� ����Ʈ�� ����ϴ�.
        List<KeyObject> allKeyObjects = new List<KeyObject>();

        // "KeyObject" �±׸� ���� ��� ������Ʈ�� ã���ϴ�.
        GameObject[] keyObjectRoots = GameObject.FindGameObjectsWithTag("KeyObject");

        foreach (GameObject root in keyObjectRoots)
        {
            // �� ��Ʈ ������Ʈ���� KeyObject ������Ʈ�� ã���ϴ�.
            KeyObject rootKeyObject = root.GetComponent<KeyObject>();
            if (rootKeyObject != null)
            {
                allKeyObjects.Add(rootKeyObject);
            }

            // ��Ʈ ������Ʈ�� �ڽ� ������Ʈ�� �˻��մϴ�.
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
                    // Collider�� ���� UI ����� Rect�� ��ġ���� Ȯ���մϴ�.
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
                        // UI ���� �ۿ� �ִ� KeyObject�� ��ȣ�ۿ� ����
                        objKeyObject.Off_Interaction();
                    }
                }
            }
        }

        // ���� ����� KeyObject�� Ȱ��ȭ�ϱ�
        if (newClosestKeyObject != keyObject)
        {
            // ������ closestKeyObject�� ��ȣ�ۿ� ����
            if (keyObject != null)
            {
                keyObject.Off_Interaction();
            }

            // ���ο� closestKeyObject�� ��ȣ�ۿ� �ѱ�
            keyObject = newClosestKeyObject;
            if (keyObject != null)
            {
                keyObject.On_Interaction(mainCamera.transform.forward);
                UpdateKeyCapIcon(keyObject);
                // Debug.Log("KeyObject detected: " + keyObject.gameObject.name);
            }
        }

        // UI ���� �ۿ� �ִ� ��� ��� ��ȣ�ۿ� ����
        if (keyObject != null && !IsColliderIntersectingWithRect(keyObject.GetComponent<Collider>(), uiRect))
        {
            keyObject.Off_Interaction();
            keyObject = null;  // closestKeyObject �ʱ�ȭ
            UpdateKeyCapIcon(keyObject);
        }
    }

    private bool IsColliderIntersectingWithRect(Collider collider, Rect rect)
    {
        // Collider�� Bounds�� �����ɴϴ�.
        Bounds bounds = collider.bounds;

        // Bounds�� 8�� �������� UI ����� ȭ�� ��ǥ�� ��ȯ�մϴ�.
        Vector3[] colliderCorners = new Vector3[8];
        colliderCorners[0] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
        colliderCorners[1] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
        colliderCorners[2] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
        colliderCorners[3] = mainCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
        colliderCorners[4] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
        colliderCorners[5] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
        colliderCorners[6] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));
        colliderCorners[7] = mainCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));

        // Rect�� Bounding Box�� ���� ���θ� �˻��մϴ�.
        foreach (Vector3 corner in colliderCorners)
        {
            if (rect.Contains(corner))
            {
                return true;
            }
        }

        // Collider�� �߽����� Rect�� ���ԵǴ��� Ȯ���մϴ�.
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


    // #.KeyObject �Լ� �߰� �� ����
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
    // #. Key ������Ʈ�� ������ �Լ��� �߰��ϴ� �Լ�
    private void PlusKeyObjcetFunc(KeyObject keyObj, int funcNum)
    {
        if (keyObject.keycapFuncNum.Count >= 3) return; // �̹� 3�� �̻��� �Լ��� ��� �ִٸ�
        if (!(keycapCntAssist.CheckKeyCapCnt(funcNum))) return; // ���� Ű�� �����ϰ� ���� �ʴٸ�

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





    #region // ����� ��� UI ����

    // #. ����� ��� Aim Image Ȱ��ȭ����
    public void SetAimImage(bool _bool)
    {
        DebugModeImage.SetActive(_bool);
    }


    // #. Űĸ �Լ� ������ Ȱ��ȭ
    private void UpdateKeyCapIcon(KeyObject keyObj = null)
    {
        // ���� �ڽĵ� ����
        foreach (Transform child in keycapIconLayOut.transform)
        {
            Destroy(child.gameObject);
        }

        // KeyObject�� null�� �ƴ� ��쿡�� ���ο� ������ ���� �� �߰�
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
