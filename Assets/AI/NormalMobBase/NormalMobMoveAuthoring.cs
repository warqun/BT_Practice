using Unity.Entities;
using UnityEngine;

public class NormalMobMoveAuthoring : MonoBehaviour
{
    public NormalMobMoveData moveData;

    class Baker : Baker<NormalMobMoveAuthoring>
    {
        public override void Bake(NormalMobMoveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new NormalMobMoveComponent { moveSpeed = authoring.moveData.moveSpeed });
        }
    }
}