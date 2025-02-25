using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleAchieveUI : MonoBehaviour
{
    [Header("업적 버튼 설정")]
    [SerializeField] private Button achieveButton;

    [Header("업적 패널 설정")]
    [SerializeField] private GameObject achievePanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject achieveItemPrefab;
    
    [Header("확인 창 설정")]
    [SerializeField] private GameObject confirmResetPanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    
    // 간격 설정 (Inspector에서 조정 가능)
    [Header("레이아웃 설정")]
    [SerializeField] private float itemSpacing = 20f; // 아이템 간 간격
    
    private AchieveManager achieveManager;
    private List<GameObject> achieveItems = new List<GameObject>();
    
    private void Awake()
    {
        // 버튼 이벤트 연결
        if (achieveButton != null)
            achieveButton.onClick.AddListener(ShowPanel);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
        
        if (resetButton != null)
            resetButton.onClick.AddListener(ShowResetConfirmation);
        
        if (confirmButton != null)
            confirmButton.onClick.AddListener(ResetAchievements);
        
        if (cancelButton != null)
            cancelButton.onClick.AddListener(HideResetConfirmation);
        
        // 초기 상태 설정
        if (achievePanel != null)
            achievePanel.SetActive(false);
        
        if (confirmResetPanel != null)
            confirmResetPanel.SetActive(false);
    }
    
    private void Start()
    {
        // 업적 매니저 참조 가져오기
        achieveManager = AchieveManager.Instance;
        
        if (achieveManager == null)
        {
            StartCoroutine(TryGetAchieveManager());
        }
    }
    
    private IEnumerator TryGetAchieveManager()
    {
        yield return new WaitForSeconds(0.2f);
        achieveManager = AchieveManager.Instance;
        
        if (achieveManager == null)
        {
            Debug.LogWarning("업적 매니저를 찾을 수 없습니다.");
        }
    }
    
    public void ShowPanel()
    {
        if (achievePanel != null)
        {
            achievePanel.SetActive(true);
            UpdateAchievementList();
        }
    }

    public void ClosePanel()
    {
        if (achievePanel != null)
            achievePanel.SetActive(false);
    }
    
    private void ShowResetConfirmation()
    {
        if (confirmResetPanel != null)
            confirmResetPanel.SetActive(true);
    }
    
    private void HideResetConfirmation()
    {
        if (confirmResetPanel != null)
            confirmResetPanel.SetActive(false);
    }
    
    private void ResetAchievements()
    {
        if (achieveManager != null)
        {
            achieveManager.ResetAllAchievements();
            UpdateAchievementList();
        }
        
        HideResetConfirmation();
    }
    
    private void UpdateAchievementList()
    {
        // 이전에 생성된 항목들 제거
        foreach (GameObject item in achieveItems)
        {
            if (item != null)
                Destroy(item);
        }
        achieveItems.Clear();
        
        // 필수 참조 확인
        if (achieveManager == null)
        {
            achieveManager = AchieveManager.Instance;
            if (achieveManager == null)
            {
                Debug.LogError("업적 매니저가 없습니다!");
                return;
            }
        }

        if (contentTransform == null || achieveItemPrefab == null)
        {
            Debug.LogError("Content Transform 또는 프리팹이 할당되지 않았습니다!");
            return;
        }
        
        // LayoutGroup 설정
        SetupLayoutGroup();
        
        // 업적 목록 가져오기
        List<AchieveData> achievements = achieveManager.GetAchievements();
        if (achievements == null || achievements.Count == 0)
        {
            Debug.LogWarning("업적 목록이 비어있습니다!");
            return;
        }
        
        Debug.Log($"업적 수: {achievements.Count}");
        
        // 각 업적에 대한 UI 항목 생성
        foreach (AchieveData achievement in achievements)
        {
            CreateAchievementItem(achievement);
        }
    }
    
    private void SetupLayoutGroup()
    {
        // 이전 레이아웃 컴포넌트 제거
        VerticalLayoutGroup existingLayout = contentTransform.GetComponent<VerticalLayoutGroup>();
        if (existingLayout != null)
        {
            DestroyImmediate(existingLayout);
        }
        
        ContentSizeFitter existingSizeFitter = contentTransform.GetComponent<ContentSizeFitter>();
        if (existingSizeFitter != null)
        {
            DestroyImmediate(existingSizeFitter);
        }
        
        // 새 레이아웃 컴포넌트 추가
        VerticalLayoutGroup layoutGroup = contentTransform.gameObject.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.UpperCenter;
        layoutGroup.spacing = itemSpacing; // 넓은 간격 적용
        layoutGroup.padding = new RectOffset(10, 10, 20, 20); // 여백 추가
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = false; // 높이는 원본 유지
        
        // ContentSizeFitter 추가
        ContentSizeFitter sizeFitter = contentTransform.gameObject.AddComponent<ContentSizeFitter>();
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
    
    private void CreateAchievementItem(AchieveData achievement)
    {
        if (achievement == null) return;
        
        // 프리팹 인스턴스 생성
        GameObject item = Instantiate(achieveItemPrefab, contentTransform);
        achieveItems.Add(item);
        
        Debug.Log($"업적 생성: {achievement.title}");
        
        // RectTransform 설정 - 원본 높이 유지하면서 레이아웃에 맞게 조정
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 프리팹 원본 높이(303.545) 유지
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 303.545f);
            
            // 배치를 위한 앵커 설정
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);
        }
        
        // 배경 이미지 설정
        Image itemBg = item.GetComponent<Image>();
        if (itemBg != null)
        {
            // 원본 색상 유지하되 불투명도 확보
            Color color = itemBg.color;
            color.a = 1f; // 완전 불투명으로 설정
            itemBg.color = color;
            
            // 달성한 업적은 약간 다른 색상으로 표시
            if (achievement.unlocked)
            {
                itemBg.color = new Color(0.9f, 1f, 0.9f, 1f); // 연한 녹색 배경
            }
        }
        
        // UI 요소 참조 가져오기
        TextMeshProUGUI titleText = item.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descText = item.transform.Find("DescriptionText")?.GetComponent<TextMeshProUGUI>();
        Image iconImage = item.transform.Find("Icon")?.GetComponent<Image>();
        Slider progressSlider = item.transform.Find("ProgressBar")?.GetComponent<Slider>();
        TextMeshProUGUI progressText = item.transform.Find("ProgressText")?.GetComponent<TextMeshProUGUI>();
        
        // 정보 설정
        if (titleText != null)
        {
            titleText.text = achievement.title;
            if (achievement.unlocked)
            {
                titleText.color = new Color(0.2f, 0.8f, 0.2f); // 달성 업적은 녹색 텍스트
            }
        }
        
        if (descText != null)
        {
            descText.text = achievement.description;
        }
        
        if (iconImage != null && achievement.icon != null)
        {
            iconImage.sprite = achievement.icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }
        
        // 진행 상태 계산
        int currentValue = CalculateCurrentValue(achievement);
        float progress = achievement.requiredAmount > 0 
            ? (float)currentValue / achievement.requiredAmount
            : 0f;
        
        // 진행 바 업데이트
        if (progressSlider != null)
        {
            progressSlider.value = progress;
            
            // 달성했을 경우 색상 변경 (선택적)
            Image fillImage = progressSlider.fillRect?.GetComponent<Image>();
            if (fillImage != null && achievement.unlocked)
            {
                fillImage.color = new Color(0.2f, 0.8f, 0.2f); // 녹색으로 변경
            }
        }
        
        // 진행 텍스트 업데이트
        if (progressText != null)
        {
            try
            {
                if (achievement.unlocked)
                    progressText.text = "Complete! (100%)";  // 영어로 표시
                else
                    progressText.text = $"{currentValue} / {achievement.requiredAmount} ({progress * 100:F0}%)";
            }
            catch (System.Exception e)
            {
                Debug.LogError($"텍스트 설정 중 오류 발생: {e.Message}");
            }
        }
    }
    
    private int CalculateCurrentValue(AchieveData achievement)
    {
        if (achieveManager == null) return 0;
        
        switch (achievement.type)
        {
            case AchieveType.SurviveTime:
                return Mathf.Min((int)achieveManager.GetSurviveTime(), achievement.requiredAmount);
            case AchieveType.AppleCount:
                return Mathf.Min(achieveManager.GetAppleCount(), achievement.requiredAmount);
            case AchieveType.JumpCount:
                return Mathf.Min(achieveManager.GetJumpCount(), achievement.requiredAmount);
            case AchieveType.SlideCount:
                return Mathf.Min(achieveManager.GetSlideCount(), achievement.requiredAmount);
            case AchieveType.DeathCount:
                return Mathf.Min(achieveManager.GetDeathCount(), achievement.requiredAmount);
            default:
                return 0;
        }
    }
}