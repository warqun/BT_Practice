using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace OriginProject.Mob
{
    public class PlayerTag : MonoBehaviour
    {
        public float3 Position;

        private void Update()
        {
            Position = transform.position;
        }

        class Baker : Baker<PlayerTag>
        {
            public override void Bake(PlayerTag authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerTagComponent
                {
                    Position = authoring.Position
                });
            }
        }
    }

    public struct PlayerTagComponent : IComponentData
    {
        public float3 Position;
    }
} 