using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Check_IfAvaiableEnemySpawn_Effect : EffectSO
    {
        public int SizeNeeded = 1;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            return stats.GetRandomEnemySlot(SizeNeeded) != -1;
        }
    }
}
