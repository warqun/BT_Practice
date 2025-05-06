using Unity.Entities;
using UnityEngine;

namespace OriginProject.Mob
{
    public class GameBootstrap : MonoBehaviour
    {
        private void Start()
        {
            // 기본 월드 초기화
            var world = World.DefaultGameObjectInjectionWorld;
            
            // 시스템 그룹 생성
            var simulationGroup = world.GetOrCreateSystemManaged<SimulationSystemGroup>();
            
            // 몬스터 시스템 그룹 생성
            var mobSystemGroup = world.GetOrCreateSystemManaged<MobSystemGroup>();
            
            // 시스템 등록
            var spawnSystem = world.GetOrCreateSystemManaged<MobSpawnSystem>();
            var movementSystem = world.GetOrCreateSystemManaged<MobMovementSystem>();
            var attackSystem = world.GetOrCreateSystemManaged<MobAttackSystem>();
            var damageSystem = world.GetOrCreateSystemManaged<MobDamageSystem>();
            
            // 몬스터 시스템 그룹에 시스템 추가
            mobSystemGroup.AddSystemToUpdateList(spawnSystem);
            mobSystemGroup.AddSystemToUpdateList(movementSystem);
            mobSystemGroup.AddSystemToUpdateList(attackSystem);
            mobSystemGroup.AddSystemToUpdateList(damageSystem);
            
            // 몬스터 시스템 그룹을 시뮬레이션 그룹에 추가
            simulationGroup.AddSystemToUpdateList(mobSystemGroup);
            
            // 시스템 순서 최적화
            mobSystemGroup.SortSystems();
            simulationGroup.SortSystems();
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class MobSystemGroup : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }
    }
} 