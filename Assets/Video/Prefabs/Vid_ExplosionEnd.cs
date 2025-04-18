using UnityEngine;

public class Vid_ExplosionEnd : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float endtime = 0.0f; // 타이머 변수

    [SerializeField]
    public float lifetime = 0.2f; // 타이머 변수

    void Awake()
    {
        endtime = Time.time + lifetime; // 현재 시간에 생명주기를 더하여 종료 시간을 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > endtime) // 현재 시간이 종료 시간보다 크면
        {
            Destroy(gameObject); // 게임 오브젝트를 파괴
        }
    }
}
