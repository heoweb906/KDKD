using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Canvas canvas;               // UI 아이콘이 표시될 캔버스
    public Transform position_UiIcon;   // 아이콘이 위치할 월드 좌표
    public Image image_UiIconPrefab;    // UI 아이콘 프리팹

    private Image uiIconInstance;       // 생성된 UI 아이콘 인스턴스
    public bool isPlayerInRange = false;

    void Update()
    {
        // 플레이어가 범위 안에 있을 때만 UI 아이콘의 위치를 업데이트
        if (isPlayerInRange && uiIconInstance != null)
        {
            // 월드 좌표를 화면 좌표로 변환
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position_UiIcon.position);
            uiIconInstance.transform.position = screenPosition;
        }
    }


    // 플레이어가 콜라이더에 닿았을 때 
    public void UiIconOn()
    {
        isPlayerInRange = true;

        // 이미 아이콘이 존재하지 않는 경우에만 생성
        if (uiIconInstance == null)
        {
            // UI 아이콘 생성
            uiIconInstance = Instantiate(image_UiIconPrefab, canvas.transform);

            // 아이콘의 초기 위치 설정
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(position_UiIcon.position);
            uiIconInstance.transform.position = screenPosition;
        }
    }

    // 플레이어가 콜라이더에서 나갔을 때 호출
    public void UiIconOff()
    {
        isPlayerInRange = false;

        // UI 아이콘 제거
        if (uiIconInstance != null)
        {
            Destroy(uiIconInstance.gameObject);
            uiIconInstance = null;  // 참조 초기화
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            UiIconOn();        // UI 아이콘을 활성화합니다.
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            UiIconOff();        // UI 아이콘을 활성화합니다.
        }
    }


}
