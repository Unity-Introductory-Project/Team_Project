using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance; // �̱��� ���� ����

    public GameObject[] characterPrefabs; // ĳ���� ������ ����Ʈ
    public Transform spawnPoint; // ���� ��ġ
    public GameObject currentPlayer; // ���� ĳ���� ������Ʈ

    public PlayerCameraFollow cameraFollow; // ī�޶�
    public ArrowSpawner arrowSpawner; // ȭ�� ������
    public GroundSpawner groundSpawner;
    public BackgroundSpawner BGSpawner;
    private void Awake()
    {
        // �̱��� ���� (���ӿ��� �� �ϳ��� �ν��Ͻ��� ����)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);   
    }


    public void ChangeCharacter(int index)
    {
        if (index < 0 || index >= characterPrefabs.Length)
        {
            Debug.LogError("�߸��� ĳ���� �ε����Դϴ�.");
            return;
        }

        // ���� ĳ���� ����
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // ���ο� ĳ���� ����
        currentPlayer = Instantiate(characterPrefabs[index], spawnPoint.position, Quaternion.identity);
        Debug.Log($"{characterPrefabs[index].name} ĳ���� ���� �Ϸ�!");

        // ī�޶� Ÿ�� ����
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(currentPlayer.transform);
        }

        // ȭ�� ������ Ÿ�� ����
        if (arrowSpawner != null)
        {
            arrowSpawner.SetPlayerTarget(currentPlayer.transform);
        }

        if(groundSpawner != null)
        {
            groundSpawner.SetPlayer(currentPlayer.transform);
        }

        if(BGSpawner != null)
        {
            BGSpawner.SetPlayer(currentPlayer.transform);
        }

        if(GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayer(currentPlayer.GetComponent<CharacterBase>());
        }
    }
}

