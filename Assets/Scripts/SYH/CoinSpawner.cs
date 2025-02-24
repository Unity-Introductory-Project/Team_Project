using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // 생성할 코인 프리팹
    public Transform coinParent; // 코인을 저장할 부모 오브젝트
    public Transform player; // 플레이어
    public int maxCoinsPerGround = 0; // Ground당 최대 코인 개수
    public float minY = 0f; // 코인 최소 Y 위치
    public float maxY = 0f; // 코인 최대 Y 위치
    public float coinSpacing = 0f; // 코인 간격
    private List<GameObject> coinList = new List<GameObject>(); // 생성된 코인을 저장할 리스트
    public void SpawnCoins(Vector3 groundPosition, float groundWidth)
    {
        int coinCount = Random.Range(1, maxCoinsPerGround + 1); // 랜덤한 개수의 코인 생성
        for (int i = 0; i < coinCount; i++)
        {
            float xOffset = Random.Range(-groundWidth / 2, groundWidth / 2);
            float yOffset = Random.Range(minY, maxY);
            Vector3 coinPosition = new Vector3(groundPosition.x + xOffset, groundPosition.y + yOffset, 0);

            GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity, coinParent);
            coinList.Add(coin);
        }
    }
}