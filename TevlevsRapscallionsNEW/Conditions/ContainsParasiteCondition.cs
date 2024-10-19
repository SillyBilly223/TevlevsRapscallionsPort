using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace TevlevsRapscallionsNEW.Conditions
{
    public class ContainsParasiteCondition : EffectConditionSO
    {
        public BaseCombatTargettingSO CheckTargets = Targeting.Slot_Front;

        public bool WasSuccessful = false;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            TargetSlotInfo[] Targets = CheckTargets.GetTargets(CombatManager._instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter);
            for (int i = 0; i < Targets.Length; i++)
                if (Targets[i].HasUnit && (Targets[i].Unit.ContainsPassiveAbility("ParasiteLB_ID") || Targets[i].Unit.ContainsPassiveAbility(Passives.ParasiteParasitism.m_PassiveID)) == WasSuccessful)
                    return true;
            return false;
        }
    }
}
