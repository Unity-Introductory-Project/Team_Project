using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal; // Light2D 사용

public class GroundSpawner : MonoBehaviour
{
    public ItemSpawner appleSpawner; // 코인 스포너 참조 추가
    public GameObject groundPrefab; // 생성할 Ground 프리팹
    public Transform groundParent; // Ground를 저장할 Grid 오브젝트
    public int count = 0; // 처음 생성할 Ground 개수
    public float offsetX = 0f; // Ground X축 간격
    public float Hole = 0f; // 밑으로 빠지는 구멍 만들기 위해 추가
    public Transform player; // 플레이어
    public List<GameObject> groundList = new List<GameObject>(); // 생성된 Ground 오브젝트를 저장할 리스트
    private bool isFirstGround = true; // 첫 번째 Ground 여부 체크
    private static float currentIntensity = 0f; // 초기 intensity 값 (전역 변수)

    void Start()
    {
        // 첫 번째 Ground는 Y = 0 고정
        CreateGround(0f, true);
        currentIntensity = 0f; // 게임 다시 시작하면 초기화

        // 이후 Ground는 랜덤한 Y 위치로 생성
        for (int i = 1; i < count; i++)
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

            Hole = Random.Range(3, 5);

            // 플레이어가 마지막 Ground의 X위치 - (offsetX의 1.5배) 이상 이동하면 새 Ground 생성
            if (player.position.x > lastGroundX - (offsetX * 0.8f)) 
            {
                CreateGround(lastGroundX + offsetX + Hole); // 마지막 Ground의 오른쪽에 새 Ground 생성
            }

            // 플레이어가 첫 번째 Ground의 X위치를 넘어서면 삭제
            if (player.position.x > firstGroundX + (offsetX * 0.8f))
            {
                DestroyOldestGround();
            }
        }
    }

    void CreateGround(float xPos, bool isFirstGround = false)
    {
        float yPos = isFirstGround ? 0f : new float[] { 0f, 2f }[Random.Range(0, 2)]; // 첫 번째 Ground는 Y=0 고정

        GameObject ground = Instantiate(groundPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, groundParent);
        groundList.Add(ground);

        // 🔥 모든 자식 Light2D에 적용 (GetComponentsInChildren 사용)
        Light2D[] childLights = ground.GetComponentsInChildren<Light2D>();  
        foreach (Light2D light in childLights)  
        {
            light.intensity = currentIntensity; // 현재 intensity 적용
        }
        
        currentIntensity = Mathf.Min(currentIntensity + 0.1f, 10.0f); // 다음 Ground를 위해 증가 (최대 10 제한)

        if (!isFirstGround && appleSpawner != null)
        {
            appleSpawner.SpawnApples(ground.transform.position, offsetX);
        }

        isFirstGround = false; // 첫 번째 Ground 생성 후 false로 변경
    }

    void DestroyOldestGround()
    {
        Destroy(groundList[0]); // 가장 오래된 Ground 삭제
        groundList.RemoveAt(0); // 리스트에서 삭제
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}