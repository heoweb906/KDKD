using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Move,
    Debug,
    Pause
}

public class StateManager : MonoBehaviour
{
    [HideInInspector]
    public static StateManager instance = null;

    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private DebugMode debugMode;

    [Header("디버그 모드 슬로우(보통이 1)"),SerializeField]
    private float debugTimeScale;
    [Header("디버그 모드 슬로우(보통이 1)"),Range(0,5),SerializeField]
    private float camBlendSpeed;

    public PlayerState state = PlayerState.Move; 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InputManager.instance.keyaction += SwitchModeTap;
    }

    public void SwitchModeTap()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;

        if (CompareState(PlayerState.Move) && !cameraController.GetIsCamBlending())
        {
            SetState(PlayerState.Debug);
            Time.timeScale = debugTimeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.008333f;
            cameraController.SetCamBlendSpeed(camBlendSpeed * Time.timeScale);
            StartCoroutine(C_SetDebugCam());
            playerMovement.DeleteMoveAction();
            debugMode.AddDebugFunc();
        }
        else if (CompareState(PlayerState.Debug) && !cameraController.GetIsCamBlending())
        {
            SetState(PlayerState.Move);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.008333f;
            cameraController.SetCamBlendSpeed(0.5f);
            playerMovement.AddMoveAction();
            StartCoroutine(C_SetMoveCam());
            debugMode.DeleteDebugFunc();
        }
    }

    private IEnumerator C_SetDebugCam()
    {
        yield return null;
        cameraController.SetDebugCam();
    }private IEnumerator C_SetMoveCam()
    {
        yield return null;
        cameraController.SetMoveCam();
    }

    public bool CompareState(PlayerState _state)
    {
        if (state == _state) return true;
        else return false;
    }

    public void SetState(PlayerState _state)
    {
        state = _state;
    }

    public void SetTimeScaleDebug()
    {
        if (Time.timeScale > 0.01f) Time.timeScale -= 1f * Time.deltaTime;
    }

}
