using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public Transform cameraTransform;

    public CharacterController characterController;

    public float moveSpeed = 10f;

    public float jumpSpeed = 10f;

    public float gravity = -20f;

    public float yVelocity = 0;





    [Header("디버그 모드 관련")]
    public DebugMode debugMode;
    public GameObject DebugMode;
    public GameObject DebugModeImage;
    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            debugMode.bDebugMode = !debugMode.bDebugMode;

            if(debugMode.bDebugMode)
            {
                DebugMode.SetActive(true);
                DebugModeImage.SetActive(true);
                Debug.Log("디버그 모드 ON");
            }
            else
            {
                Debug.Log("디버그 모드 OFF");
                debugMode.DebugFuncStart();
                debugMode.CheckKeyObjectOff();
                DebugMode.SetActive(false);
                DebugModeImage.SetActive(false);
            }
        }


        Vector3 moveDirection = new Vector3(h, 0, v);
      
        moveDirection = cameraTransform.TransformDirection(moveDirection);

        moveDirection *= moveSpeed;

        if (characterController.isGrounded)
        {
            yVelocity = 0;
         
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = jumpSpeed;
                
            }
        }

        yVelocity += (gravity * Time.deltaTime);
    
        moveDirection.y = yVelocity;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}