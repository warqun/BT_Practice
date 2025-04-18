using Unity.VisualScripting;
using UnityEngine;

public class Vid_ChildProjectileMove : MonoBehaviour
{
    float endtime = 0.0f; // Ÿ�̸� ����

    [SerializeField]
    public float lifetime = 1.0f; // Ÿ�̸� ����
    public float speed = 10f; // �ӵ�
    public bool AllowMultipleCreate = true; // Flag to indicate if multiple spawns are allowed

    void Awake()
    {
        endtime = Time.time + lifetime; // ���� �ð��� �����ֱ⸦ ���Ͽ� ���� �ð��� ����


    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // ������Ʈ�� ������ �̵�
        HitCheck(); // �浹 üũ �Լ� ȣ��

        if (Time.time > endtime) // ���� �ð��� ���� �ð����� ũ��
        {
            Destroy(gameObject); // ���� ������Ʈ�� �ı�
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