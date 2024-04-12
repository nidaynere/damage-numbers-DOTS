using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ErenAydin.DamageNumbers
{
    [BurstCompile]
    internal partial struct SpawnDamageTextJob : IJobEntity
    {
        [ReadOnly] public int MaxDamageTexts;

        public EntityCommandBuffer ecb;

        [BurstCompile]
        public void Execute (ref DamageTextInitializerComponent initializer)
        {
            if (initializer.isInitialized == 1)
            {
                return;
            }

            initializer.isInitialized = 1;

            var instances = new NativeArray<Entity>(MaxDamageTexts, Allocator.Temp);
            ecb.Instantiate(initializer.damageTextPrefab, instances);
        }
    }
}