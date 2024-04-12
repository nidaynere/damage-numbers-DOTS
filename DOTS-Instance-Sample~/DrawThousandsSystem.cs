using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace ErenAydin.DamageNumbers
{
    [BurstCompile]
    public partial struct DrawThousandsSystem : ISystem
    {
        private NativeArray<DamageNumberBuffer> damageTexts;

        private const int k_Range = 90;
        private const int k_Size = 16;

        public void OnCreate (ref SystemState state)
        {
            state.RequireForUpdate<DrawThousandsSampleComponent>();
            damageTexts = new NativeArray<DamageNumberBuffer> (k_Size, Allocator.Persistent);
        }

        public void OnDestroy (ref SystemState state)
        {
            damageTexts.Dispose ();
        }

        public void OnUpdate (ref SystemState state) 
        {
            state.Dependency = new RandomJob()
            {
                 data = damageTexts,
                 random1 = (uint)UnityEngine.Random.Range (0, uint.MaxValue),
                 random2 = (uint)UnityEngine.Random.Range (0, uint.MaxValue),
                 random3 = (uint)UnityEngine.Random.Range (0, uint.MaxValue),
                 random4 = (uint)UnityEngine.Random.Range (0, uint.MaxValue),
            }.Schedule(state.Dependency);

            state.Dependency = new WriteToBuffer() 
            { 
                damageTexts = damageTexts 
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct RandomJob : IJob
        {
            [ReadOnly] public float3 basePosition;
            public uint random1, random2, random3, random4;

            public NativeArray<DamageNumberBuffer> data;

            public void Execute ()
            {
                for (int i = 0, length = data.Length; i < length; i++)
                {
                    var r1 = new Random(random1 + (uint)i * 1000);
                    var r2 = new Random(random2 + (uint)i * 1000);
                    var r3 = new Random(random3 + (uint)i * 1000);
                    var r4 = new Random(random4 + (uint)i * 1000);

                    var color = r1.NextFloat4();
                    color.w = 1;

                    data[i] = new DamageNumberBuffer()
                    {
                        color = color,
                        damageNumber = r2.NextUInt(1, 999999),
                        position = basePosition + r3.NextFloat3() * k_Range,
                        scale = r4.NextFloat(1, 1.5f)
                    };
                }
            }
        }

        [BurstCompile]
        private partial struct WriteToBuffer : IJobEntity
        {
            public NativeArray<DamageNumberBuffer> damageTexts;
            public void Execute (DynamicBuffer<DamageNumberBuffer> damageTextBuffer) 
            {
                damageTextBuffer.AddRange(damageTexts);
            }
        }
    }
}
