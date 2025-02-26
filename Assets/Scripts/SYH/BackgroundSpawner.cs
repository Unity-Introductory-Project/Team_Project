using UnityEngine;
using System.Collections.Generic;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject backgroundPrefab; // 諛곌꼍 ?꾨━??
    public Transform backgroundParent; // 諛곌꼍????ν븷 遺紐??ㅻ툕?앺듃
    public Transform player; // ?뚮젅?댁뼱
    public int initialCount = 0; // 泥섏쓬 ?앹꽦??諛곌꼍 媛쒖닔
    public float backgroundWidth = 0f; // 諛곌꼍???덈퉬
    private List<GameObject> backgroundList = new List<GameObject>(); // 諛곌꼍 ???由ъ뒪??

    void Start()
    {
        // 泥섏쓬??諛곌꼍???쇱젙 媛쒖닔留뚰겮 ?앹꽦
        for (int i = 0; i < initialCount; i++)
        {
            CreateBackground(i * backgroundWidth);
        }
    }

    void Update()
    {
        if (player != null && backgroundList.Count > 0)
        {
            float lastBgX = backgroundList[backgroundList.Count - 1].transform.position.x;
            float firstBgX = backgroundList[0].transform.position.x;

            // ?뚮젅?댁뼱媛 留덉?留?諛곌꼍??X?꾩튂 - ?쇱젙 嫄곕━ ?댁긽 ?대룞?섎㈃ ??諛곌꼍 異붽?
            if (player.position.x > lastBgX - backgroundWidth * 1.5f)
            {
                CreateBackground(lastBgX + backgroundWidth);
            }

            // ?뚮젅?댁뼱媛 泥?踰덉㎏ 諛곌꼍???섏뼱?쒕㈃ 媛???ㅻ옒??諛곌꼍 ??젣
            if (player.position.x > firstBgX + backgroundWidth * 1.5f)
            {
                DestroyOldestBackground();
            }
        }
    }

    void CreateBackground(float xPos)
    {
        GameObject bg = Instantiate(backgroundPrefab, new Vector3(xPos, 0, 0), Quaternion.identity, backgroundParent);
        backgroundList.Add(bg);
    }

    void DestroyOldestBackground()
    {
        Destroy(backgroundList[0]);
        backgroundList.RemoveAt(0);
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}
