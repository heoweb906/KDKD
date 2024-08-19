using UnityEngine;
using DG.Tweening;

public class TrainController : MonoBehaviour
{
    public Transform[] waypoints; // ��θ� ������ ����Ʈ��
    public float duration = 20f; // ��θ� ���󰡴� �ð�
    public PathType pathType = PathType.CatmullRom; // ��� ���� (���� �Ǵ� �)
    public PathMode pathMode = PathMode.Full3D; // ��� ���

    public float fStartinterval;

    public void Movetrain()
    {
        Invoke("MoveAlongPath", fStartinterval);
    }
    
    void MoveAlongPath()
    {
        // ��� ���� �� �̵� ����
        Vector3[] pathPoints = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            pathPoints[i] = waypoints[i].position;
        }

        // ���� �̵�
        transform.DOPath(pathPoints, duration, pathType, pathMode)
                .SetLookAt(0.01f, Vector3.forward) // �̵� ������ ���ϵ��� ȸ�� (X�� ����)
                .SetEase(Ease.Linear) // �ε巯�� �̵��� ���� Ease ����
                .OnComplete(MoveAlongPath); // �̵��� ������ �ٽ� ����
    }
}
