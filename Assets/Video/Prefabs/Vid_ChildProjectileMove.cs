using Unity.VisualScripting;
using UnityEngine;

public class Vid_ChildProjectileMove : MonoBehaviour
{
    float endtime = 0.0f; // 타이머 변수

    [SerializeField]
    public float lifetime = 1.0f; // 타이머 변수
    public float speed = 10f; // 속도
    public bool AllowMultipleCreate = true; // Flag to indicate if multiple spawns are allowed

    void Awake()
    {
        endtime = Time.time + lifetime; // 현재 시간에 생명주기를 더하여 종료 시간을 설정


    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // 오브젝트를 앞으로 이동
        HitCheck(); // 충돌 체크 함수 호출

        if (Time.time > endtime) // 현재 시간이 종료 시간보다 크면
        {
            Destroy(gameObject); // 게임 오브젝트를 파괴
        }
    }

    void HitCheck()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit!");
                Destroy(gameObject);
            }
        }

    }
}