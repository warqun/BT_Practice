using UnityEngine;
using Unity.Entities;

public class SeparationAuthoring : MonoBehaviour
{
    public float separationRadius = 1.5f;
    public float separationStrength = 0.2f;

    class Baker : Baker<SeparationAuthoring>
    {
        public override void Bake(SeparationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic); // 이동 필요하므로 Dynamic
            AddComponent(entity, new SeparationComponent
            {
                separationRadius = authoring.separationRadius,
                separationStrength = authoring.separationStrength
            });
        }
    }
}
