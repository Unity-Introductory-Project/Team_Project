using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFallCharacter : CharacterBase
{
    private bool isFastFalling = false;

    public override void Update()
    {
        base.Update();

        if(!isGround && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            FastFall();
        }
    }

    private void FastFall()
    {
        if (!isFastFalling) // 중복 실행 방지
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpHeight * 1.5f); // 아래로 빠르게 낙하
            isFastFalling = true;
            animator.SetBool("isFall", true);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isFastFalling = false; // 땅에 닿으면 다시 초기화
        }
    }

    public override void Ability()
    {
        Debug.Log("슬라이드 시 빠르게 떨어짐");
    }
}
