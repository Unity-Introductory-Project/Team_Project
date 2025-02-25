using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetDirection;
    private LineRenderer lineRenderer;

    public void SetTarget(Vector3 predictedPos)
    {
        targetDirection = (predictedPos - transform.position).normalized;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        ShowTrajectory(predictedPos);
    }
    public void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ground")) // �÷��̾ ���� �浹�ϸ� ����
        {
            if (lineRenderer != null) Destroy(lineRenderer.gameObject);
            Destroy(gameObject);
        }
    }

    private void ShowTrajectory(Vector3 targetPosition)
    {
        // ���ο� LineRenderer�� �����ϰ� ȭ�쿡 �߰�
        GameObject lineObject = new GameObject("ArrowTrajectory");
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, transform.position); // ȭ�� ��� ��ġ
        lineRenderer.SetPosition(1, targetPosition); // ��ǥ ��ġ

        // 0.5�� �� ���� ����
        Destroy(lineObject, 0.5f);
    }
}
