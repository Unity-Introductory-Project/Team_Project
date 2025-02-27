using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public float life = 100f;
    public float maxlife = 100f;

    public float speed = 5f;
    public float jumpHeight = 5f;
    private bool isSlide = false;
    public int jumpCount = 0;
    protected int fullJumpCount = 1;

    protected bool isGround = false;
    protected float dashTimer = 0f;
    private bool isInvincible = false;
    private float minY = -3f;
    private float maxY = 4f;


    public bool isDead = false;



    protected Rigidbody2D rb;
    protected Animator animator;
    protected BoxCollider2D colider;
    


    // 업적 관련 변수
    protected AchieveManager achieveManager;
    protected bool slideTracked = false;
    
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colider = GetComponent<BoxCollider2D>();

        achieveManager = AchieveManager.Instance;

        if (rb == null) Debug.LogError("Rigidbody2D가 없습니다!");
        if (animator == null) Debug.LogError("Animator가 없습니다!");
        if (colider == null) Debug.LogError("BoxCollider2D가 없습니다!");
    }

    public virtual void Update()
    {
        AutoMove();

        if (!isDead && !isInvincible)
        {
            ChangeHp(-(Time.deltaTime));
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, 0));
        }

        if (isInvincible)
        {
            if (Input.GetKey(KeyCode.Space)) // Space를 누르고 있으면
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            else
            {
                if (transform.position.y < minY)
                {
                    transform.position = new Vector3(transform.position.x, minY, transform.position.z); // Y 좌표 고정
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, 0));
                }
                // Space를 떼면 떨어지지 않도록 설정
            } 
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space)) Jump();
            if (isGround) // 땅에 있을 때만 슬라이드 가능
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) Slide();
                else StopSlide();
            }

            CheckFalling();
        }
    }

    /// <summary>
    /// 자동 이동하는 속도 조절 함수. 지금은 시간에 따라 빨라짐
    /// </summary>
    protected virtual void AutoMove()
    {
        speed += Time.deltaTime * 0.05f;
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    /// <summary>
    /// 점프 함수.
    /// </summary>
    protected virtual void Jump()
    {
        if (rb == null) return;
        
        if (jumpCount < fullJumpCount && life > 0) // 점프 횟수 제한
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight); // 점프
            SoundManager.instance.PlaySFX(SoundManager.instance.jumpSFX);
            jumpCount++; 
            //animator.Play("jump", 0, 0);
            animator.SetBool("isJump", true);

            // 업적 매니저에 점프 기록
            if (achieveManager != null) achieveManager.AddJump();
        }
    }

    /// <summary>
    /// 슬라이딩 함수 계속 누르면 계속 슬라이드하도록 설정
    /// </summary>
    protected virtual void Slide()
    {
        if (!isSlide) // 슬라이드 시작할 때만 실행
        {
            isSlide = true;
            SoundManager.instance.PlaySFX(SoundManager.instance.slideSFX);
            //animator.Play("StartSlide", 0, 0);
            animator.SetBool("isSlide", true);
        }

        // 한 번의 슬라이드에 한 번만 카운트
        if (!slideTracked && achieveManager != null)
        {
            achieveManager.AddSlide();
            slideTracked = true;
        }

        //animator.Play("KeepSlide", 0, 0);
    }

    /// <summary>
    /// 슬라이드 버튼을 그만 눌렀을 때 원래대로 돌아오도록 만드는 함수
    /// </summary>
    protected virtual void StopSlide()
    {
        if (isSlide) 
        {
            slideTracked = false;
            isSlide = false;
            animator.SetBool("isSlide", false);
        }
    }

    /// <summary>
    /// 점프 후 떨어지고 있는지 확인하여 떨어지고 있으면 그에 맞는 애니메이션 상태 설정.
    /// </summary>
    protected void CheckFalling()
    {
        if(rb.velocity.y < 0.0f && !isGround )
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", true);
        }
        else if (isGround) // 착지하면 애니메이션 초기화
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", false);
            jumpCount = 0;
        }
    }

    /// <summary>
    /// 충돌 확인 메서드
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            // 충돌 방향 확인
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 충돌 방향이 아래쪽인 경우에만 isGround를 true로 설정
                if (contact.normal.y > 0.7f)  // 0.7은 약간의 여유를 두기 위한 값
                {
                    isGround = true;
                    break;
                }
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle") && !isInvincible)
        {
            ChangeHp(-10f);
            SoundManager.instance.PlaySFX(SoundManager.instance.hitSFX);
        }
        if (collider.CompareTag("Apple"))
        {
            ChangeHp(0.5f);
            SoundManager.instance.PlaySFX(SoundManager.instance.itemSFX);

            // 업적 매니저에 사과 획득 기록
            if (achieveManager != null)
            {
                achieveManager.AddApple();
            }
        }
        if (collider.CompareTag("PoisonApple") && !isInvincible)
        {
            ChangeHp(-2f);
            SoundManager.instance.PlaySFX(SoundManager.instance.itemSFX);
        }
        if (collider.CompareTag("DashItem"))
        {
            StartCoroutine(ActivateDashMode());
            Destroy(collider.gameObject); // 아이템 제거
        }
       
    }

    /// <summary>
    /// 5초 동안 무적 + 속도 2배
    /// </summary>
    private IEnumerator ActivateDashMode()
    {
        if(isInvincible)
        {
            dashTimer = 5f;
            yield break;
        }

        isInvincible = true; //  무적 상태 ON
        speed *= 2; // 속도 2배 증가
        dashTimer = 5f; // 대시 타이머 초기화

        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            yield return null; // 한 프레임 대기
        }

        isInvincible = false; // 무적 상태 OFF
        speed /= 2; // 원래 속도로 복구
    }

    /// <summary>
    /// 땅에서 벗어남 확인 메서드
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            // 충돌 해제 시 바닥과의 접촉만 체크
            bool stillOnGround = false;
            
            // 다른 Ground 오브젝트와 여전히 접촉 중인지 확인
            Collider2D[] colliders = Physics2D.OverlapBoxAll(colider.bounds.center, colider.bounds.size, 0f);
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Ground") && col.gameObject != collision.gameObject)
                {
                    // 다른 Ground와 접촉 중이면 바닥 상태 유지
                    stillOnGround = true;
                    break;
                }
            }
            
            if (!stillOnGround)
            {
                isGround = false;
            }
        }
    }

    /// <summary>
    /// 데미지 계산 함수
    /// </summary>
    /// <param name="damage"></param>
    public virtual void ChangeHp(float value)
    {
        if (life <= value && isDead == false)
        {
            life = 0;
            Dead();
        }
        else if (value > 0 && life >= maxlife)
        {
            life = maxlife;
        }
        else
        {
            life += value;
            // 추가: 회복 후 최대체력 초과 시 최대체력으로 제한
            if (life > maxlife)
            {
                life = maxlife;
            }
        }
    }

    /// <summary>
    /// 체력이 0이 되었을 때 애니메이션 처리 등을 하기 위한 함수
    /// </summary>
    protected virtual void Dead()
    {
        isDead = true;
        speed = 0;
        animator.SetBool("isDead", true);

        // 업적 매니저에 사망 기록
        if (achieveManager != null) achieveManager.AddDeath();
    }

    protected virtual void Revive()
    {
        speed = 2f;
        animator.SetBool("isDead", false);
    }

    public abstract void Ability();

}