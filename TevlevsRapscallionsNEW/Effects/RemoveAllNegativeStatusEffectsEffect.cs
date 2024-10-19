using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class RemoveAllNegativeStatusEffectsEffect : EffectSO
    {
        public bool RemovePositive = false;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            for (int i = 0; i < targets.Length; i++)
                if (targets[i].HasUnit && targets[i].Unit is IStatusEffector istatusEffector)
                {
                    List<string> RemoveID = new List<string>();
                    
                    foreach (IStatusEffect statusEffect in istatusEffector.StatusEffects)
                        if (statusEffect.IsPositive == RemovePositive) RemoveID.Add(statusEffect.StatusID);

                    for (int s = 0; s < RemoveID.Count; s++)
                        exitAmount += targets[i].Unit.TryRemoveStatusEffect(RemoveID[s]);
                }

            return exitAmount > 0;
        }
    }
}
