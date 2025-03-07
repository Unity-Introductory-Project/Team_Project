using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform player;
    public float playerSpeed = 5f;
    public float initialSpwanInterval = 5f;
    public float spawnInterval;
    public float minSpawnInterval = 0.5f;
    public float spawnDistance = 10f;
    public float spawnIntervalDecreaseRate = 0.05f;
    private bool isPlayerSet = false;


    void Start()
    {
        spawnInterval = initialSpwanInterval; // 초기 값 설정
        StartCoroutine(AdjustSpawnRate());
        InvokeRepeating("PrepareArrow", 1f, spawnInterval);
    }

   
    void PrepareArrow()
    {
        if (!isPlayerSet || player == null) return;
        Vector3 spawnPosition = GetValidSpawnPoint();
        Vector3 targetPosition = player.position;

        // 0.5초 후 화살 생성 (경로를 미리 보여준 후)
        StartCoroutine(SpawnArrowDelayed(spawnPosition, targetPosition, 0.5f));
    }

    IEnumerator SpawnArrowDelayed(Vector3 spawnPosition, Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (player == null) yield break;

        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetTarget(targetPosition);   
    }

    Vector3 GetValidSpawnPoint()
    {
        if (player == null) return Vector3.zero;

        int maxAttempts = 5; // Ground 위에 생성되지 않도록 여러 번 시도
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 position = GetRandomSpawnPos();
            if (!isGroundAtPos(position)) return position; // Ground가 없으면 유효한 위치로 사용
        }
        return Vector3.zero; // 모든 시도가 실패하면 생성하지 않음
    }
    
    Vector3 GetRandomSpawnPos()
    {
        if (player == null) return Vector3.zero;
        Vector3 position = Vector3.zero;

        // 카메라의 오른쪽 끝과 위쪽 끝 가져오기
        float cameraRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x; // 카메라 오른쪽 경계

        float spawnOffsetX = playerSpeed; // 화면 오른쪽 밖에서 등장할 거리

        Collider2D playerCollider = player.GetComponent<Collider2D>();

        float playerBottom = playerCollider.bounds.min.y;
        float playerTop = playerCollider.bounds.max.y;
        
        float spawnMinY = playerBottom;
        float spawnMaxY = playerTop + 1f;

        float spawnY = Random.Range(spawnMinY, spawnMaxY);


        return new Vector3(cameraRight + spawnOffsetX, spawnY, 0);

    }

    private bool isGroundAtPos(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.1f);

        return hit != null && hit.CompareTag("Ground");
    }

    public void SetPlayerTarget(Transform newPlayer)
    {
        player = newPlayer;
        isPlayerSet = true;
    }

    /// <summary>
    /// 시간이 지날수록 화살 생성 속도를 점점 빠르게 하는 로직
    /// </summary>
    IEnumerator AdjustSpawnRate()
    {
        while (spawnInterval > minSpawnInterval)
        {
            yield return new WaitForSeconds(10f); // 10초마다 난이도 증가
            spawnInterval -= spawnIntervalDecreaseRate; // 점점 빠르게 생성
            spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval); // 최소 간격 제한

            // 새롭게 `InvokeRepeating()`을 업데이트 (기존 반복 실행 취소 후 재시작)
            CancelInvoke("PrepareArrow");
            InvokeRepeating("PrepareArrow", 0f, spawnInterval);
        }
    }
}

