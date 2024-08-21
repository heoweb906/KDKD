using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Button_Option : MenuButton
{
    public GameObject Panel_Main;
    public GameObject Panel_Option;

    public Volume globalVolume;
    private DepthOfField dof;
    private ColorAdjustments colorAdjustments;

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
        Panel_Option.SetActive(true);




        MenuUIBlurOn();

        mainMenuController.FindMenuButtons(2);
        Debug.Log("버튼 클릭-옵션창");
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



    private void MenuUIBlurOn()
    {
        if (globalVolume.profile.TryGet(out dof) && globalVolume.profile.TryGet(out colorAdjustments))
        {
            DOTween.KillAll(); 

            dof.active = true;
            DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 150f, 0.4f)
                   .SetEase(Ease.OutQuint);

            // Color Adjustments를 활성화하고, colorFilter를 부드럽게 변경
            colorAdjustments.active = true;
            DOTween.To(() => colorAdjustments.colorFilter.value, x => colorAdjustments.colorFilter.value = x, new Color(177f / 255f, 177f / 255f, 177f / 255f), 0.4f)
                   .SetEase(Ease.OutQuint);
  
        }
    }
}
