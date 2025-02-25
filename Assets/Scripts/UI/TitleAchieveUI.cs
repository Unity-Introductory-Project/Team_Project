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
        layoutGroup.childControlHeight = false; // 높이는 직접 제어
        
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
        
        // RectTransform 설정
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 170f);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        // 배경 이미지 설정
        Image itemBg = item.GetComponent<Image>();
        if (itemBg != null)
        {
            Color color = itemBg.color;
            color.a = 1f;
            itemBg.color = color;
            
            if (achievement.unlocked)
            {
                itemBg.color = new Color(0.9f, 1f, 0.9f, 1f);
            }
        }
        
        // UI 요소 참조 가져오기
        TextMeshProUGUI titleText = item.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descText = item.transform.Find("DescriptionText")?.GetComponent<TextMeshProUGUI>();
        Image iconImage = item.transform.Find("Icon")?.GetComponent<Image>();
        
        // 정보 설정
        if (titleText != null)
        {
            titleText.text = achievement.title;
            if (achievement.unlocked)
            {
                titleText.color = new Color(0.2f, 0.8f, 0.2f);
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
        
        // 이제 ProgressBar를 직접 찾아서 처리
        Transform progressBarTr = item.transform.Find("ProgressBar");
        
        if (progressBarTr != null)
        {
            // ProgressBar에 슬라이더가 없는 것 같으므로 직접 Fill 이미지 조정
            Transform fillArea = progressBarTr.Find("Fill Area");
            if (fillArea != null)
            {
                Transform fill = fillArea.Find("Fill");
                if (fill != null)
                {
                    Image fillImage = fill.GetComponent<Image>();
                    if (fillImage != null)
                    {
                        // Fill 이미지 색상 설정
                        fillImage.color = achievement.unlocked 
                            ? new Color(0.2f, 0.8f, 0.2f) // 달성: 녹색
                            : new Color(0.0f, 0.6f, 1.0f); // 진행중: 파란색
                    }
                    
                    // Fill RectTransform 직접 조정 (여기가 핵심)
                    RectTransform fillRect = fill.GetComponent<RectTransform>();
                    if (fillRect != null)
                    {
                        // X 앵커 값을 진행도에 맞게 설정 (이 부분이 프로그레스바 표시를 제어)
                        Vector2 anchorMax = fillRect.anchorMax;
                        anchorMax.x = progress;
                        fillRect.anchorMax = anchorMax;
                    }
                }
            }
            
            // 만약 슬라이더 컴포넌트를 추가하고 싶다면:
            /*
            Slider slider = progressBarTr.gameObject.GetComponent<Slider>();
            if (slider == null)
            {
                slider = progressBarTr.gameObject.AddComponent<Slider>();
                slider.direction = Slider.Direction.LeftToRight;
                slider.minValue = 0f;
                slider.maxValue = 1f;
                
                // Fill Rect 설정
                Transform fillArea = progressBarTr.Find("Fill Area");
                if (fillArea != null)
                {
                    Transform fill = fillArea.Find("Fill");
                    if (fill != null)
                    {
                        slider.fillRect = fill.GetComponent<RectTransform>();
                    }
                }
                
                // Background 설정
                Transform background = progressBarTr.Find("Background");
                if (background != null)
                {
                    slider.targetGraphic = background.GetComponent<Image>();
                }
            }
            
            slider.value = progress;
            */
        }
        
        // 진행 텍스트 업데이트
        TextMeshProUGUI progressText = item.transform.Find("ProgressText")?.GetComponent<TextMeshProUGUI>();
        if (progressText != null)
        {
            try
            {
                if (achievement.unlocked)
                    progressText.text = "Complete! (100%)";
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