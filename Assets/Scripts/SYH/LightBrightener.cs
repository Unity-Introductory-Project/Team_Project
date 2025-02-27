using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D를 사용하기 위한 네임스페이스

public class LightBrightener : MonoBehaviour
{
    public Light2D globalLight; // URP 2D Light (Global Light)
    public float duration = 60f; // 빛이 증가하는 총 시간 (초)
    public float maxIntensity = 1.0f; // 최대로 밝아질 값
    private float minIntensity; // 초기 밝기 값
    private float timer = 0f;

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>(); // 현재 오브젝트의 Light2D 가져오기
        }

        if (globalLight != null)
        {
            minIntensity = globalLight.intensity; // 시작할 때 초기 밝기 저장
        }
        else
        {
            Debug.LogError("Global Light2D를 찾을 수 없습니다!");
        }
    }

    void Update()
    {
        if (globalLight == null) return;

        timer += Time.deltaTime;

        // 시간에 따라 밝기 증가 (Lerp 사용)
        globalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, timer / duration);
    }
}