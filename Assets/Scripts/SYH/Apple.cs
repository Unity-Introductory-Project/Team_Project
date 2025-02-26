using UnityEngine;

public class Apple : MonoBehaviour
{
    public int scoreValue = 10; // 사과 점수
    public float lifetime = 10f; // 사과 유지 시간

    void Start()
    {
        Destroy(gameObject, lifetime); // 생성 후 일정 시간이 지나면 삭제
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 닿았을 때만 동작
        {            
            Destroy(gameObject); // 사과 삭제
        }
    }
}