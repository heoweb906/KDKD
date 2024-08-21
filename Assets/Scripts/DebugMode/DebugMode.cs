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
                    // �̹��� �ȿ� �ִ� KeyObject ã��
                    float distanceToCenter = Vector3.Distance(screenPos, rt.position);

                    if (distanceToCenter < closestDistance)
                    {
                        closestDistance = distanceToCenter;
                        newClosestKeyObject = objKeyObject;
                    }
                }
                else
                {
                    // �̹��� �ۿ� �ִ� KeyObject�� ��ȣ�ۿ� ����
                    objKeyObject.Off_Interaction();
                    UpdateKeyCapIcon(keyObject);
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

        // �̹��� �ۿ� �ִ� ��� ��� ��ȣ�ۿ� ����
        if (keyObject != null && !screenRect.Contains(mainCamera.WorldToScreenPoint(keyObject.transform.position)))
        {
            keyObject.Off_Interaction();
            keyObject = null;  // closestKeyObject �ʱ�ȭ
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
