using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float life = 100f;
    private float speed = 2f;
    private float jumpHeight = 5f;
    private bool isSlide = false;
    public int jumpCount = 0;
    private int fullJumpCount = 2;
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
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) Slide();
        else StopSlide();

        if (isGround)
        { 
            jumpCount = 0; 
            animator.SetBool("isJump", false); 
        }
        else
        {
            animator.SetBool("isJump", true); // 공중에 있는 경우 isJump 유지
        }
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
        }
    }

    void Slide()
    {
        if (isSlide) return;

        isSlide = true;
        animator.SetBool("isSlide", true);

    }

    void StopSlide()
    {
        if (isSlide)
        {
            isSlide = false;
            animator.SetBool("isSlide", false);
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
