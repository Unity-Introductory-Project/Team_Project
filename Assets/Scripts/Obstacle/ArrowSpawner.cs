using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform player;
    public float playerSpeed = 5f;
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

        // 0.5�� �� ȭ�� ���� (��θ� �̸� ������ ��)
        StartCoroutine(SpawnArrowDelayed(spawnPosition, targetPosition, 0.5f));
    }

    IEnumerator SpawnArrowDelayed(Vector3 spawnPosition, Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ȭ�� ����
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetTarget(targetPosition);   
    }
    
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 position = Vector3.zero;

        // ī�޶��� ������ ���� ���� �� ��������
        float cameraRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x; // ī�޶� ������ ���

        float spawnOffsetX = playerSpeed; // ȭ�� ������ �ۿ��� ������ �Ÿ�

        Collider2D playerCollider = player.GetComponent<Collider2D>();

        float playerBottom = playerCollider.bounds.min.y;
        float playerTop = playerCollider.bounds.max.y;
        
        float spawnMinY = playerBottom;
        float spawnMaxY = playerTop + 1f;

        float spawnY = Random.Range(spawnMinY, spawnMaxY);


        return new Vector3(cameraRight + spawnOffsetX, spawnY, 0);

    }
    public void SetPlayerTarget(Transform newPlayer)
    {
        player = newPlayer;
    }
}
