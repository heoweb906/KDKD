using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public MenuButton nowPlayerButton; // ���� ���õǾ� �ִ� ��ư
    public MenuButton lastButton;
    public MenuButton[] menuButtons;


    public int nowPanelNum;
    [Header("Main Panel")]
    public GameObject Panel_Main;

    [Header("StageSelect Panel")]
    public GameObject Panel_StageSelect;

    [Header("Option Panel")]
    public GameObject Panel_Option;

    // #. �������� ���� 
    public GameObject[] Image_Stages;
    public MenuButton[] buttonsStageSelcet; // �������� ���� ��ư��
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
            // #. �ٸ� �г��� ��쵵 �����ؾ� ��
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

        // Panel�� ��� ���� GameObject���� ������
        Transform[] childTransforms = Panel.GetComponentsInChildren<Transform>(true);

        // MenuButton ��ũ��Ʈ�� ��ӹ��� ������Ʈ���� ã�Ƽ� menuButtons �迭�� �Ҵ�
        List<MenuButton> foundButtons = new List<MenuButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // ���� GameObject���� MenuButton ��ũ��Ʈ�� ��ӹ��� ������Ʈ�� ã��
            MenuButton menuButton = childTransform.GetComponent<MenuButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List�� �迭�� ��ȯ�Ͽ� menuButtons�� �Ҵ�
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
