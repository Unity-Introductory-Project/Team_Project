using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllRounderCharacter : CharacterBase
{
    private int maxActionCount = 3;
    private int actionCount = 0;
    private int appleCount = 0;
    public int needed = 10;

    public Slider actionGauge;
    public TextMeshProUGUI actionCountText;


    public override void Start()
    {
        base.Start();
        UpdateUI();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F) && actionCount > 0) AttackAction(); // 공격 (장애물 제거)

        UpdateUI();
    }

    /// <summary>
    /// 0.5초 동안 공격 판정을 유지하며 적 제거
    /// </summary>
    private IEnumerator AttackCoroutine()
    {
        float attackDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position + Vector3.right * 1.5f, 1f);
            foreach (Collider2D obj in hitObjects)
            {
                if (obj.CompareTag("PoisonApple") || obj.CompareTag("Obstacle"))
                {
                    Destroy(obj.gameObject);
                    Debug.Log($"공격 성공! {obj.tag} 제거!");
                }
            }

            yield return new WaitForSeconds(0.1f); // 0.1초마다 반복 실행 (더 부드러운 공격 효과)
            elapsedTime += 0.1f;
        }

        animator.ResetTrigger("Attack"); // 공격 종료 후 애니메이션 리셋
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
            StartCoroutine(AttackCoroutine());
        }
    }

    /// <summary>
    /// 사과를 먹으면 행동 개수 증가 (최대 5까지)
    /// </summary>
    public void EatApple()
    {
        if (actionCount < maxActionCount)
        {
            appleCount++;
            if (appleCount >= needed)
            {
                actionCount++;
                Debug.Log($"행동 개수 +1 증가! 현재 행동 개수: {actionCount}/{maxActionCount}");
                appleCount = 0;
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        actionGauge.value = (float)appleCount / needed;
        actionCountText.text = actionCount.ToString();
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
