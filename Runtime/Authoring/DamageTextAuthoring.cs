using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace ErenAydin.DamageNumbers 
{
	public class DamageTextAuthoring : MonoBehaviour
	{
		class Baker : Baker<DamageTextAuthoring>
		{
			public override void Bake (DamageTextAuthoring authoring)
			{
				var flags =
					TransformUsageFlags.Dynamic |
					TransformUsageFlags.Renderable |
					TransformUsageFlags.WorldSpace;

				var entity = GetEntity(flags);
	
				AddComponent(entity, new DamageTextComponent());
				AddComponent(entity, new DamageTextNumber0Component());
				AddComponent(entity, new DamageTextNumber1Component());
				AddComponent(entity, new DamageTextNumber2Component());
				AddComponent(entity, new DamageTextNumber3Component());
				AddComponent(entity, new DamageTextNumber4Component());
				AddComponent(entity, new DamageTextNumber5Component());
				AddComponent(entity, new DamageTextAnimTime01Component());
				AddComponent(entity, new DamageTextAnimDirectionComponent());
				AddComponent(entity, new DamageTextValidNumberCountComponent());
				AddComponent(entity, new DamageTextColorComponent());
				AddComponent(entity, new DamageTextScaleComponent());
			}
		}
	}
	
	[BurstCompile]
	public partial struct DamageTextComponent : IComponentData
	{
		public byte IsBusy;
	}
	
	[BurstCompile]
	[MaterialProperty ("_Number0")]
	public partial struct DamageTextNumber0Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Scale")]
	public partial struct DamageTextScaleComponent : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Number1")]
	public partial struct DamageTextNumber1Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Number2")]
	public partial struct DamageTextNumber2Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Number3")]
	public partial struct DamageTextNumber3Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Number4")]
	public partial struct DamageTextNumber4Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Number5")]
	public partial struct DamageTextNumber5Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_ValidNumberCount")]
	public partial struct DamageTextValidNumberCountComponent : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_AnimTime01")]
	public partial struct DamageTextAnimTime01Component : IComponentData
	{
		public float Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_AnimDirection")]
	public partial struct DamageTextAnimDirectionComponent : IComponentData
	{
		public float3 Value;
	}
	
	[BurstCompile]
	[MaterialProperty("_Color")]
	public partial struct DamageTextColorComponent : IComponentData
	{
		public float4 Value;
	}
}
