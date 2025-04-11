using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Systems;
using Unity.Physics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct SeparationSystem : ISystem
{
    private ComponentLookup<LocalTransform> _transformLookup;

    public void OnCreate(ref SystemState state)
    {
        _transformLookup = state.GetComponentLookup<LocalTransform>(true);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _transformLookup.Update(ref state);

        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
        var currentTime = SystemAPI.Time.ElapsedTime;

        foreach (var (transform, separation, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<SeparationComponent>>().WithEntityAccess())
        {

        }

        commandBuffer.Playback(state.EntityManager);
    }
}