using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Parasitism_IncreaseValue_Effect : EffectSO
    {
        public string StoredValueID = "ParasiteLBD_ID";

        public bool UsePreviousValue = true;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (UsePreviousValue) entryVariable *= PreviousExitValue;
            
            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++)
                if (targets[i].HasUnit && targets[i].Unit.IsAlive)
                    exitAmount += targets[i].Unit.ApplyEmptyParasiteToUnit(entryVariable);

            return exitAmount > 0;
        }
    }
}
