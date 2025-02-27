using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(3f, 1f, -10f);

    private float initialY;

    private void Start()
    {
        if (player != null)
        {
            initialY = player.position.y + offset.y; // 시작 Y 위치를 설정
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = new Vector3(player.position.x + offset.x, initialY, player.position.z + offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newPlayer)
    {
        player = newPlayer;
        initialY = player.position.y + offset.y; // 시작 Y 위치 설정
    }

}

