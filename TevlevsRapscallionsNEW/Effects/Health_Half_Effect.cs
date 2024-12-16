using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Health_Half_Effect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            for (int i  = 0; i < targets.Length; i++) 
            {
                if (targets[i].HasUnit && targets[i].Unit.CurrentHealth > 1)
                {
                    int HalfAmount = Mathf.RoundToInt(targets[i].Unit.CurrentHealth / 2);
                    targets[i].Unit.MaximizeHealth(Math.Max(targets[i].Unit.CurrentHealth - HalfAmount, 1));
                    exitAmount++;
                }
            }
            return exitAmount > 0;
        }
    }
}
