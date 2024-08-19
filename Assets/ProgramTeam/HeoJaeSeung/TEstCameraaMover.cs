using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{

    public float rayLength = 100.0f;
    public float sphereRadius;
    private Vector3 rayOrigin;
    private Vector3 hitPoint;
    private bool isHit;


    // #. ���ۿ��� �ܾ�°�
    public float sesitivity = 500f;
    public float rotationX;
    public float rotationY;


    void Start()
    {
        LockCursor();
    }

    void Update()
    {
       

        //if (Input.GetMouseButtonDown(0))
        //{
        //    FireRayFromMouseClick();
        //}




        MouseRotate();

    }



    void FireRayFromMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            Transform hitTransform = hit.collider.transform;

            if (hitTransform.CompareTag("KeyObject"))
            {
                Transform topParent = hitTransform;

                while (topParent.parent != null)
                {
                    topParent = topParent.parent;
                }
                Debug.Log("����� ������Ʈ �̸� : " + topParent.name);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2.0f);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 2.0f);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 2.0f);
        }
    }



    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Ŀ���� �߾ӿ� ����
        Cursor.visible = false;  // Ŀ���� ����
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;  // Ŀ�� ��� ����
        Cursor.visible = true;  // Ŀ���� ǥ��
    }




    private void MouseRotate()
    {
        float mouseMoveX = Input.GetAxis("Mouse X");
        float mouseMoveY = Input.GetAxis("Mouse Y");

        rotationY += mouseMoveX * sesitivity * Time.deltaTime;
        rotationX += mouseMoveY * sesitivity * Time.deltaTime;

        if (rotationX > 35f)
        {
            rotationX = 35f;
        }

        if (rotationX < -30f)
        {
            rotationX = -30f;
        }

        transform.eulerAngles = new Vector3(-rotationX, rotationY, 0);
    }


}