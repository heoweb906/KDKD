using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Button_OptionBack : MenuButton
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

        MenuUIBlurOff();

        Panel_Main.SetActive(true);
        Panel_Option.SetActive(false);

       

        mainMenuController.FindMenuButtons(0);
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

    private void MenuUIBlurOff()
    {
        if (globalVolume.profile.TryGet(out dof) && globalVolume.profile.TryGet(out colorAdjustments))
        {
            DOTween.KillAll();

            // DepthOfField�� Ȱ��ȭ�ϰ�, focalLength ���� �ε巴�� ����
            DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 0f, 0.4f)
                   .SetEase(Ease.OutQuint)
                   .OnComplete(() => dof.active = false); // �ִϸ��̼� �Ϸ� �� DepthOfField ��Ȱ��ȭ

            // Color Adjustments�� Ȱ��ȭ�ϰ�, colorFilter�� �ε巴�� ����
            DOTween.To(() => colorAdjustments.colorFilter.value, x => colorAdjustments.colorFilter.value = x, new Color(1f, 1f, 1f), 0.4f)
                   .SetEase(Ease.OutQuint)
                   .OnComplete(() => colorAdjustments.active = false); // �ִϸ��̼� �Ϸ� �� Color Adjustments ��Ȱ��ȭ
        }
    }
}
