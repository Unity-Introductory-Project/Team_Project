using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    public Button[] characterButtons;
    private void Start()
    {
        // 버튼에 클릭 이벤트 추가
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i; // 람다 캡처 문제 해결을 위한 지역 변수 사용
            characterButtons[i].onClick.AddListener(() => OnCharacterSelect(index));

        }
    }

    private void OnCharacterSelect(int index)
    {
        Debug.Log($"캐릭터 {index} 선택됨");
        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.ChangeCharacter(index); // 캐릭터 생성
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CharacterSelected(); // 게임 시작
        }

        gameObject.SetActive(false);
    }
}

