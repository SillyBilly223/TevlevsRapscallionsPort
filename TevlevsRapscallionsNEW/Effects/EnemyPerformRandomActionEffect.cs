using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.Effects
{
    public class EnemyPerformRandomActionEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.IsUnitCharacter) return false;
            EnemyCombat CurrentEnemy = stats.TryGetEnemyOnField(caster.ID);    
            if (CurrentEnemy == null) return false;

            int AbilityRange = (CurrentEnemy.Abilities.Count - 1) - (CurrentEnemy.ExtraAbilities.Count - 1);
            AbilitySO ChosenAbility = CurrentEnemy.Abilities[Random.Range(1, AbilityRange)].ability;

            StringReference args = new StringReference(ChosenAbility.GetAbilityLocData().text);
            CombatManager.Instance.PostNotification(TriggerCalls.OnAbilityWillBeUsed.ToString(), this, args);
            CombatManager.Instance.AddSubAction(new PlayAbilityAnimationAction(ChosenAbility.visuals, ChosenAbility.animationTarget, CurrentEnemy));
            CombatManager.Instance.AddSubAction(new EffectAction(ChosenAbility.effects, CurrentEnemy));

            return true;
        }
    }
}
