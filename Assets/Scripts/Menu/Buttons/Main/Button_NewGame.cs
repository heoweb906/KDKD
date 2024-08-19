using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button_NewGame : MenuButton
{
    public GameObject Panel_Main;
    public GameObject Panel_StageSelect;

    public override void OnPointerEnter(PointerEventData eventData) 
    { 
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData) 
    {
        base.OnPointerExit(eventData);
    }

    public override void ImplementButton() 
    {
        base.ImplementButton();

        Panel_Main.SetActive(false);
        Panel_StageSelect.SetActive(true);

        mainMenuController.FindMenuButtons(1);
        mainMenuController.StageIconClear();
    }

    public override void SelecetButtonOn()
    {
        base.SelecetButtonOn();

        if (iamge_select != null)
        {
            iamge_select.color = new Color(1f, 1f, 1f, 1f);
        }
        if (textButton != null)
        {
            textButton.DOFontSize(85f, 0.15f).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(0f, 0f, 0f, 1f), 0.15f).SetEase(Ease.OutCirc);
        }
    }

    public override void SelecetButtonOff()
    {
        base.SelecetButtonOff();

        if (iamge_select != null)
        {
            iamge_select.color = new Color(1f, 1f, 1f, 0f);
        }
        if (textButton != null)
        {
            textButton.DOFontSize(70f, 0.15f).SetEase(Ease.OutCirc);
            textButton.DOColor(new Color(1f, 1f, 1f, 1f), 0.15f).SetEase(Ease.OutCirc);
        }
    }


}
