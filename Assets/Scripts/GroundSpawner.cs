using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab; // 생성할 Ground 프리팹
    public Transform groundParent; // Ground를 저장할 Grid 오브젝트
    public int count = 0; // 처음 생성할 Ground 개수
    public float offsetX = 0f; // Ground X축 간격
    public Transform player; // 플레이어
    public List<GameObject> groundList = new List<GameObject>(); // 생성된 Ground 오브젝트를 저장할 리스트

    void Start()
    {
        // 처음에 count만큼 Y축에 간격을 두고 Ground 생성
        for (int i = 0; i < count; i++)
        {
            CreateGround(i * offsetX);
        }
    }

    void Update()
    {
        if (player != null && groundList.Count > 0) // 플레이어가 존재하고 Ground가 1개 이상이라면
        {
            float lastGroundX = groundList[groundList.Count - 1].transform.position.x; // 마지막 Ground의 X위치
            float firstGroundX = groundList[0].transform.position.x; // 첫 번째 Ground의 X위치
            // Y축은 0, 2 중 랜덤으로 선택
            offsetX = Random.Range(0, 2) * 2;

            // 플레이어가 마지막 Ground의 X위치 - (offsetX의 1.5배) 이상 이동하면 새 Ground 생성
            if (player.position.x > lastGroundX - (offsetX * 0.8f)) 
            {
                CreateGround(lastGroundX + offsetX);
            }

            // 플레이어가 첫 번째 Ground의 X위치를 넘어서면 삭제
            if (player.position.x > firstGroundX + (offsetX * 0.8f))
            {
                DestroyOldestGround();
            }
        }
    }

    void CreateGround(float xPos)
    {
        GameObject ground = Instantiate(groundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity, groundParent); // Ground 생성
        groundList.Add(ground); // 생성한 Ground를 리스트에 추가
    }

    void DestroyOldestGround()
    {
        Destroy(groundList[0]); // 가장 오래된 Ground 삭제
        groundList.RemoveAt(0); // 리스트에서 삭제
    }
}