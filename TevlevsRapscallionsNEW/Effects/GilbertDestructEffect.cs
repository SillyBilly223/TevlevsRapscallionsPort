using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class GilbertDestructEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            for (int i = 0; i < targets.Length; i++)
                if (targets[i].HasUnit && targets[i].Unit.ContainsPassiveAbility("GilbertEnemy_ID") || !targets[i].HasUnit)
                    return false;

            foreach (EnemyCombat Enemies in stats.EnemiesOnField.Values)
                if (Enemies.UnitTypes.Contains("GilbertType"))
                {
                    exitAmount += Enemies.MaximumHealth;
                    Enemies.DirectDeath(caster);
                }

            return exitAmount > 0;
        }
    }
}
