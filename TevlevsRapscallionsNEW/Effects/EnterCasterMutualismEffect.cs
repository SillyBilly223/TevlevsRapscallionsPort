using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class EnterCasterMutualismEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            for (int i = 0; i < targets.Length; i++) 
            {
                
            }

            return exitAmount > 0;
        }
    }
}
