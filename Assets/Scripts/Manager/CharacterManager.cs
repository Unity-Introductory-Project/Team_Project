using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance; // 싱글턴 패턴 적용

    public GameObject[] characterPrefabs; // 캐릭터 프리팹 리스트
    public Transform spawnPoint; // 생성 위치
    public GameObject currentPlayer; // 현재 캐릭터 오브젝트

    public PlayerCameraFollow cameraFollow; // 카메라
    public ArrowSpawner arrowSpawner; // 화살 생성기
    public GroundSpawner groundSpawner;
    public BackgroundSpawner BGSpawner;
    
    private void Awake()
    {
        // 싱글턴 적용 (게임에서 단 하나의 인스턴스만 유지)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);   
    }


    public void ChangeCharacter(int index)
    {
        if(!CanSelectCharacter(index)) return;


        if (index < 0 || index >= characterPrefabs.Length)
        {
            Debug.LogError("잘못된 캐릭터 인덱스입니다.");
            return;
        }

        // 기존 캐릭터 삭제
        if (currentPlayer != null) Destroy(currentPlayer);

        // 새로운 캐릭터 생성
        currentPlayer = Instantiate(characterPrefabs[index], spawnPoint.position, Quaternion.identity);
        Debug.Log($"{characterPrefabs[index].name} 캐릭터 생성 완료!");

        // 카메라 타겟 변경
        if (cameraFollow != null) cameraFollow.SetTarget(currentPlayer.transform);

        // 화살 스포너 타겟 변경
        if (arrowSpawner != null) arrowSpawner.SetPlayerTarget(currentPlayer.transform);
        if(groundSpawner != null) groundSpawner.SetPlayer(currentPlayer.transform);
        if(BGSpawner != null) BGSpawner.SetPlayer(currentPlayer.transform);
        if(GameManager.Instance != null) GameManager.Instance.SetPlayer(currentPlayer.GetComponent<CharacterBase>());
    }

    /// <summary>
    /// 특정 캐릭터를 선택할 수 있는지 확인
    /// </summary>
    public bool CanSelectCharacter(int index)
    {
        if (AchieveManager.Instance == null) return false;

        switch (characterPrefabs[index].name)
        {
            case "Player_Warrior":
                return true; // 기본 캐릭터는 무조건 선택 가능

            case "Player_Girl":
                return AchieveManager.Instance.IsAchievementUnlocked("jump_20"); // 점프 업적 해금 시 선택 가능

            case "Player_Knight":
                return AchieveManager.Instance.IsAchievementUnlocked("apple_50"); // 사과 업적 해금 시 선택 가능

            case "Player_Boy":
                return AchieveManager.Instance.IsAchievementUnlocked("death_3"); // 사망 업적 해금 시 선택 가능

            default:
                return false;
        }
    }
}


