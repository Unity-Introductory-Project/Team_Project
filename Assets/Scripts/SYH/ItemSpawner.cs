using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ApplePrefab; // 일반 사과 프리팹
    public GameObject PoisonApplePrefab; // 독사과 프리팹
    public GameObject DashItemPrefab; // 달리기 아이템 프리팹
    public Transform ItemParent; // 사과를 저장할 부모 오브젝트
    public int ApplePerGround = 0; // Ground당 생성할 사과 개수
    public float AppleSpacing = 0f; // 사과 간격 (일정하게 유지)
    public float PoisonAppleChance = 0f; // 독사과 생성 확률 (20%)
    public float RunItemChance = 0f; // 달리기 아이템 생성 확률 (2%)

    private List<GameObject> AppleList = new List<GameObject>(); // 생성된 사과 리스트

    public void SpawnApples(Vector3 groundPosition, float groundWidth)
    {
        float startX = groundPosition.x - (groundWidth / 2) + AppleSpacing / 2; // 사과 시작 위치
        float yPos = groundPosition.y - 2.5f; // Ground 바로 위에 생성되도록 설정

        for (int i = 0; i < ApplePerGround; i++)
        {
            float xPos = startX + (i * AppleSpacing); // 일정한 간격으로 배치
            Vector3 ApplePosition = new Vector3(xPos, yPos, 0);

            // 20% 확률로 독사과, 나머지는 일반 사과 생성
            GameObject Item;
            if (Random.value < PoisonAppleChance)
            {
                Item = Instantiate(PoisonApplePrefab, ApplePosition, Quaternion.identity, ItemParent);
            }
            else if (Random.value < RunItemChance)
            {
                Item = Instantiate(DashItemPrefab, ApplePosition, Quaternion.identity, ItemParent);
            }
            else
            {
                Item = Instantiate(ApplePrefab, ApplePosition, Quaternion.identity, ItemParent);
            }
            AppleList.Add(Item);
        }
    }
}