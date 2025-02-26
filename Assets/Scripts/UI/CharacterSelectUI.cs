using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    public Button[] characterButtons;
    private void Start()
    {
        // ��ư�� Ŭ�� �̺�Ʈ �߰�
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i; // ���� ĸó ���� �ذ��� ���� ���� ���� ���
            characterButtons[i].onClick.AddListener(() => OnCharacterSelect(index));
        }
    }

    private void OnCharacterSelect(int index)
    {
        Debug.Log($"ĳ���� {index} ���õ�");
        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.ChangeCharacter(index); // ĳ���� ����
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CharacterSelected(); // ���� ����
        }

        gameObject.SetActive(false);
    }
}
