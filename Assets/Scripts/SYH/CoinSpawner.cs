using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // 생성할 코인 프리팹
    public Transform coinParent; // 코인을 저장할 부모 오브젝트
    public int coinsPerGround = 3; // Ground당 생성할 코인 개수
    public float coinSpacing = 1.5f; // 코인 간격 (일정하게 유지)
    
    private List<GameObject> coinList = new List<GameObject>(); // 생성된 코인 리스트

    public void SpawnCoins(Vector3 groundPosition, float groundWidth)
    {
        float startX = groundPosition.x - (groundWidth / 2) + coinSpacing / 2; // 코인 시작 위치
        float yPos = groundPosition.y - 2.5f; // Ground 바로위에 생성되록 

        for (int i = 0; i < coinsPerGround; i++)
        {
            float xPos = startX + (i * coinSpacing); // 일정한 간격으로 배치
            Vector3 coinPosition = new Vector3(xPos, yPos, 0);

            GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity, coinParent);
            coinList.Add(coin);
        }
    }
}