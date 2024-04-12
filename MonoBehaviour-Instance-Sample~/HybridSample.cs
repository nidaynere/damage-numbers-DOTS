using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ErenAydin.DamageNumbers
{
    internal sealed class HybridSample : MonoBehaviour
    {
        private EntityQuery damageNumberQuery;

        [SerializeField] private int howManyPerFrame = 10;
        [SerializeField] private float range = 40;

        private void Start ()
        {
            damageNumberQuery = World.
                DefaultGameObjectInjectionWorld.
                EntityManager.
                CreateEntityQuery(typeof(DamageNumberBuffer));
        }

        private void OnDestroy ()
        {
            damageNumberQuery.Dispose();
        }

        private void Update ()
        {
            if (!damageNumberQuery.TryGetSingletonBuffer<DamageNumberBuffer>(out var buffer)) 
            {
                return;
            }

            var m_alloc = new NativeArray <DamageNumberBuffer>(howManyPerFrame, Allocator.Temp);

            for (int i = 0; i < howManyPerFrame; i++)
            {
                var color = new float4();
                color.xyz = UnityEngine.Random.insideUnitSphere;
                color.w = 1;

                m_alloc[i] = new DamageNumberBuffer()
                {
                    color = color,
                    damageNumber = (uint)UnityEngine.Random.Range(0, 999999),
                    position = UnityEngine.Random.insideUnitSphere * range,
                    scale = UnityEngine.Random.Range(0.8f, 1.2f)
                };
            }

            buffer.AddRange(m_alloc);

            m_alloc.Dispose();
        }
    }
}