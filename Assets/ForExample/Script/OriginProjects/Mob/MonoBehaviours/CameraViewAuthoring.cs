using Unity.Entities;
using UnityEngine;

namespace OriginProject.Mob
{
    public class CameraViewAuthoring : MonoBehaviour
    {
        public float viewDistance = 30f;
        public float fieldOfView = 60f;

        class Baker : Baker<CameraViewAuthoring>
        {
            public override void Bake(CameraViewAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var camera = authoring.GetComponent<Camera>();
                
                AddComponent(entity, new CameraViewComponent
                {
                    position = authoring.transform.position,
                    forward = authoring.transform.forward,
                    fieldOfView = authoring.fieldOfView,
                    viewDistance = authoring.viewDistance,
                    aspectRatio = camera.aspect
                });
            }
        }

        private void Update()
        {
            if (World.DefaultGameObjectInjectionWorld != null)
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var query = entityManager.CreateEntityQuery(typeof(CameraViewComponent));
                
                if (query.CalculateEntityCount() > 0)
                {
                    var entity = query.GetSingletonEntity();
                    entityManager.SetComponentData(entity, new CameraViewComponent
                    {
                        position = transform.position,
                        forward = transform.forward,
                        fieldOfView = fieldOfView,
                        viewDistance = viewDistance,
                        aspectRatio = GetComponent<Camera>().aspect
                    });
                }
            }
        }
    }
} 