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
    
    [Header("Darkness Settings")]
    [Range(0f, 1f)]
    public float initialDarkness = 0f; // 초기 어두움 정도 (0: 원래색, 1: 완전히 어두움)
    [Range(0f, 0.1f)]
    public float darknessIncrement = 0.05f; // 배경을 생성할 때마다 증가할 어두움 정도
    [Range(0f, 1f)]
    public float maxDarkness = 0.7f; // 최대 어두움 정도
    
    private float currentDarkness; // 현재 어두움 정도

    void Start()
    {
        currentDarkness = initialDarkness;
        
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
        
        // 배경의 모든 자식 스프라이트 렌더러를 찾아서 어둡게 만듦
        ApplyDarknessToSprites(bg);
        
        // 다음 배경은 더 어둡게 생성
        currentDarkness = Mathf.Min(currentDarkness + darknessIncrement, maxDarkness);
    }

    void ApplyDarknessToSprites(GameObject background)
    {
        // 배경 자신과 모든 자식들의 SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer[] spriteRenderers = background.GetComponentsInChildren<SpriteRenderer>(true);
        
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color originalColor = sr.color;
            // 색상을 어둡게 조절 (RGB 값을 낮춤)
            sr.color = new Color(
                originalColor.r * (1 - currentDarkness),
                originalColor.g * (1 - currentDarkness),
                originalColor.b * (1 - currentDarkness),
                originalColor.a // 알파 값은 유지
            );
        }
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