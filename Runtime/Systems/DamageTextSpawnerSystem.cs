using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ErenAydin.DamageNumbers 
{
    [BurstCompile]
    public partial struct DamageNumberSpawnerSystem : ISystem
    {
        private const int k_maxDamageTexts = 10000;

        private bool hasEcb;

        private EntityCommandBuffer ecb;

        [BurstCompile]
        public void OnCreate (ref SystemState state)
        {
            state.RequireForUpdate<DamageTextInitializerComponent>();
        }

        [BurstCompile]
        public void OnUpdate (ref SystemState state)
        {
            if (hasEcb)
            {
                ecb.Playback(state.EntityManager);
                ecb.Dispose();
            }

            ecb = new EntityCommandBuffer(Allocator.TempJob);
            state.Dependency = new SpawnDamageTextJob() 
            { 
                ecb = ecb, 
                MaxDamageTexts = k_maxDamageTexts 
            }.
            Schedule(state.Dependency);

            hasEcb = true;
        }
    }
} 
