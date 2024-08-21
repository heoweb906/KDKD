using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public MenuButton nowPlayerButton; // 현재 선택되어 있는 버튼
    public MenuButton lastButton;
    public MenuButton[] menuButtons;


    public int nowPanelNum;
    [Header("Main Panel")]
    public GameObject Panel_Main;

    [Header("StageSelect Panel")]
    public GameObject Panel_StageSelect;

    [Header("Option Panel")]
    public GameObject Panel_Option;

    // #. 스테이지 관련 
    public GameObject[] Image_Stages;
    public MenuButton[] buttonsStageSelcet; // 스테이지 선택 버튼들
    public int iSelectStageNum = 0;


    private void Awake()
    {
        FindMenuButtons(0);
    }
    private void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (nowPlayerButton != null) nowPlayerButton.ImplementButton();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FindClosestButton(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FindClosestButton(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FindClosestButton(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindClosestButton(Vector2.right);
        }
    
        if(Input.GetKeyDown(KeyCode.Escape) && nowPanelNum != 0)
        {
            // #. 다른 패널일 경우도 수정해야 함
            if (nowPanelNum == 1) Panel_StageSelect.SetActive(false);
    
            Panel_Main.SetActive(true);
            FindMenuButtons(0);

        }
    }

    private void FindClosestButton(Vector2 direction)
    {
        if (nowPlayerButton == null)
        {
            nowPlayerButton = lastButton;
            nowPlayerButton.SelecetButtonOn();
            return;
        }

        float closestDistance = Mathf.Infinity;
        MenuButton closestButton = null;
        Vector2 currentPosition = nowPlayerButton.transform.position;

        foreach (MenuButton button in menuButtons)
        {
            if (button == nowPlayerButton) continue;

            Vector2 directionToButton = (Vector2)button.transform.position - currentPosition;
            if (Vector2.Dot(directionToButton.normalized, direction) > 0.5f)
            {
                float distance = directionToButton.magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestButton = button;
                }
            }
        }

        if (closestButton != null)
        {
            nowPlayerButton.SelecetButtonOff();
            nowPlayerButton = closestButton;
            nowPlayerButton.SelecetButtonOn();
        }
    }

    public void FindMenuButtons(int index)
    {
        nowPlayerButton = null;

        GameObject Panel = null;

        switch (index)
        {
            case 0:
                Panel = Panel_Main;
                break;
            case 1:
                Panel = Panel_StageSelect;
                break;
            case 2:
                Panel = Panel_Option;
                break;
            default:
                break;
        }

        nowPanelNum = index;

        // Panel의 모든 하위 GameObject들을 가져옴
        Transform[] childTransforms = Panel.GetComponentsInChildren<Transform>(true);

        // MenuButton 스크립트를 상속받은 컴포넌트들을 찾아서 menuButtons 배열에 할당
        List<MenuButton> foundButtons = new List<MenuButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // 하위 GameObject에서 MenuButton 스크립트를 상속받은 컴포넌트를 찾음
            MenuButton menuButton = childTransform.GetComponent<MenuButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List를 배열로 변환하여 menuButtons에 할당
        menuButtons = foundButtons.ToArray();
        lastButton = menuButtons[0];
    }


    public void StageIconupdate(int index)
    {
        if (index < 0 || index >= Image_Stages.Length) return;

        foreach (GameObject stage in Image_Stages) stage.SetActive(false);

        for(int i = 0; i< buttonsStageSelcet.Length; i++)
        {
            buttonsStageSelcet[i].bSelect = false;
            buttonsStageSelcet[i].SelecetButtonOff();
        }

        iSelectStageNum = index;
        Image_Stages[index].SetActive(true);
    }

    public void StageIconClear()
    {
        foreach (GameObject stage in Image_Stages) stage.SetActive(false);

        for (int i = 0; i < buttonsStageSelcet.Length; i++)
        {
            buttonsStageSelcet[i].bSelect = false;
            buttonsStageSelcet[i].SelecetButtonOff();
        }

        iSelectStageNum = -1;
    }

}
