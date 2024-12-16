using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class EnemyTurn_RemoveExtraAbilityFromEnemy_Effect : EffectSO
    {
        public ExtraAbilityInfo ExtraAbility;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.IsUnitCharacter) return false;
            EnemyCombat Enemy = stats.TryGetEnemyOnField(caster.ID);
            if (Enemy == null) return false;

            for (int i = 0; i < stats.timeline.Round.Count; i++)
                if (stats.timeline.Round[i].turnUnit == Enemy && Enemy.Abilities[stats.timeline.Round[i].abilitySlot].ability.name == ExtraAbility.ability.name)
                    return false;

            caster.TryRemoveExtraAbility(ExtraAbility);
            return true;
        }
    }
}
