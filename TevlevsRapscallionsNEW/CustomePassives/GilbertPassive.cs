using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TevlevsRapscallionsNEW.Actions;
using TevlevsRapscallionsNEW.Characters;
using TevlevsRapscallionsNEW.ValueModifiers;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.CustomePassives
{
    public class GilbertPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => IsEnemyGilbert != true;

        public override bool DoesPassiveTrigger => true;

        public override void TriggerPassive(object sender, object args)
        {
            IUnit Unit = sender as IUnit;

            if (IsEnemyGilbert && Unit != null)
            {
                if (ExtraUtils.TryRemoveEnemyGilbert(Unit.ID, out int CharID))
                {
                    IUnit Gilbert = CombatManager._instance._stats.TryGetCharacterOnField(CharID);
                    if (Gilbert != null)
                        Gilbert.MaximizeHealth(Gilbert.MaximumHealth + Unit.MaximumHealth);
                }
                return;
            }

            UseAbilityInfo UseInfo = args as UseAbilityInfo;

            //Debug.Log(Ability.ability._description);

            foreach (EnemyCombat Enemy in CombatManager._instance._stats.EnemiesOnField.Values)
            {
                if (Gilbert_Check(Enemy))
                {
                    int ABPointer = Gilbert_EqualsBaseAbility(sender as CharacterCombat, UseInfo.Ability.ability.name);
                    if (ABPointer != -1 && ABPointer < Enemy.AbilityCount)
                    {
                        Gilbert_AddAbilityToTimeLine(Enemy, ABPointer);
                    }
                    else
                    {
                        ExtraAbilityInfo Ability = ExtraUtils.GilbertAbilityTranslate((args as UseAbilityInfo).Ability);
                        if (Enemy.ExtraAbilities.Contains(Ability))
                        {
                            for (int i = 0; i < Enemy.ExtraAbilities.Count; i++)
                                if (Enemy.ExtraAbilities[i] == Ability) ABPointer = ((Enemy.Abilities.Count - 1) - (Enemy.ExtraAbilities.Count - 1) + i);
                        }
                        else
                        {
                            Enemy.AddExtraAbility(Ability);
                            ABPointer = Enemy.AbilityCount - 1;
                        }

                        Gilbert_AddAbilityToTimeLine(Enemy, ABPointer);
                    }
                }
            }
        }

        public void Gilbert_AddAbilityToTimeLine(ITurn Unit, int AbilityID)
        {
            if (AddGilbertActionsToTimeLineAction.IsPending)
                AddGilbertActionsToTimeLineAction.AddToPending(Unit, AbilityID);
            else
                CombatManager._instance.AddRootAction(new AddGilbertActionsToTimeLineAction(Unit, AbilityID));
        }

        public bool Gilbert_Check(IUnit Unit)
        {
            return (Unit.UnitTypes.Contains("GilbertType")) && (Unit.IsAlive && Unit.CurrentHealth > 0) && (ExtraUtils.ContainsEnemyGilbert(Unit.ID) != -1);
        }

        public int Gilbert_EqualsBaseAbility(CharacterCombat Character, string AbilityID) 
        {
            if (Gilbert.Char_Gilbert.basicCharAbility.ability.name == AbilityID)
                return 0;
            for (int i = 0; i < Gilbert.Char_Gilbert.rankedData[Character.Rank].rankAbilities.Length; i++)
                if (Gilbert.Char_Gilbert.rankedData[Character.Rank].rankAbilities[i].ability.name == AbilityID)
                    return i + 1;
            return -1;
        }

        public void PassiveHalfDamage(object sender, object args)
        {
            DamageDealtValueChangeException expectation = args as DamageDealtValueChangeException;
            if (expectation.damagedUnit.UnitTypes.Contains("GilbertType"))
                expectation.AddModifier(new GilbertValueModifier(true, 50));
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            if (IsEnemyGilbert)
            {
                CombatManager._instance.AddObserver(TryTriggerPassive, TriggerCalls.OnDeath.ToString(), unit);
            }
            else
            {
                ExtraUtils.AddGilbert(unit.ID);
                CombatManager._instance.AddObserver(TryTriggerPassive, "OnAbilityUsedInfo", unit);
            }
            CombatManager._instance.AddObserver(PassiveHalfDamage, TriggerCalls.OnWillApplyDamage.ToString(), unit);
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
        }

        public override void CustomOnTriggerDettached(IPassiveEffector caller)
        {
            if (IsEnemyGilbert)
            {
                CombatManager._instance.RemoveObserver(TryTriggerPassive, TriggerCalls.OnDeath.ToString(), caller);
            }
            else
            {
                ExtraUtils.TryRemoveGilbert(caller.ID);
                CombatManager._instance.RemoveObserver(TryTriggerPassive, "OnAbilityUsedInfo", caller);
            }
            CombatManager._instance.RemoveObserver(PassiveHalfDamage, TriggerCalls.OnWillApplyDamage.ToString(), caller);
        }

        public bool IsEnemyGilbert;
    }
}
