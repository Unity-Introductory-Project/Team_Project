using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreenObjects : MonoBehaviour
{
    private BoxCollider2D offScreenCollider;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null) return;
        CreateOffScreenColider();
    }

    private void CreateOffScreenColider()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        offScreenCollider = gameObject.AddComponent<BoxCollider2D>();
        offScreenCollider.isTrigger = true;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        offScreenCollider.size = new Vector2(camWidth * 2.5f, camHeight * 2.5f);
    }
    private void Update()
    {
        // 카메라 위치를 따라가도록 설정
        if (mainCamera != null)
        {
            transform.position = mainCamera.transform.position;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CharacterBase player = collision.GetComponent<CharacterBase>();
            if (player != null)
            {
                player.ChangeHp(player.life);
            }
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
