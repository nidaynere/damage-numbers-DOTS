using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ErenAydin.DamageNumbers 
{
    public class DamageTextsSingletonAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject damageTextPrefab;
    
        class Baker : Baker<DamageTextsSingletonAuthoring>
        {
            public override void Bake (DamageTextsSingletonAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var flags =
                    TransformUsageFlags.Dynamic |
                    TransformUsageFlags.Renderable |
                    TransformUsageFlags.WorldSpace;
    
                AddComponent(entity, new DamageTextInitializerComponent
                {
                    damageTextPrefab = GetEntity (authoring.damageTextPrefab, flags)
                });
    
                AddBuffer<DamageNumberBuffer>(entity);
            }
        }
    }
    
    [BurstCompile]
    public struct DamageTextInitializerComponent : IComponentData
    {
        public byte isInitialized;
        public Entity damageTextPrefab;
    }
    
    [BurstCompile]
    public partial struct DamageNumberBuffer : IBufferElementData
    {
        public float3 position;
        public float4 color;
        public uint damageNumber;
        public float scale;
    }
}
