using Unity.Burst;
using Unity.Entities;

namespace ErenAydin.DamageNumbers 
{
	[BurstCompile]
	public partial struct DamageTextAnimationSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate (ref SystemState state)
		{
			state.RequireForUpdate<DamageTextComponent>();
		}
	
		[BurstCompile]
		public void OnUpdate (ref SystemState state)
		{
			var dT = SystemAPI.Time.DeltaTime;
			state.Dependency = new DamageTextAnimationJob () { dT = dT }.
				ScheduleParallel(state.Dependency);
		}
	}
}
