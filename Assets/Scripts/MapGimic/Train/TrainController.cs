using UnityEngine;
using DG.Tweening;

public class TrainController : MonoBehaviour
{
    public Transform[] waypoints; // 경로를 설정할 포인트들
    public float duration = 20f; // 경로를 따라가는 시간
    public PathType pathType = PathType.CatmullRom; // 경로 유형 (직선 또는 곡선)
    public PathMode pathMode = PathMode.Full3D; // 경로 모드

    public float fStartinterval;

    public void Movetrain()
    {
        Invoke("MoveAlongPath", fStartinterval);
    }
    
    void MoveAlongPath()
    {
        // 경로 설정 및 이동 시작
        Vector3[] pathPoints = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            pathPoints[i] = waypoints[i].position;
        }

        // 기차 이동
        transform.DOPath(pathPoints, duration, pathType, pathMode)
                .SetLookAt(0.01f, Vector3.forward) // 이동 방향을 향하도록 회전 (X축 기준)
                .SetEase(Ease.Linear) // 부드러운 이동을 위한 Ease 설정
                .OnComplete(MoveAlongPath); // 이동이 끝나면 다시 시작
    }
}
