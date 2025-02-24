using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public float life = 100f;
    public float speed = 2f;
    public float jumpHeight = 5f;
    private bool isSlide = false;
    public int jumpCount = 0;
    protected int fullJumpCount = 1;
    private bool isGround = false;

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D colider;
    
    
    public virtual void Start()
    {
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

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // "Ground" 태그 확인
        {
            isGround = true;
        }
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Damaged(10f);
        }
    }
    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // "Ground"에서 벗어날 때
        {
            isGround = false;
        }
    }

    /// <summary>
    /// 데미지 계산 함수
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void Damaged(float damage)
    {
        if(life <= damage)
        {
            life = 0;
            Dead();
        }
        else
        {
            life -= damage;
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
