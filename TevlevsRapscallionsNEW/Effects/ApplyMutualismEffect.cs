using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class ApplyMutualismEffect : EffectSO
    {
        public ParasitePassiveAbility CustomeMutuilism = null;

        public bool ApplyEmptyMutualism = false;

        public bool UsePrevious = false;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (UsePrevious) entryVariable *= PreviousExitValue;

            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                if (ApplyEmptyMutualism || targets[i].Unit == caster)
                {
                    if (targets[i].Unit.ApplyEmptyMutualism(entryVariable))
                        exitAmount += entryVariable;
                }
                else if (caster.ConvertUnitToMutualism(targets[i].Unit, CustomeMutuilism))
                { 
                    exitAmount += targets[i].Unit.CurrentHealth; 
                    return true; 
                }
            }
            return exitAmount > 0;
        }
    }
}
