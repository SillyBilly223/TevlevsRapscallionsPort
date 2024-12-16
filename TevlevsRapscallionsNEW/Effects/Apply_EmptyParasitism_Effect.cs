using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.CustomePopUpsActions;
using static UnityEngine.UI.CanvasScaler;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Apply_EmptyParasitism_Effect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            for (int i = 0; i < targets.Length; i++)
                if (targets[i].HasUnit)
                    exitAmount += targets[i].Unit.ApplyEmptyParasiteToUnit(entryVariable);

            return exitAmount > 0;
        }
    }
}
