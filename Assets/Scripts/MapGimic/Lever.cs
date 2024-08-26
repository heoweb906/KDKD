using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : KeyObject
{
    public InteractiveObject interObj;
    public ObjectTrigger[] objTrigger;
    public bool bSwitch = false;

    void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.F) && !bSwitch && interObj.isPlayerInRange) 
        { 
            if(iCntSizeControl == 1) 
            { 
                ObjStart(); 
                bSwitch = true; 

                Debug.Log("�۵�"); 
                interObj.UiIconOff(); 
            } 
        } 
    } 

    private void ObjStart()
    {
        foreach (var obj in objTrigger) 
        {
            obj.ActivaObj();
        }
    }


    override public void KeyCapFunc_Bigger()
    {
        Debug.Log("Ŀ����");
        gameObject.transform.DOScale(gameObject.transform.localScale * 10f, 0.15f).SetEase(Ease.InOutBack);

        iCntSizeControl++;

        InteractionEffectupdate();
    }


    // #. "-" ��ư - �۾�����
    override public void KeyCapFunc_Smaller()
    {
        Debug.Log("�۾�����");
        gameObject.transform.DOScale(gameObject.transform.localScale * 0.1f, 0.15f).SetEase(Ease.InOutBack);

        iCntSizeControl--;

        InteractionEffectupdate();
    }
}
