using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10; // 코인 점수
    public float lifetime = 10f; // 코인 유지 시간 (10초 후 삭제)

    void Start()
    {
        Destroy(gameObject, lifetime); // 생성 후 일정 시간이 지나면 삭제
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어가 닿았을 때만 동작
        {
            // ScoreManager.instance.AddScore(scoreValue); // 점수 추가
            Destroy(gameObject); // 코인 삭제
        }
    }
}