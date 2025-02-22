using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 따라갈 플레이어
    public float offsetX = 5f; // 플레이어와의 X축 거리

    void Update()
    {
        if (player != null)
        {
            // 카메라를 플레이어 X축에 맞춰 이동
            transform.position = new Vector3(player.position.x + offsetX, transform.position.y, transform.position.z);
        }
    }
}