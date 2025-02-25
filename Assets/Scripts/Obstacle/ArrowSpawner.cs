using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform player;
    public float spawnInterval = 2f;
    public float spawnDistance = 10f;


    void Start()
    {
        InvokeRepeating("PrepareArrow", 1f, spawnInterval);
    }

    void PrepareArrow()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Vector3 targetPosition = player.position;

        // 0.5초 후 화살 생성 (경로를 미리 보여준 후)
        StartCoroutine(SpawnArrowDelayed(spawnPosition, targetPosition, 0.5f));
    }

    IEnumerator SpawnArrowDelayed(Vector3 spawnPosition, Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetTarget(targetPosition);   
    }
    
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 position = Vector3.zero;

        // 카메라의 오른쪽 끝과 위쪽 끝 가져오기
        float cameraRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x; // 카메라 오른쪽 경계
        float cameraTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)).y; // 카메라 위쪽 경계

        float spawnOffsetX = 2f; // 화면 오른쪽 밖에서 등장할 거리
        float spawnMinY = player.position.y; // 최소 Y 위치 (플레이어 높이)
        float spawnMaxY = cameraTop + 2f; // 최대 Y 위치 (카메라 위쪽 + 추가 높이)

        // 정면(오른쪽) ~ 오른쪽 위 45도 사이에서 랜덤한 위치에서 생성
        float spawnY = Random.Range(spawnMinY, spawnMaxY);
        position = new Vector3(cameraRight + spawnOffsetX, spawnY, 0);

        return position;
    }
}
