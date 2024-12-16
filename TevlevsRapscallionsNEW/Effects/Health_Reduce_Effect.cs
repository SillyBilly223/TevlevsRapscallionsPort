using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Health_Reduce_Effect : EffectSO
    {

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++) 
                if (targets[i].HasUnit) 
                {
                    exitAmount += targets[i].Unit.ReduceHealthTo(targets[i].Unit.CurrentHealth - entryVariable);
                    if (targets[i].Unit.IsAlive && targets[i].Unit.CurrentHealth <= 0)
                        targets[i].Unit.DirectDeath(caster);
                }
            return exitAmount > 0;
        }
    }
}
