using UnityEngine;
using System.Collections.Generic;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject backgroundPrefab; // 생성할 Ground 프리팹
    public Transform backgroundParent; // Ground를 저장할 Grid 오브젝트
    public Transform player; // 플레이어
    public int initialCount = 0; // 처음 생성할 Ground 개수
    public float backgroundWidth = 0f; // 배경의 넓이
    private List<GameObject> backgroundList = new List<GameObject>(); // 생성된 Ground 오브젝트를 저장할 리스트

    void Start()
    {
        // 이후 Ground는 랜덤한 Y 위치로 생성
        for (int i = 0; i < initialCount; i++)
        {
            CreateBackground(i * backgroundWidth);
        }
    }

    void Update()
    {
        if (player != null && backgroundList.Count > 0)
        {
            float lastBgX = backgroundList[backgroundList.Count - 1].transform.position.x;
            float firstBgX = backgroundList[0].transform.position.x;

            // 플레이어가 마지막 Ground의 X위치 - (offsetX의 1.5배) 이상 이동하면 새 Ground 생성
            if (player.position.x > lastBgX - backgroundWidth * 1.5f)
            {
                CreateBackground(lastBgX + backgroundWidth);
            }

            // 플레이어가 첫 번째 Ground의 X위치를 넘어서면 삭제
            if (player.position.x > firstBgX + backgroundWidth * 1.5f)
            {
                DestroyOldestBackground();
            }
        }
    }

    void CreateBackground(float xPos)
    {
        GameObject bg = Instantiate(backgroundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity, backgroundParent);
        backgroundList.Add(bg);
    }

    void DestroyOldestBackground()
    {
        Destroy(backgroundList[0]);
        backgroundList.RemoveAt(0);
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}
