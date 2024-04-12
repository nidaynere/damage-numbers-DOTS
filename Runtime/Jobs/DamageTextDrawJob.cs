using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ErenAydin.DamageNumbers 
{
	[BurstCompile]
	internal partial struct DamageTextDrawJob : IJobEntity
	{
		[ReadOnly] public uint baseSeed;
	
		public DynamicBuffer<DamageNumberBuffer> damageRequests;
		public NativeArray<uint> m_allocator;
	
		[BurstCompile]
		private int NumberToArray (uint number, NativeArray<uint> allocator)
		{
			int found = 0;
			while (number != 0)
			{
				allocator[found++] = number % 10;
				number /= 10;
			}
	
			return found;
		}
	
		[BurstCompile]
		public void Execute (
			[EntityIndexInQuery] int entityIndexInQuery,
			ref LocalTransform localTransform,
			ref DamageTextComponent damageText,
			ref DamageTextNumber0Component number0,
			ref DamageTextNumber1Component number1,
			ref DamageTextNumber2Component number2,
			ref DamageTextNumber3Component number3,
			ref DamageTextNumber4Component number4,
			ref DamageTextNumber5Component number5,
			ref DamageTextValidNumberCountComponent validNumberCount,
			ref DamageTextAnimDirectionComponent animDirection,
			ref DamageTextColorComponent color,
			ref DamageTextScaleComponent scale,
			ref DamageTextAnimTime01Component animTime
			)
		{
            if (this.damageRequests.Length == 0)
			{
				return;
			}
	
			if (damageText.IsBusy == 1)
			{
				return;
			}
	
			var damageRequest = this.damageRequests[0];
	
			var damageNumber = math.min(999999, damageRequest.damageNumber);
	
			if (damageNumber == 0)
			{
				return;
			}
	
			damageText.IsBusy = 1;
	
			var digitsCount = NumberToArray(damageNumber, m_allocator);
	
			#region enter numbers
				if (digitsCount >= 1)
				{
					number0.Value = m_allocator[digitsCount - 1];
				}
	
				if (digitsCount >= 2)
				{
					number1.Value = m_allocator[digitsCount - 2];
				}
	
				if (digitsCount >= 3)
				{
					number2.Value = m_allocator[digitsCount - 3];
				}
	
				if (digitsCount >= 4)
				{
					number3.Value = m_allocator[digitsCount - 4];
				}
	
				if (digitsCount >= 5)
				{
					number4.Value = m_allocator[digitsCount - 5];
				}
	
				if (digitsCount >= 6)
				{
					number5.Value = m_allocator[digitsCount - 6];
				}
				#endregion
	
			validNumberCount.Value = digitsCount;
	
			scale.Value = damageRequest.scale;
	
			localTransform.Position = damageRequest.position;
	
			var random = Random.CreateFromIndex ((uint)entityIndexInQuery * 1000 + baseSeed);
			var randomDir = random.NextFloat3Direction();
			var randomLength = random.NextFloat(3f, 6f);
			randomDir.y = math.abs(randomDir.y);
	
			animDirection.Value = randomDir * randomLength;
	
			color.Value = damageRequest.color;
			animTime.Value = 0;

            this.damageRequests.RemoveAt(0);
		}
	}
}