using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchieveUI : MonoBehaviour
{
    [Header("업적 팝업 설정")]
    [SerializeField] private GameObject achievePopup;     // 업적 팝업 오브젝트
    [SerializeField] private TextMeshProUGUI titleText;   // 업적 제목 텍스트
    [SerializeField] private TextMeshProUGUI descText;    // 업적 설명 텍스트
    [SerializeField] private Image iconImage;             // 업적 아이콘 (선택적)
    
    [Header("팝업 애니메이션 설정")]
    [SerializeField] private float showDuration = 3f;     // 팝업 표시 지속 시간
    [SerializeField] private float slideInTime = 0.5f;    // 왼쪽으로 슬라이드 인 시간
    [SerializeField] private float slideOutTime = 0.5f;   // 오른쪽으로 슬라이드 아웃 시간

    [Header("사운드 설정")]
    [SerializeField] private AudioClip achievementClip;   // 업적 달성 사운드
    
    private RectTransform popupRect;                      // 팝업의 RectTransform
    private Vector2 hiddenPosition;                       // 숨겨진 위치 (화면 오른쪽 바깥)
    private Vector2 visiblePosition;                      // 보이는 위치 (화면 내부)
    private AudioSource audioSource;                      // 오디오 소스 컴포넌트
    
    private void Awake()
    {
        // RectTransform 가져오기
        if (achievePopup != null)
        {
            popupRect = achievePopup.GetComponent<RectTransform>();
            achievePopup.SetActive(false);
        }

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    private void Start()
    {
        Time.timeScale = 1.0f;
        
        // 위치 초기화
        if (popupRect != null)
        {
            // 보이는 위치: 화면 오른쪽 가장자리에서 약간 왼쪽
            visiblePosition = new Vector2(-20f, popupRect.anchoredPosition.y);
            
            // 숨겨진 위치: 화면 오른쪽 바깥
            float width = popupRect.rect.width;
            hiddenPosition = new Vector2(width + 50f, popupRect.anchoredPosition.y);
            
            // 초기 위치 설정
            popupRect.anchoredPosition = hiddenPosition;
        }
    }
    
    public void ShowAchievement(string title, string description, Sprite icon = null)
    {
        StopAllCoroutines();
        
        // 텍스트 및 아이콘 설정
        if (titleText != null) titleText.text = title;
        if (descText != null) descText.text = description;
        
        if (iconImage != null && icon != null)
        {
            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }
        
        // 팝업 애니메이션 시작
        StartCoroutine(ShowPopupAnimation());

        // 사운드 재생 (null 체크 추가)
        if (audioSource != null && achievementClip != null)
        {
            audioSource.PlayOneShot(achievementClip);
        }
    }
    
    private IEnumerator ShowPopupAnimation()
    {
        // 팝업 활성화 및 초기 위치 설정
        achievePopup.SetActive(true);
        popupRect.anchoredPosition = hiddenPosition;
        
        // 슬라이드 인 애니메이션
        float elapsedTime = 0f;
        while (elapsedTime < slideInTime)
        {
            float t = elapsedTime / slideInTime;
            popupRect.anchoredPosition = Vector2.Lerp(hiddenPosition, visiblePosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        popupRect.anchoredPosition = visiblePosition;
        
        yield return new WaitForSeconds(showDuration);
        
        // 슬라이드 아웃 애니메이션
        elapsedTime = 0f;
        while (elapsedTime < slideOutTime)
        {
            float t = elapsedTime / slideOutTime;
            popupRect.anchoredPosition = Vector2.Lerp(visiblePosition, hiddenPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        popupRect.anchoredPosition = hiddenPosition;
        achievePopup.SetActive(false);
    }
}