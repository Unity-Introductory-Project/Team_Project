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
        float cameraTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)).y; // ī�޶� ���� ���

        float spawnOffsetX = 2f; // ȭ�� ������ �ۿ��� ������ �Ÿ�
        float spawnMinY = player.position.y; // �ּ� Y ��ġ (�÷��̾� ����)
        float spawnMaxY = cameraTop + 2f; // �ִ� Y ��ġ (ī�޶� ���� + �߰� ����)

        // ����(������) ~ ������ �� 45�� ���̿��� ������ ��ġ���� ����
        float spawnY = Random.Range(spawnMinY, spawnMaxY);
        position = new Vector3(cameraRight + spawnOffsetX, spawnY, 0);

        return position;
    }
}
