using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D를 사용하기 위한 네임스페이스

public class GlobalLightDimmer : MonoBehaviour
{
    public Light2D globalLight; // URP 2D Light (Global Light)
    public float duration = 0f; // 빛이 감소하는 총 시간 (초)
    public float minIntensity = 0f; // 최소 밝기 값
    private float maxIntensity; // 초기 밝기 값
    private float timer = 0f;

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>(); // 현재 오브젝트의 Light2D 가져오기
        }

        if (globalLight != null)
        {
            maxIntensity = globalLight.intensity; // 시작할 때 초기 밝기 저장
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

        // 시간에 따라 밝기 감소 (Lerp 사용)
        globalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, timer / duration);
    }
}