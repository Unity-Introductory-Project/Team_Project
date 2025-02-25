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
            // 일반사과에 닿았다면 체력회복과 스코어점수 획득
            // if (CompareTag("Apple"))
            // {
            //     other.GetComponent<Player>().Heal(1); // 체력 1 회복
            //     other.GetComponent<Player>().AddScore(scoreValue); // 점수 획득
            // }
            // // 독사과에 닿았다면 체력감소
            // else if (CompareTag("PoisonApple"))
            // {
            //     other.GetComponent<Player>().Hurt(1); // 체력 1 감소
            // }
            
            Destroy(gameObject); // 사과 삭제
        }
    }
}