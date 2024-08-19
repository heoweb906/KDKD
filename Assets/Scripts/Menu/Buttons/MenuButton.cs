using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected MainMenuController mainMenuController;
    public Image iamge_select;
    public TMP_Text textButton;

    public bool bSelect = false;

    private void Awake() 
    {
        mainMenuController = FindObjectOfType<MainMenuController>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData) 
    {
        if (mainMenuController.nowPlayerButton != null)
        {
            mainMenuController.nowPlayerButton.SelecetButtonOff();
        }
        SelecetButtonOn();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        SelecetButtonOff();
    }

    public virtual void ImplementButton() 
    {
        SelecetButtonOff();
        // mainMenuController.nowPlayerButton = null;
    }
  
    public virtual void SelecetButtonOn()
    {
        mainMenuController.nowPlayerButton = this;
    }

    public virtual void SelecetButtonOff()
    {
        mainMenuController.lastButton = this;
    }



}
