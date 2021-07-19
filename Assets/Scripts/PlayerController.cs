using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crounchSpeed;


    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isRun = false; // 기본값
    private bool isCrounch = false;
    private bool isGround = true;

    // 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지 여부
    private CapsuleCollider capsuleCollider;

    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;

        // 초기화
        originPosY = theCamera.transform.localPosition.y;
        // localPosition을 쓴 이유 : 월드 기준에서 카메라의 기본위치?(reset시켰을 때 0,0,0)는 월드에서의0,0,0이지만
        // Player 기준에서 카메라의 기본 위치는 월드에서의 0,1,0이기 때문에 상대적인 위치 변화로 인해 쓴다.
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun(); // 걷는지 뛰는지 판단
        TryCrouch();
        Move(); // 그 다음 움직이게 하기. 그래서 Move가 TryRun보다 뒤에 있어야한다.
        CameraRotation();
        CharacterRotation();
    }

    // 앉기 시도
    private void TryCrouch()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
            Crouch();
		}
	}

    // 앉기 동작
    private void Crouch()
	{
        isCrounch = !isCrounch;
		if (isCrounch)
		{
            applySpeed = crounchSpeed;
            applyCrouchPosY = crouchPosY;
		}
		else
		{
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
		}

        StartCoroutine(CrouchCoroutine());
	    
    }

    // 부드러운 동작 실행
    IEnumerator CrouchCoroutine()
	{
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
		{
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
			if (count > 15)
			{
                break;
			}
            yield return null;
		}
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
	}

    // 지면 체크
    private void IsGround()
	{
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
	}

    // 점프 시도
    private void TryJump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isGround) 
        {
            Jump();
		}
	}

    // 점프
    private void Jump()
	{
        // 앉은 상태 해제
		if (isCrounch)
		{
            Crouch();
		}
        myRigid.velocity = transform.up * jumpForce;
	}

    // 달리기 시도
    private void TryRun()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
            Running();
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
            RunningCancel();
		}
	}

    // 달리기
    private void Running()
	{
        // 앉은 상태 해제
        if (isCrounch)
        {
            Crouch();
        }
        isRun = true;
        applySpeed = runSpeed;
	}

    // 달리기 취소
    private void RunningCancel()
	{
        isRun = false;
        applySpeed = walkSpeed;
	}

    // 움직임 실행
	private void Move()
	{
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
	
    // 좌우 캐릭터 회전
    private void CharacterRotation()
	{
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //Debug.Log(myRigid.rotation); // 삼원수
        //Debug.Log(myRigid.rotation.eulerAngles); // 사원수
    }

    // 상하 카메라 회전
    private void CameraRotation()
	{
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
