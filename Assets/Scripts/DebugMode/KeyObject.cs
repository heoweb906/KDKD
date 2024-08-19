using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeyObject : MonoBehaviour
{
    //[Header("UI 관련")]
    //public GameObject CanvasUI;
    //public GameObject CanInteractionUI; // 상호작용 기본 표시 UI
    //public GameObject IconImage;
    //public Image[] IconImageList;

    [Header("KeyCapEffect")]
    public GameObject particle_Select;
    // 0 - Plus
    // 1 - Minus 
    // 2 - J
    public ParticleSystem[] particles_KeyCap;
    private GameObject[] particles_KeyCap_Obj;


    [Header("상호작용 관련 변수")]
    private bool bIsInteraction; // 현재 상호 작용 중
    public List<int> keycapFuncNum = new List<int>(); // 저장된 함수들
    private int maxKeyCapFuncCnt = 3;
    private bool isExecuting = false;

    [Header("- / + 관련")]
    public bool bCanSizeControl; // 사이즈 조절이 가능한가
    public int iCntSizeControl; // 사이즈 정도

    [Header(" J 관련")]
    public bool bCanGrab;
    public Vector3 positionPlayer;
    private Vector3 direction;
    public float forceGrap;

    [Header(" Z 관련")]
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



    // #. 상호작용 함수 추가
    public void PlusKeyCapFunc(int funcNum)
    {
        if(keycapFuncNum.Count < maxKeyCapFuncCnt && bIsInteraction)
        {
            keycapFuncNum.Add(funcNum);
            InteractionEffectupdate();
        }
    }
    // #. 상호작용 함수 제거
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
     


    // #. 사용될 키캡 기능에 맞는 색상 이펙트 활성화
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

    // #. 현재 상호작용 가능한 상태임을 표시할 때
    public void On_Interaction(Vector3 rotationDirection)
    {
        bIsInteraction = true;

        Quaternion rotation = Quaternion.LookRotation(rotationDirection);
        particle_Select.SetActive(true);
    }
    // #. 상호 작용 가능 상태가 해제될 때
    public void Off_Interaction()
    {
        bIsInteraction = false;
        particle_Select.SetActive(false);
    }


    // #. 상호작용 순차적으로 실행
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





    #region // 상호작용 함수들
    // #. "+" 버튼 - 커지기
    public void KeyCapFunc_Bigger()
    {
        Debug.Log("커지기");
        gameObject.transform.DOScale(gameObject.transform.localScale * 2f, 0.15f).SetEase(Ease.InOutBack);

        InteractionEffectupdate();
    }


    // #. "-" 버튼 - 작아지기
    public void KeyCapFunc_Smaller()
    {
        Debug.Log("작아지기");
        gameObject.transform.DOScale(gameObject.transform.localScale * 0.5f, 0.15f).SetEase(Ease.InOutBack);

        InteractionEffectupdate();

    }

  

    // #. "J" 버튼 - 당기기
    public void KeyCapFunc_Grap()
    {
        Debug.Log("당기기");

        rb.AddForce(direction.normalized * forceGrap);

        InteractionEffectupdate();
    }

    // #. "Z" 버튼 - 스프링 속성 추가하기
    public void KeyCapFunc_GiveSpring()
    {
        bIsSpring = !bIsSpring;

        if(bIsSpring) Debug.Log("스프링 속성이 추가되었습니다.");
        else Debug.Log("스프링 속성이 제거되었습니다.");

    }

    // #. "T" 버튼 - 위치 고정시키기
    public void KeyCapFunc_FixPosition()
    {

    }

    // #. "X" 버튼 - 오브젝트 제거
    public void KeyCapFunc_Disappear()
    {

    }

    #endregion



    // #. 테스트용으로 제작한 부분임 나중에 지워야 함
    // #. 테스트용으로 제작한 부분임 나중에 지워야 함
    // #. 테스트용으로 제작한 부분임 나중에 지워야 함
    // #. 테스트용으로 제작한 부분임 나중에 지워야 함
    void OnCollisionEnter(Collision collision)
    {
        if(bIsSpring)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log("스프링 작동");

                Vector3 contactPoint = collision.contacts[0].point;
                Vector3 bounceDirection = collision.transform.position - contactPoint;
                bounceDirection = bounceDirection.normalized;
                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }









}
