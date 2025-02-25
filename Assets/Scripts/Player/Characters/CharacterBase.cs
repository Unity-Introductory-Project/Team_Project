using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public float life = 100;
    public float speed = 2f;
    public float jumpHeight = 5f;
    private bool isSlide = false;
    public int jumpCount = 0;
    protected int fullJumpCount = 1;
    private bool isGround = false;
    public float maxlife = 100f;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D colider;
    
    
    public virtual void Start()
    {
        life = maxlife;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colider = GetComponent<BoxCollider2D>();

        if (rb == null) Debug.LogError("Rigidbody2D가 없습니다!");
        if (animator == null) Debug.LogError("Animator가 없습니다!");
        if (colider == null) Debug.LogError("BoxCollider2D가 없습니다!");
    }

    public virtual void Update()
    {
        AutoMove();

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (isGround&&Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) Slide();
        else StopSlide();

        CheckFalling();

        if(life <= 0)
        {
            Dead();
        }//죽음 확인 위한 함수

        ChangeHp(-(Time.deltaTime));
    }

    /// <summary>
    /// 자동 이동하는 속도 조절 함수. 지금은 시간에 따라 빨라짐
    /// </summary>
    protected virtual void AutoMove()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    /// <summary>
    /// 점프 함수.
    /// </summary>
    protected virtual void Jump()
    {
        if (rb == null) return;

        if (jumpCount < fullJumpCount && life > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
            //animator.Play("jump", 0, 0);
            animator.SetBool("isJump", true);
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
            //animator.Play("StartSlide", 0, 0);
            animator.SetBool("isSlide", true);
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
        if (collider.CompareTag("Obstacle"))
        {
            Debug.Log("장애물(Obstacle) 충돌 감지됨!");
            ChangeHp(-10f);
        }
        if (collider.CompareTag("Apple"))
        {
            Debug.Log("사과(Apple) 충돌 감지됨! 체력 회복!");
            ChangeHp(10f);
        }
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
        Debug.Log($" {gameObject.name}의 hp {value} 변동! 현재 체력: {life}");
        if (life <= value)
        {
            life = 0;
            Dead();
        }
        else if(life == maxlife)
        {
            life = maxlife;
        }
        else
        {
            life += value;
        }
    }

    /// <summary>
    /// 체력이 0이 되었을 때 애니메이션 처리 등을 하기 위한 함수
    /// </summary>
    protected virtual void Dead()
    {
        speed = 0;
        animator.SetBool("isDead", true);
    }

    protected virtual void Revive()
    {
        speed = 2f;
        animator.SetBool("isDead", false);
    }

    public abstract void Ability();

}