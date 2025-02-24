using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float life = 100f;
    private float speed = 2f;
    private float jumpHeight = 5f;
    private bool isSlide = false;
    private bool isJump = false;
    public int jumpCount = 0;
    private int fullJumpCount = 1;
    private bool isGround = false;

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D colider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colider = GetComponent<BoxCollider2D>();

        if (rb == null) Debug.LogError("Rigidbody2D가 없습니다!");
        if (animator == null) Debug.LogError("Animator가 없습니다!");
        if (colider == null) Debug.LogError("BoxCollider2D가 없습니다!");
    }

    void Update()
    {
        AutoMove();

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (isGround&&Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) Slide();
        else StopSlide();

        CheckFalling();
    }

    void AutoMove()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void Jump()
    {
        if (rb == null) return;

        if (jumpCount < fullJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
            isJump = true;
            animator.Play("jump", 0, 0);
            animator.SetBool("isJump", true);
        }
    }

    void Slide()
    {
        if (!isSlide) // 슬라이드 시작할 때만 실행
        {
            isSlide = true;
            animator.Play("StartSlide", 0, 0);
            animator.SetBool("isSlide", true);
        }

        animator.Play("KeepSlide", 0, 0);
    }

    void StopSlide()
    {
        if (isSlide) 
        {
            isSlide = false;
            animator.SetBool("isSlide", false);
        }
    }

    void CheckFalling()
    {
        if(rb.velocity.y < 0.0f && !isGround )
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", true);
        }
        else if (isGround) // 착지하면 애니메이션 초기화
        {
            isJump = false;
            animator.SetBool("isJump", false);
            animator.SetBool("isFall", false);
            jumpCount = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // "Ground" 태그 확인
        {
            isGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // "Ground"에서 벗어날 때
        {
            isGround = false;
        }
    }

}
