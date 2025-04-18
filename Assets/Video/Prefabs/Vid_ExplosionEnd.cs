using UnityEngine;

public class Vid_ExplosionEnd : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float endtime = 0.0f; // Ÿ�̸� ����

    [SerializeField]
    public float lifetime = 0.2f; // Ÿ�̸� ����

    void Awake()
    {
        endtime = Time.time + lifetime; // ���� �ð��� �����ֱ⸦ ���Ͽ� ���� �ð��� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > endtime) // ���� �ð��� ���� �ð����� ũ��
        {
            Destroy(gameObject); // ���� ������Ʈ�� �ı�
        }
    }
}
