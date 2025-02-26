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
        if (!isFastFalling) // �ߺ� ���� ����
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpHeight * 1.5f); // �Ʒ��� ������ ����
            isFastFalling = true;
            animator.SetBool("isFall", true);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isFastFalling = false; // ���� ������ �ٽ� �ʱ�ȭ
        }
    }

    public override void Ability()
    {
        Debug.Log("�����̵� �� ������ ������");
    }
}
