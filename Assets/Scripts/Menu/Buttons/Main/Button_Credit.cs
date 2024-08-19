using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Button_Credit : MenuButton
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

        Debug.Log("버튼 클릭-제작진");
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
