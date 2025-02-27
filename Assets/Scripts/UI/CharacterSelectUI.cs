using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour
{
    public Button[] characterButtons;
    public TextMeshProUGUI[] buttonTexts;
    private void Start()
    {
        UpdateCharacterSelectionUI();
        // 버튼에 클릭 이벤트 추가
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i; // 람다 캡처 문제 해결을 위한 지역 변수 사용
            characterButtons[i].onClick.AddListener(() => OnCharacterSelect(index));

        }
    }

    private void UpdateCharacterSelectionUI()
    {
        if (CharacterManager.Instance == null) return;

        for (int i = 0; i < characterButtons.Length; i++)
        {
            bool isUnlocked = CharacterManager.Instance.CanSelectCharacter(i);
            if (buttonTexts != null && i < buttonTexts.Length)
            {
                if (isUnlocked)
                {
                    buttonTexts[i].text = "Select";
                    buttonTexts[i].color = Color.white;
                }
                else
                {
                    buttonTexts[i].text = "Locked";
                    buttonTexts[i].color = Color.yellow;
                }
            }
            characterButtons[i].interactable = isUnlocked;
        }
    }
    private void OnCharacterSelect(int index)
    {
        if (!CharacterManager.Instance.CanSelectCharacter(index))
        {
            Debug.Log($"{CharacterManager.Instance.characterPrefabs[index].name}은(는) 아직 잠겨 있음!");
            return;
        }

        CharacterManager.Instance.ChangeCharacter(index);
        GameManager.Instance?.CharacterSelected();
        gameObject.SetActive(false);
    }
}

