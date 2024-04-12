using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ErenAydin.DamageNumbers
{
    [BurstCompile]
    internal partial struct DamageTextAnimationJob : IJobEntity
    {
        [ReadOnly] public float dT;
    
        [BurstCompile]
        public void Execute (
            ref DamageTextComponent damageText,
            ref DamageTextAnimTime01Component damageTextAnimation)
        {
            if (damageText.IsBusy == 0)
            {
                return;
            }
    
            damageTextAnimation.Value += dT;
    
            if (damageTextAnimation.Value >= 1)
            {
                damageText.IsBusy = 0;
            }
        }
    }
}
