using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [Header("�ó׸ӽ� �극��")]
    [SerializeField]
    private CinemachineBrain cam_brain;

    [Header("�̵� ��� ī�޶�")]
    [SerializeField]
    private CinemachineFreeLook cam_MoveView;

    [Header("����׸�� ī�޶�")]
    [SerializeField]
    private CinemachineFreeLook cam_DebugView;
    [SerializeField]
    private Vector3 debugCamPosition;
    [SerializeField]
    private Vector3 debugCamRotation;
    [SerializeField]
    public float debugCamSensitivity;

    private Vector3 debugStartRotation;

    private float debugCamPreXAxis = 0;

    // �̵� ķ���� ��ȯ
    public void SetMoveCam()
    {
        cam_DebugView.gameObject.SetActive(false);
        cam_MoveView.gameObject.SetActive(true);

        cam_MoveView.m_XAxis.Value = 0;
        cam_MoveView.m_YAxis.Value = cam_DebugView.m_YAxis.Value * 0.8f;
        InputManager.instance.keyaction -= DebugCamMove;
    }

    //����� ķ���� ��ȯ
    public void SetDebugCam()
    {
        playerMovement.gameObject.transform.rotation
            = Quaternion.Euler(0,
            cam_MoveView.m_XAxis.Value + debugCamPreXAxis, 0);

        debugCamPreXAxis = cam_MoveView.m_XAxis.Value + debugCamPreXAxis;
        cam_MoveView.gameObject.SetActive(false);
        cam_DebugView.gameObject.SetActive(true);

        cam_DebugView.m_XAxis.Value = -20;

        float debugView_YAxis = 0;
        if(cam_MoveView.m_YAxis.Value < 0.7f)
        {
            debugView_YAxis = cam_MoveView.m_YAxis.Value + (0.7f - cam_MoveView.m_YAxis.Value) * 0.5f;
        }
        else
        {
            debugView_YAxis = cam_MoveView.m_YAxis.Value;
        }

        cam_DebugView.m_YAxis.Value = debugView_YAxis;
        debugStartRotation = playerMovement.gameObject.transform.rotation.eulerAngles;
        InputManager.instance.keyaction += DebugCamMove;
    }

    //����� ķ �����϶� �÷��̾� �����̼�
    private void DebugCamMove()
    {
        playerMovement.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, debugStartRotation.y + cam_DebugView.m_XAxis.Value, 0));
    }

    //ķ ���� �ӵ� ����
    public void SetCamBlendSpeed(float _time)
    {
        cam_brain.m_DefaultBlend.m_Time = _time;
    }

    public bool GetIsCamBlending()
    {
        return cam_brain.IsBlending;
    }
}
