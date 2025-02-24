using UnityEngine;
using System.Collections.Generic;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject backgroundPrefab; // 배경 프리팹
    public Transform backgroundParent; // 배경을 저장할 부모 오브젝트
    public Transform player; // 플레이어
    public int initialCount = 0; // 처음 생성할 배경 개수
    public float backgroundWidth = 0f; // 배경의 너비
    private List<GameObject> backgroundList = new List<GameObject>(); // 배경 저장 리스트

    void Start()
    {
        // 처음에 배경을 일정 개수만큼 생성
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

            // 플레이어가 마지막 배경의 X위치 - 일정 거리 이상 이동하면 새 배경 추가
            if (player.position.x > lastBgX - backgroundWidth * 1.5f)
            {
                CreateBackground(lastBgX + backgroundWidth);
            }

            // 플레이어가 첫 번째 배경을 넘어서면 가장 오래된 배경 삭제
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
}