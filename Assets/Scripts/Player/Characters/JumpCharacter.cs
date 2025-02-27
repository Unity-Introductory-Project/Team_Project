using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpCharacter : CharacterBase
{
    private int maxJumpCount = 5;
    private int appleCount = 0;

    public override void Start()
    {
        base.Start();
        fullJumpCount = 2;
    }

    public override void Ability()
    {
        
    }

    public void EatApple()
    {
        appleCount++;

        // 사과 5개 먹을 때마다 점프 횟수 증가 (최대 5까지)
        if (appleCount % 5 == 0 && fullJumpCount < maxJumpCount)
        {
            fullJumpCount++;
        }
    }

    /// <summary>
    /// 오브젝트 충돌 감지 - 사과를 먹으면 점프 횟수 증가
    /// </summary>
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider); // 부모 클래스의 충돌 로직 유지

        if (collider.CompareTag("Apple"))
        {
            EatApple(); // 사과를 먹으면 점프 증가
        }
    }

}

