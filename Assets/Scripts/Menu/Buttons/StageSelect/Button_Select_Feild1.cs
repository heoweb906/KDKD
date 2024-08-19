using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Button_Select_Feild1 : MenuButton
{
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

        StageSelect();
    }


    public override void SelecetButtonOn()
    {
        base.SelecetButtonOn();

        if (!bSelect)
        {
            if (iamge_select != null)
            {
                iamge_select.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            if (textButton != null)
            {
                textButton.DOColor(new Color(0f, 0f, 0f, 1f), 0.02f).SetEase(Ease.OutCirc);
            }
        }
    }

    public override void SelecetButtonOff()
    {
        base.SelecetButtonOff();

        if (!bSelect)
        {
            if (iamge_select != null)
            {
                iamge_select.color = new Color(1f, 1f, 1f, 0f);
            }
            if (textButton != null)
            {
                textButton.DOColor(new Color(1f, 1f, 1f, 1f), 0.02f).SetEase(Ease.OutCirc);
            }
        }
    }

    public void StageSelect()
    {
        mainMenuController.StageIconupdate(1);

        bSelect = true;

        if (iamge_select != null)
        {
            iamge_select.color = new Color(1f, 1f, 1f, 1f);
        }
        if (textButton != null)
        {
            textButton.DOColor(new Color(0f, 0f, 0f, 1f), 0.02f).SetEase(Ease.OutCirc);
        }
    }

}
