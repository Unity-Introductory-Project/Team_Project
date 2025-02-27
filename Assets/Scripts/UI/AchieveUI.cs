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
    [SerializeField] private float soundVolume = 0.3f;    // 업적 사운드 볼륨 (0.0 ~ 1.0)
    
    // 항상 표시할 업적 제목 목록 (생존 시간 관련 업적)
    private readonly string[] alwaysShowAchievements = {
        "초보 생존자", 
        "끈질긴 바퀴벌레"
    };
    
    private RectTransform popupRect;                      // 팝업의 RectTransform
    private Vector2 hiddenPosition;                       // 숨겨진 위치 (화면 오른쪽 바깥)
    private Vector2 visiblePosition;                      // 보이는 위치 (화면 내부)
    private AudioSource audioSource;                      // 오디오 소스 컴포넌트
    
    // 이미 표시된 업적을 추적하기 위한 HashSet
    private HashSet<string> displayedAchievements = new HashSet<string>();
    
    // 이번 게임 세션에서 달성한 업적 추적용
    private HashSet<string> sessionAchievements = new HashSet<string>();
    
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
        
        // 표시된 업적 로드
        LoadDisplayedAchievements();
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
    
    /// <summary>
    /// 항상 표시해야 하는 업적인지 확인
    /// </summary>
    private bool IsAlwaysShowAchievement(string title)
    {
        foreach (string achievementTitle in alwaysShowAchievements)
        {
            if (title == achievementTitle)
                return true;
        }
        return false;
    }
    
    /// <summary>
    /// 업적 달성 팝업 표시
    /// <summary>
    public void ShowAchievement(string title, string description, Sprite icon = null)
    {
        // 항상 표시하는 업적은 별도 처리
        bool isAlwaysShow = IsAlwaysShowAchievement(title);
        
        // 현재 세션에서 이미 표시했다면 중복 표시 방지 (항상 표시 업적 제외)
        if (!isAlwaysShow && sessionAchievements.Contains(title))
        {
            return;
        }
        
        // 이미 영구적으로 표시된 업적이고, 현재 세션에서 아직 표시되지 않았다면 (항상 표시 업적 제외)
        if (!isAlwaysShow && displayedAchievements.Contains(title) && !sessionAchievements.Contains(title))
        {
            sessionAchievements.Add(title);
            return;
        }
        
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

        // 사운드 재생 (볼륨 조정 및 null 체크 추가)
        if (audioSource != null && achievementClip != null)
        {
            audioSource.PlayOneShot(achievementClip, soundVolume);
        }
        
        // 현재 세션에 표시된 업적으로 기록
        sessionAchievements.Add(title);
        
        // 영구 표시된 업적 목록에 추가 (항상 표시 업적은 저장하지 않음)
        if (!isAlwaysShow && !displayedAchievements.Contains(title))
        {
            displayedAchievements.Add(title);
            // 영구 목록 저장
            SaveDisplayedAchievements();
        }
    }
    
    /// <summary>
    /// 팝업 애니메이션 코루틴
    /// <summary>
    private IEnumerator ShowPopupAnimation()
    {
        // 팝업 활성화 및 초기 위치 설정
        achievePopup.SetActive(true);
        popupRect.anchoredPosition = hiddenPosition;
        
        // 슬라이드 인 애니메이션
        float elapsedTime = 0f;
        while (elapsedTime < slideInTime)
        {
            float t = elapsedTime / slideInTime; // 0~1 사이 값
            popupRect.anchoredPosition = Vector2.Lerp(hiddenPosition, visiblePosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // 최종 위치 설정
        popupRect.anchoredPosition = visiblePosition;
        
        // 표시 지속 시간 만큼 대기
        yield return new WaitForSeconds(showDuration);
        
        // 슬라이드 아웃 애니메이션
        elapsedTime = 0f;
        while (elapsedTime < slideOutTime)
        {
            float t = elapsedTime / slideOutTime; // 0~1 사이 값
            popupRect.anchoredPosition = Vector2.Lerp(visiblePosition, hiddenPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // 최종 위치 설정 및 비활성화화
        popupRect.anchoredPosition = hiddenPosition;
        achievePopup.SetActive(false);
    }
    
    /// <summary>
    /// 표시된 업적 목록 저장
    /// </summary>
    private void SaveDisplayedAchievements()
    {
        // 업적 수 저장
        PlayerPrefs.SetInt("DisplayedAchievements_Count", displayedAchievements.Count);
        
        // 각 업적 제목 저장
        int index = 0;
        foreach (string title in displayedAchievements)
        {
            PlayerPrefs.SetString("DisplayedAchievement_" + index, title);
            index++;
        }
        
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 표시된 업적 목록 로드
    /// </summary>
    private void LoadDisplayedAchievements()
    {
        displayedAchievements.Clear();
        
        int count = PlayerPrefs.GetInt("DisplayedAchievements_Count", 0);
        
        for (int i = 0; i < count; i++)
        {
            string title = PlayerPrefs.GetString("DisplayedAchievement_" + i, "");
            if (!string.IsNullOrEmpty(title))
            {
                displayedAchievements.Add(title);
            }
        }
    }
    
    /// <summary>
    /// 모든 표시된 업적 기록 초기화
    /// </summary>
    public void ResetAllAchievements()
    {
        displayedAchievements.Clear();
        sessionAchievements.Clear();
        
        // PlayerPrefs에서 업적 관련 키 모두 삭제
        PlayerPrefs.DeleteKey("DisplayedAchievements_Count");
        for (int i = 0; i < 50; i++)  // 충분히 큰 숫자로 설정
        {
            PlayerPrefs.DeleteKey("DisplayedAchievement_" + i);
        }
        PlayerPrefs.Save();
    }
}