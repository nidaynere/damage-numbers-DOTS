using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ErenAydin.DamageNumbers 
{
	[BurstCompile]
	public partial struct DamageTextRenderSystem : ISystem
	{
		private NativeArray<uint> m_allocator;
	
		[BurstCompile]
		public void OnCreate (ref SystemState state)
		{
			state.RequireForUpdate<DamageTextInitializerComponent>();
			m_allocator = new NativeArray<uint>(6, Allocator.Persistent);
		}
	
		[BurstCompile]
		public void Dispose ()
		{
			m_allocator.Dispose();
		}
	
		[BurstCompile]
		public void OnUpdate (ref SystemState state)
		{
			var baseSeed = (uint)Random.Range(1, uint.MaxValue);
			var damageTextBuffer = SystemAPI.GetSingletonBuffer<DamageNumberBuffer>();
	
			if (damageTextBuffer.Length == 0)
			{
				return;
			}

            state.Dependency = new DamageTextDrawJob () { 
				baseSeed = baseSeed,
				m_allocator = m_allocator, 
				damageRequests = damageTextBuffer }.Schedule(state.Dependency);
		}
	}
}