using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// 업적 데이터 클래스
[System.Serializable]
public class AchieveData
{
    public string id;              // 고유 ID
    public string title;           // 제목
    public string description;     // 설명
    public Sprite icon;            // 아이콘 (선택적)
    public AchieveType type;       // 업적 유형
    public int requiredAmount;     // 달성에 필요한 수량
    public bool unlocked;          // 잠금 해제 여부
}

// 업적 유형 열거형
public enum AchieveType
{
    SurviveTime,    // 생존 시간
    AppleCount,     // 사과 수집
    JumpCount,      // 점프 횟수
    SlideCount,     // 슬라이드 횟수
    DeathCount      // 사망 횟수
}

public class AchieveManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static AchieveManager Instance { get; private set; }
    
    [Header("통계 데이터")]
    private float surviveTime;  // 생존 시간
    private int appleCount;     // 사과 수집 수
    private int jumpCount;      // 점프 횟수
    private int slideCount;     // 슬라이드 횟수
    private int deathCount;     // 사망 횟수
    
    [Header("업적 설정")]
    [SerializeField] private List<AchieveData> achievements = new List<AchieveData>();
    
    // UI 관리자 참조
    private AchieveUI achieveUI;
    
    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            LoadStats();
            LoadAchievements();
            
            // 업적 목록이 비어있으면 초기화
            if (achievements.Count == 0)
            {
                InitializeAchievements();
            }

            // 현재 게임의 생존 시간 초기화
            surviveTime = 0;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        // AchieveUI 찾기
        achieveUI = FindFirstObjectByType<AchieveUI>();
    }
    
    private void Update()
    {
        // 게임 진행 중일 때만 생존 시간 증가 (타임스케일이 0이 아닐 때)
        if (Time.timeScale > 0)
        {
            surviveTime += Time.deltaTime;
        }
        
        // 업적 달성 조건 확인
        CheckAchievements();
    }
    
    /// <summary>
    /// 기본 업적 초기화
    /// </summary>
    private void InitializeAchievements()
    {
        // 생존 시간 업적
        achievements.Add(new AchieveData
        {
            id = "survive_60",
            title = "초보 생존자",
            description = "60초 동안 살아남기. 아직 시작일 뿐이다.",
            type = AchieveType.SurviveTime,
            requiredAmount = 60,
            unlocked = false
        });

        achievements.Add(new AchieveData
        {
            id = "survive_180",
            title = "끈질긴 바퀴벌레",
            description = "3분 동안 살아남기. 핵폭발에도 살아남는다는데...",
            type = AchieveType.SurviveTime,
            requiredAmount = 180,
            unlocked = false
        });

        // 사과 수집 업적 (아이폰 패러디)
        achievements.Add(new AchieveData
        {
            id = "apple_50",
            title = "iCollector",
            description = "사과 50개 모으기. 혁신적인 컬렉션.",
            type = AchieveType.AppleCount,
            requiredAmount = 50,
            unlocked = false
        });

        achievements.Add(new AchieveData
        {
            id = "apple_100",
            title = "Genius Bar",
            description = "사과 100개 모으기. 프리미엄 사과만 취급합니다.",
            type = AchieveType.AppleCount,
            requiredAmount = 100,
            unlocked = false
        });

        achievements.Add(new AchieveData
        {
            id = "apple_200",
            title = "Genius Bar",
            description = "사과 200개 모으기. 프리미엄 사과만 취급합니다.",
            type = AchieveType.AppleCount,
            requiredAmount = 200,
            unlocked = false
        });

        // 점프 업적
        achievements.Add(new AchieveData
        {
            id = "jump_20",
            title = "차가운 발바닥",
            description = "20번 점프하기. 바닥이 용암인가요?",
            type = AchieveType.JumpCount,
            requiredAmount = 20,
            unlocked = false
        });

        achievements.Add(new AchieveData
        {
            id = "jump_50",
            title = "중력 부정론자",
            description = "50번 점프하기. 뉴턴이 사과를 보면 화낼 거예요.",
            type = AchieveType.JumpCount,
            requiredAmount = 50,
            unlocked = false
        });

        // 슬라이드 업적
        achievements.Add(new AchieveData
        {
            id = "slide_15",
            title = "미끄럼틀 마스터",
            description = "15번 슬라이드하기. 바지가 닳아 버렸어요.",
            type = AchieveType.SlideCount,
            requiredAmount = 15,
            unlocked = false
        });

        achievements.Add(new AchieveData
        {
            id = "slide_30",
            title = "완벽한 슬라이딩",
            description = "30번 슬라이드하기. 야구 선수들도 부러워할 실력!",
            type = AchieveType.SlideCount,
            requiredAmount = 30,
            unlocked = false
        });

        // 사망 업적
        achievements.Add(new AchieveData
        {
            id = "death_3",
            title = "불사신의 첫 걸음",
            description = "3번 사망 후에도 계속하기. 죽음은 그저 작은 불편함일 뿐.",
            type = AchieveType.DeathCount,
            requiredAmount = 3,
            unlocked = false
        });

        achievements.Add(new AchieveData
        {
            id = "death_9",
            title = "고양이 보스",
            description = "9번 사망 후에도 계속하기. 아홉 개의 목숨을 모두 썼네요!",
            type = AchieveType.DeathCount,
            requiredAmount = 9,
            unlocked = false
        });
        
        // 저장
        SaveAchievements();
    }
    
    /// <summary>
    /// 업적 달성 조건 확인
    /// </summary>
    private void CheckAchievements()
    {
        foreach (AchieveData achievement in achievements)
        {
            // 이미 달성한 업적은 건너뛰기
            if (achievement.unlocked) continue;
            
            bool achieved = false;
            
            // 업적 유형에 따른 조건 확인
            switch (achievement.type)
            {
                case AchieveType.SurviveTime:
                    achieved = surviveTime >= achievement.requiredAmount;
                    break;
                case AchieveType.AppleCount:
                    achieved = appleCount >= achievement.requiredAmount;
                    break;
                case AchieveType.JumpCount:
                    achieved = jumpCount >= achievement.requiredAmount;
                    break;
                case AchieveType.SlideCount:
                    achieved = slideCount >= achievement.requiredAmount;
                    break;
                case AchieveType.DeathCount:
                    achieved = deathCount >= achievement.requiredAmount;
                    break;
            }
            
            // 업적 달성 시 처리
            if (achieved)
            {
                UnlockAchievement(achievement);
            }
        }
    }
    
    /// <summary>
    /// 업적 달성 처리
    /// </summary>
    private void UnlockAchievement(AchieveData achievement)
    {
        // 업적 상태 변경
        achievement.unlocked = true;
        
        // 저장
        SaveAchievements();
        
        // 업적 UI 업데이트 (팝업 표시)
        if (achieveUI != null)
        {
            achieveUI.ShowAchievement(achievement.title, achievement.description, achievement.icon);
        }
        
        // 로그 출력
        Debug.Log("업적 달성: " + achievement.title);
    }
    
    /// <summary>
    /// 통계 및 업적 저장
    /// </summary>
    private void SaveStats()
    {
        PlayerPrefs.SetFloat("Stats_SurviveTime", surviveTime);
        PlayerPrefs.SetInt("Stats_AppleCount", appleCount);
        PlayerPrefs.SetInt("Stats_JumpCount", jumpCount);
        PlayerPrefs.SetInt("Stats_SlideCount", slideCount);
        PlayerPrefs.SetInt("Stats_DeathCount", deathCount);
        PlayerPrefs.Save();
    }
    
    private void SaveAchievements()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            AchieveData achievement = achievements[i];
            PlayerPrefs.SetInt("Achievement_" + achievement.id, achievement.unlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 통계 및 업적 로드
    /// </summary>
    private void LoadStats()
    {
        surviveTime = PlayerPrefs.GetFloat("Stats_SurviveTime", 0);
        appleCount = PlayerPrefs.GetInt("Stats_AppleCount", 0);
        jumpCount = PlayerPrefs.GetInt("Stats_JumpCount", 0);
        slideCount = PlayerPrefs.GetInt("Stats_SlideCount", 0);
        deathCount = PlayerPrefs.GetInt("Stats_DeathCount", 0);
    }
    
    private void LoadAchievements()
    {
        foreach (AchieveData achievement in achievements)
        {
            achievement.unlocked = PlayerPrefs.GetInt("Achievement_" + achievement.id, 0) == 1;
        }
    }
    
    /// <summary>
    /// 앱 종료 시 저장
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveStats();
        SaveAchievements();
    }
    
    /// <summary>
    /// 현재 게임에서의 통계 초기화 (새 게임 시작 시 호출)
    /// </summary>
    public void ResetCurrentGameStats()
    {
        surviveTime = 0;
    }

    /// <summary>
    /// 모든 업적 및 통계 리셋
    /// </summary>
    public void ResetAllAchievements()
    {
        // 통계 초기화
        surviveTime = 0;
        appleCount = 0;
        jumpCount = 0;
        slideCount = 0;
        deathCount = 0;
        
        // 업적 잠금 상태로 초기화
        foreach (AchieveData achievement in achievements)
        {
            achievement.unlocked = false;
        }
        
        // 저장
        SaveStats();
        SaveAchievements();
        
        Debug.Log("모든 업적과 통계가 초기화되었습니다.");
    }
    
    // 외부에서 호출할 수 있는 메서드들
    
    /// <summary>
    /// 사과 획득 시 호출
    /// </summary>
    public void AddApple()
    {
        appleCount++;
        SaveStats();
    }
    
    /// <summary>
    /// 점프 시 호출
    /// </summary>
    public void AddJump()
    {
        jumpCount++;
        SaveStats();
    }
    
    /// <summary>
    /// 슬라이드 시 호출
    /// </summary>
    public void AddSlide()
    {
        slideCount++;
        SaveStats();
    }
    
    /// <summary>
    /// 사망 시 호출
    /// </summary>
    public void AddDeath()
    {
        deathCount++;
        SaveStats();
    }
    
    /// <summary>
    /// 현재 통계 값 가져오기
    /// </summary>
    public float GetSurviveTime() { return surviveTime; }
    public int GetAppleCount() { return appleCount; }
    public int GetJumpCount() { return jumpCount; }
    public int GetSlideCount() { return slideCount; }
    public int GetDeathCount() { return deathCount; }
    
    /// <summary>
    /// 업적 목록 가져오기
    /// </summary>
    public List<AchieveData> GetAchievements()
    {
        return achievements;
    }

    public bool IsAchievementUnlocked(string achievementID)
    {
        foreach (AchieveData achievement in achievements)
        {
            if (achievement.id == achievementID)
            {
                return achievement.unlocked;
            }
        }
        return false;
    }
}