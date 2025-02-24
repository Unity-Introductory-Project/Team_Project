using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator = null; // 애니메이터 컴포넌트
    Rigidbody2D _rigidbody = null; // 리지드바디 컴포넌트
    public float flapForce = 6f; // 플랩 힘
    public float forwardSpeed = 3f; // 전진 속도
    public bool isDead = false; // 사망 여부
    float deathCooldown = 0f; // 사망 후 재시작까지의 시간
    bool isFlap = false; // 플랩 여부
    public bool godMode = false; // 무적 모드
    void Start()
    {
        animator = transform.GetComponentInChildren<Animator>(); // 자식 오브젝트에서 애니메이터 컴포넌트 찾기
        _rigidbody = transform.GetComponent<Rigidbody2D>();  // 리지드바디 컴포넌트 찾기

        if (animator == null) // 애니메이터 컴포넌트가 없다면
        {
            // Debug.LogError("Not Founded Animator"); // 에러 메시지 출력
        }

        if (_rigidbody == null) // 리지드바디 컴포넌트가 없다면
        {
            Debug.LogError("Not Founded Rigidbody"); // 에러 메시지 출력
        }
    }
    void Update()
    {
        if (isDead) // 사망 상태라면
        {
            if (deathCooldown <= 0) // 사망 후 재시작까지의 시간이 지났다면
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // 스페이스바 혹은 마우스 왼쪽 버튼을 눌렀다면
                {
                    // 게임 재시작
                }
            }
            else
            {
                deathCooldown -= Time.deltaTime; // 사망 후 재시작까지의 시간을 줄인다
            }
        }
        else // 사망 상태가 아니라면
        {
            if (Input.GetKeyDown(KeyCode.Space) ||Input.GetMouseButtonDown(0)) // 스페이스바 혹은 마우스 왼쪽 버튼을 눌렀다면
            {
                isFlap = true; // 플랩 상태로 변경
            }
        }
    }

    public void FixedUpdate() 
    {
        if (isDead) // 사망 상태라면
            return; // 아래 코드를 실행하지 않고 종료
        
        Vector3 velocity = _rigidbody.velocity; // 현재 속도를 가져온다
        velocity.x = forwardSpeed; // x축 속도를 전진 속도로 변경

        if (isFlap) // 플랩 상태라면
        {
            velocity.y += flapForce; // y축 속도에 플랩 힘을 더한다
            isFlap = false; // 플랩 상태를 해제한다
        }
        
        _rigidbody.velocity = velocity; // 변경된 속도를 적용한다
        
        float angle = Mathf.Clamp((_rigidbody.velocity.y * 10f), -90, 90); // 현재 속도에 따라 플레이어의 각도를 계산한다
        float lerpAngle = Mathf.Lerp(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * 5f); // 부드럽게 회전하기 위해 현재 각도에서 목표 각도로 회전한다
        transform.rotation = Quaternion.Euler(0, 0, lerpAngle); // 회전을 적용한다
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode)
            return;
            
	      if (isDead)
            return;

        animator.SetInteger("IsDie", 1);
        isDead = true;
        deathCooldown = 1f;
    }
}