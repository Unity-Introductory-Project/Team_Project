using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRounderCharacter : CharacterBase
{
    private int maxActionCount = 3;
    private int actionCount = 0;
    private int appleCount = 0;
    public int needed = 10;

    public override void Start()
    {
        base.Start();
        fullJumpCount = 2;
    }

    public override void Update()
    {
        base.Update();

        // 특수 커맨드 입력 감지
        if (Input.GetKeyDown(KeyCode.Space)) Jump(); // 점프 추가
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGround && actionCount > 0) FastFallAction(); // 빠른 낙하
        if (Input.GetKeyDown(KeyCode.F) && actionCount > 0) AttackAction(); // 공격 (장애물 제거)
    }

    /// <summary>
    /// 기본 점프 (행동 개수 소모 X)
    /// </summary>
    protected override void Jump()
    {
        if (jumpCount < fullJumpCount) // 기본 점프 횟수 내에서는 행동 개수 소모 X
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
            Debug.Log($"기본 점프! 현재 점프 횟수: {jumpCount}/{fullJumpCount}");
        }
        else if (actionCount > 0) // 기본 점프 횟수를 초과하면 추가 점프로 처리 (행동 개수 소모)
        {
            JumpAction();
        }
    }

    /// <summary>
    /// 행동 개수를 하나 소모해서 추가 점프 가능 (fullJumpCount 초과 시에만 사용됨)
    /// </summary>
    private void JumpAction()
    {
        if (actionCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
            actionCount--; // 행동 개수 감소
            Debug.Log($"추가 점프 사용! 남은 행동 개수: {actionCount}");
        }
    }

    /// <summary>
    /// 행동 개수를 하나 소모하여 공중에서 빠르게 떨어질 수 있음.
    /// </summary>
    private void FastFallAction()
    {
        if (actionCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpHeight * 2); // 빠른 낙하
            actionCount--; // 행동 개수 감소
            Debug.Log($"빠른 낙하 사용! 남은 행동 개수: {actionCount}");
        }
    }

    /// <summary>
    /// 행동 개수를 하나 소모해서 공격! 장애물(Obstacle)과 독사과(PoisonApple) 제거
    /// </summary>
    private void AttackAction()
    {
        if (actionCount > 0)
        {
            animator.SetTrigger("Attack");
            actionCount--;
            StartCoroutine(ResetAttackTrigger());
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position + Vector3.right * 1.5f, 1f);
            foreach (Collider2D obj in hitObjects)
            {
                if (obj.CompareTag("PoisonApple") || obj.CompareTag("Obstacle"))
                {
                    Destroy(obj.gameObject);
                    Debug.Log($"공격 성공! {obj.tag} 제거! 남은 행동 개수: {actionCount}");
                }
            }
        }
    }

    /// <summary>
    /// 사과를 먹으면 행동 개수 증가 (최대 5까지)
    /// </summary>
    public void EatApple()
    {
        appleCount++;

        if(appleCount >= needed)
        {
            if(actionCount < maxActionCount)
            {
                actionCount++;
                Debug.Log($"행동 개수 +1 증가! 현재 행동 개수: {actionCount}/{maxActionCount}");
            }

            appleCount = 0;
        }
    }
    /// <summary>
    /// 사과를 먹으면 행동 개수가 증가하도록 `OnTriggerEnter2D` 재정의
    /// </summary>
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.CompareTag("Apple")) // 사과 먹으면 행동 개수 증가
        {
            EatApple();
        }
    }

    public override void Ability()
    {
        Debug.Log($"현재 행동 개수: {actionCount}/{maxActionCount}");
    }
    /// <summary>
    /// 일정 시간이 지나면 Attack 트리거를 해제 (애니메이션 실행 후 자동 초기화)
    /// </summary>
    private IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.5f); // 애니메이션이 충분히 실행될 시간을 줌
        animator.ResetTrigger("Attack"); // 트리거 초기화
    }
}
