using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace ErenAydin.DamageNumbers
{
    public class DrawThousandsSampleAuthoring : MonoBehaviour
    {
        class Baker : Baker<DrawThousandsSampleAuthoring>
        {
            public override void Bake (DrawThousandsSampleAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                AddComponent<DrawThousandsSampleComponent>(entity);
            }
        }
    }

    [BurstCompile]
    public struct DrawThousandsSampleComponent : IComponentData
    {
    }
}
