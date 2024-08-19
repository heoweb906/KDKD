using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Button_GameStart : MenuButton
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

        if(mainMenuController.iSelectStageNum == 0) SceneManager.LoadScene("Tutorial");
        else if(mainMenuController.iSelectStageNum == 1) SceneManager.LoadScene("Chapter_1");
        else if(mainMenuController.iSelectStageNum == 2) SceneManager.LoadScene("Chpater_2");


    }

    public override void SelecetButtonOn()
    {
        base.SelecetButtonOn();

        if (iamge_select != null)
        {
            iamge_select.color = new Color(1f, 0.643f, 0f, 1f);
        }
        if (textButton != null)
        {
            textButton.DOColor(new Color(1f, 0.847f, 0.192f, 1f), 0.15f).SetEase(Ease.OutCirc);
        }
    }

    public override void SelecetButtonOff()
    {
        base.SelecetButtonOff();

        if (iamge_select != null)
        {
            iamge_select.color = new Color(1f, 1f, 1f, 1f);
        }
        if (textButton != null)
        {
            textButton.DOColor(new Color(0f, 0f, 0f, 1f), 0.15f).SetEase(Ease.OutCirc);
        }
    }

}
