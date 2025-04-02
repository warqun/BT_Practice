using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 areaCenter;
    public Vector3 areaSize = new Vector3(10, 0, 10);
    public float spawnInterval = 2f;
    public float fixedY = 0f; // Y�� ������
    // public ScriptableObject moveData; // ���� Ȯ���

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + areaCenter, areaSize);
    }

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnerComponent
            {
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Renderable),
                areaCenter = authoring.transform.position + authoring.areaCenter, // ��ġ ���� ����
                areaSize = authoring.areaSize,
                interval = authoring.spawnInterval,
                timer = 0,
                fixedY = authoring.fixedY
            });
        }
    }
}