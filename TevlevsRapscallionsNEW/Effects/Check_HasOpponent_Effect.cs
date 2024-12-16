using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Check_HasOpponent_Effect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++)
                if (targets[i].HasUnit && targets[i].Unit.IsUnitCharacter != caster.IsUnitCharacter)
                    exitAmount++;
            return exitAmount > 0;
        }
    }
}
