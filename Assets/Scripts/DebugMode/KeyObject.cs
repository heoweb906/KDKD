using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeyObject : MonoBehaviour
{
    //[Header("UI ����")]
    //public GameObject CanvasUI;
    //public GameObject CanInteractionUI; // ��ȣ�ۿ� �⺻ ǥ�� UI
    //public GameObject IconImage;
    //public Image[] IconImageList;

    [Header("KeyCapEffect")]
    public GameObject particle_Select;
    // 0 - Plus
    // 1 - Minus 
    // 2 - J
    public ParticleSystem[] particles_KeyCap;
    private GameObject[] particles_KeyCap_Obj;


    [Header("��ȣ�ۿ� ���� ����")]
    private bool bIsInteraction; // ���� ��ȣ �ۿ� ��
    public List<int> keycapFuncNum = new List<int>(); // ����� �Լ���
    private int maxKeyCapFuncCnt = 3;
    private bool isExecuting = false;

    [Header("- / + ����")]
    public bool bCanSizeControl; // ������ ������ �����Ѱ�
    public int iCntSizeControl; // ������ ����

    [Header(" J ����")]
    public bool bCanGrab;
    public Vector3 positionPlayer;
    private Vector3 direction;
    public float forceGrap;

    [Header(" Z ����")]
    public bool bCanChangeToSpring;
    public bool bIsSpring;
    public float bounceForce = 10f;


    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        particles_KeyCap_Obj = new GameObject[particles_KeyCap.Length];
        for (int i = 0; i < particles_KeyCap.Length; i++) particles_KeyCap_Obj[i] = particles_KeyCap[i].gameObject;
      
    }



    // #. ��ȣ�ۿ� �Լ� �߰�
    public void PlusKeyCapFunc(int funcNum)
    {
        if(keycapFuncNum.Count < maxKeyCapFuncCnt && bIsInteraction)
        {
            keycapFuncNum.Add(funcNum);
            InteractionEffectupdate();
        }
    }
    // #. ��ȣ�ۿ� �Լ� ����
    public int MinusKeyCapFunc()
    {
        int funcNum = keycapFuncNum[keycapFuncNum.Count - 1];

        if (keycapFuncNum.Count >= 1 && bIsInteraction)
        {
            keycapFuncNum.RemoveAt(keycapFuncNum.Count - 1);
            InteractionEffectupdate();
        }
        return funcNum;
    }
     


    // #. ���� Űĸ ��ɿ� �´� ���� ����Ʈ Ȱ��ȭ
    public void InteractionEffectupdate()
    {
        for (int i = 0; i < particles_KeyCap.Length; i++)
        {
            if (keycapFuncNum.Contains(i))
            {
                if (!particles_KeyCap[i].isPlaying)
                {
                    particles_KeyCap_Obj[i].SetActive(true);
                    particles_KeyCap[i].Play();
                }   
            }
            else
            {
                particles_KeyCap[i].Stop();
                particles_KeyCap_Obj[i].SetActive(false);
            }
        }
    }

    // #. ���� ��ȣ�ۿ� ������ �������� ǥ���� ��
    public void On_Interaction(Vector3 rotationDirection)
    {
        bIsInteraction = true;

        Quaternion rotation = Quaternion.LookRotation(rotationDirection);
        particle_Select.SetActive(true);
    }
    // #. ��ȣ �ۿ� ���� ���°� ������ ��
    public void Off_Interaction()
    {
        bIsInteraction = false;
        particle_Select.SetActive(false);
    }


    // #. ��ȣ�ۿ� ���������� ����
    public void ImplementKeyCapFunc()
    {
        if (!isExecuting && keycapFuncNum.Count > 0)
        {
            isExecuting = true;

            direction = positionPlayer - transform.position;

            StartCoroutine(RunKeyCapFunc());
        }
        else if (keycapFuncNum.Count == 0) Debug.LogWarning("keycapFuncNum is empty, no functions to execute.");
    }
    private IEnumerator RunKeyCapFunc()
    {
        yield return new WaitForSeconds(1f);
        while (keycapFuncNum.Count > 0)
        {
            int funcNum = keycapFuncNum[0];
            keycapFuncNum.RemoveAt(0);
            switch (funcNum)
            {
                case 0:
                    KeyCapFunc_Bigger();
                    break;
                case 1:
                    KeyCapFunc_Smaller();
                    break;
                case 2:
                    KeyCapFunc_Grap();
                    break;
                case 3:
                    KeyCapFunc_GiveSpring();
                    break;
                case 4:
                    KeyCapFunc_FixPosition();
                    break;
                case 5:
                    KeyCapFunc_Disappear();
                    break;
                default:
                    Debug.LogError("Unknown function number: " + funcNum);
                    break;
            }
            InteractionEffectupdate();
            yield return new WaitForSeconds(1f); 
        }

        isExecuting = false; 
    }





    #region // ��ȣ�ۿ� �Լ���
    // #. "+" ��ư - Ŀ����
    public void KeyCapFunc_Bigger()
    {
        Debug.Log("Ŀ����");
        gameObject.transform.DOScale(gameObject.transform.localScale * 2f, 0.15f).SetEase(Ease.InOutBack);

        InteractionEffectupdate();
    }


    // #. "-" ��ư - �۾�����
    public void KeyCapFunc_Smaller()
    {
        Debug.Log("�۾�����");
        gameObject.transform.DOScale(gameObject.transform.localScale * 0.5f, 0.15f).SetEase(Ease.InOutBack);

        InteractionEffectupdate();

    }

  

    // #. "J" ��ư - ����
    public void KeyCapFunc_Grap()
    {
        Debug.Log("����");

        rb.AddForce(direction.normalized * forceGrap);

        InteractionEffectupdate();
    }

    // #. "Z" ��ư - ������ �Ӽ� �߰��ϱ�
    public void KeyCapFunc_GiveSpring()
    {
        bIsSpring = !bIsSpring;

        if(bIsSpring) Debug.Log("������ �Ӽ��� �߰��Ǿ����ϴ�.");
        else Debug.Log("������ �Ӽ��� ���ŵǾ����ϴ�.");

    }

    // #. "T" ��ư - ��ġ ������Ű��
    public void KeyCapFunc_FixPosition()
    {

    }

    // #. "X" ��ư - ������Ʈ ����
    public void KeyCapFunc_Disappear()
    {

    }

    #endregion



    // #. �׽�Ʈ������ ������ �κ��� ���߿� ������ ��
    // #. �׽�Ʈ������ ������ �κ��� ���߿� ������ ��
    // #. �׽�Ʈ������ ������ �κ��� ���߿� ������ ��
    // #. �׽�Ʈ������ ������ �κ��� ���߿� ������ ��
    void OnCollisionEnter(Collision collision)
    {
        if(bIsSpring)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log("������ �۵�");

                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 bounceDirection = collision.transform.position - contactPoint;
                bounceDirection = bounceDirection.normalized;
                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }









}
