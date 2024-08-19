using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [Header("ThisComponent")]
    [SerializeField]
    private Rigidbody playerRigid;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Camera playerMoveCam;

    [Header("이동속도")]
    [SerializeField, Range(0,60)]
    private float playerMoveSpeed;
    [SerializeField, Range(0,60)]
    private float playerMoveLerpSpeed;
    [SerializeField, Range(0,60)]
    private float playerRotateLerpSpeed;
    [Header("점프")]
    [SerializeField, Range(0,20)]
    private float firstJumpPower;
    [SerializeField, Range(0,20)]
    private float secondJumpPower;
    [SerializeField, Range(0,100)]
    private float fallingPower;
    [SerializeField, Range(0,20)]
    private float maxFallingSpeed;
    [Header("대쉬")]
    [SerializeField, Range(0,60)]
    private float dashSpeed;
    [SerializeField, Range(0,2)]
    private float dashTime;

    private Vector3 curDirection = Vector3.zero;
    private Vector3 preDirection = Vector3.zero;
    
    private int jumpCount;

    [Header("경사로")]
    [SerializeField,Range(0,2)]
    private float rayDistance = 1f;
    private RaycastHit slopeHit;
    private int groundLayer;
    [SerializeField, Range(0, 90)]
    private int maxSlopeAngle;

    private void Start()
    {
        AddMoveAction();
        InputManager.instance.FixedKeyaction += ControllGravity;
        groundLayer = ~(1 << LayerMask.NameToLayer("Player"));
    }

    private void ControllGravity()
    {
        Debug.Log(playerRigid.velocity.y);
        if(playerRigid.velocity.y < 3)
        {
            if (playerRigid.velocity.y > -maxFallingSpeed)
                playerRigid.velocity -= new Vector3(0, fallingPower * Time.fixedDeltaTime, 0);
            else playerRigid.velocity = new Vector3(playerRigid.velocity.x, -maxFallingSpeed, playerRigid.velocity.z);
        }

    }

    private void PlayerWalk()
    {
        float _horizontal = Input.GetAxisRaw("Horizontal");
        float _vertical = Input.GetAxisRaw("Vertical");

        // 카메라 방향 값 가져오기
        Transform cameraTransform = playerMoveCam.transform;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        // 카메라 방향 * 입력값 (카메라 방향 기준으로 이동)
        curDirection = forward * _vertical + right * _horizontal;
        if (curDirection.magnitude > 1) curDirection = curDirection.normalized;


        // Rotation 이동 방향으로 조절
        if(curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(curDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotateLerpSpeed * Time.fixedDeltaTime);
        }

        // Rigid의 속도 조절로 이동, 보간 사용
        curDirection = Vector3.Lerp(preDirection, curDirection, playerMoveLerpSpeed * Time.fixedDeltaTime);
        playerAnim.SetFloat("moveSpeed", curDirection.magnitude); // 애니메이터 moveSpeed값 세팅

        Vector3 velocity;
        Vector3 gravity;
        
        if (IsOnSlope())
        {
            velocity = AdjustDirectionToSlope(curDirection);
            gravity = Vector3.zero;
            playerRigid.useGravity = false;
        }
        else
        {
            velocity = curDirection;
            gravity = new Vector3(0, playerRigid.velocity.y, 0);
            playerRigid.useGravity = true;
        }

        playerRigid.velocity = velocity * playerMoveSpeed + gravity;

        preDirection = curDirection;
    }

    private bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, Vector3.down * rayDistance, Color.red);
        if(Physics.Raycast(ray,out slopeHit, rayDistance, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle && jumpCount == 0;
        }
        return false;
    }

    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0)
        {
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
            playerRigid.AddForce(new Vector3(0, firstJumpPower, 0), ForceMode.VelocityChange);
            jumpCount = 1;
            playerAnim.SetInteger("jumpCount", jumpCount);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 1)
        {
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
            playerRigid.AddForce(new Vector3(0, secondJumpPower, 0), ForceMode.VelocityChange);
            jumpCount = 2;
            playerAnim.SetTrigger("doubleJump");
        }
    }

    private bool bCanDash = true;

    private void PlayerDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && bCanDash)
        {
            StartCoroutine(C_PlayerDash());
        }
    }

    private float dashPosY;

    private IEnumerator C_PlayerDash()
    {
        bCanDash = false;
        InputManager.instance.FixedKeyaction -= PlayerWalk;
        InputManager.instance.keyaction += LockDashPositionY;
        Vector3 dashVelocity = gameObject.transform.forward * dashSpeed;
        dashVelocity.y = 0;
        playerRigid.velocity = dashVelocity;
        dashPosY = gameObject.transform.position.y;

        yield return new WaitForSeconds(dashTime);

        InputManager.instance.FixedKeyaction += PlayerWalk;
        InputManager.instance.keyaction -= LockDashPositionY;
        //playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
        if (groundList.Count > 0)
            bCanDash = true;
    }

    public void LockDashPositionY()
    {
        if (playerRigid.velocity.y < 0)
        {
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
        }
        //gameObject.transform.position = new Vector3(transform.position.x,dashPosY, transform.position.z);
    }


    public void AddMoveAction()
    {
        InputManager.instance.keyaction += PlayerJump;
        InputManager.instance.keyaction += PlayerDash;
        InputManager.instance.FixedKeyaction += PlayerWalk;
    }

    public void DeleteMoveAction()
    {
        InputManager.instance.keyaction -= PlayerJump;
        InputManager.instance.keyaction -= PlayerDash;
        InputManager.instance.FixedKeyaction -= PlayerWalk;
    }

    [SerializeField]
    private List<GameObject> groundList = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            groundList.Add(other.gameObject);
            jumpCount = 0;
            playerAnim.SetInteger("jumpCount", jumpCount);
            bCanDash = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && playerRigid.velocity.y < 0)
        {
            jumpCount = 0;
            playerAnim.SetInteger("jumpCount", jumpCount);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            groundList.Remove(other.gameObject);
            if (groundList.Count > 0) return;
            jumpCount = 1;
            playerAnim.SetInteger("jumpCount", jumpCount);
        }
    }
}
