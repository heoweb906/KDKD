using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Canvas canvas;               // UI �������� ǥ�õ� ĵ����
    public Transform position_UiIcon;   // �������� ��ġ�� ���� ��ǥ
    public Image image_UiIconPrefab;    // UI ������ ������

    private Image uiIconInstance;       // ������ UI ������ �ν��Ͻ�
    public bool isPlayerInRange = false;

    void Update()
    {
        // �÷��̾ ���� �ȿ� ���� ���� UI �������� ��ġ�� ������Ʈ
        if (isPlayerInRange && uiIconInstance != null)
        {
            // ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position_UiIcon.position);
            uiIconInstance.transform.position = screenPosition;
        }
    }


    // �÷��̾ �ݶ��̴��� ����� �� 
    public void UiIconOn()
    {
        isPlayerInRange = true;

        // �̹� �������� �������� �ʴ� ��쿡�� ����
        if (uiIconInstance == null)
        {
            // UI ������ ����
            uiIconInstance = Instantiate(image_UiIconPrefab, canvas.transform);

            // �������� �ʱ� ��ġ ����
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position_UiIcon.position);
            uiIconInstance.transform.position = screenPosition;
        }
    }

    // �÷��̾ �ݶ��̴����� ������ �� ȣ��
    public void UiIconOff()
    {
        isPlayerInRange = false;

        // UI ������ ����
        if (uiIconInstance != null)
        {
            Destroy(uiIconInstance.gameObject);
            uiIconInstance = null;  // ���� �ʱ�ȭ
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            UiIconOn();        // UI �������� Ȱ��ȭ�մϴ�.
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            UiIconOff();        // UI �������� Ȱ��ȭ�մϴ�.
        }
    }


}
