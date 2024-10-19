using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace TevlevsRapscallionsNEW.CustomeTargetting
{
    public class Targetting_OrignGilbert : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO OrginTargeting;

        public override bool AreTargetAllies => OrginTargeting.AreTargetAllies;

        public override bool AreTargetSlots => OrginTargeting.AreTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> Targets = new List<TargetSlotInfo>();
            if (OrginTargeting == null || isCasterCharacter) return Targets.ToArray();

            TargetSlotInfo SlotInfo = slots.GetEnemyTargetSlot(casterSlotID, 0);
            if (SlotInfo == null || !SlotInfo.HasUnit) return Targets.ToArray();

            int OrginGilbert = ExtraUtils.ContainsEnemyGilbert(SlotInfo.Unit.ID);
            if (OrginGilbert == -1) return Targets.ToArray();

            CharacterCombat Char = CombatManager._instance._stats.TryGetCharacterOnField(OrginGilbert);
            if (Char == null) return Targets.ToArray();

            return OrginTargeting.GetTargets(slots, Char.SlotID, Char.IsUnitCharacter);
        }

        public static Targetting_OrignGilbert Generate (BaseCombatTargettingSO orgin)
        {
            Targetting_OrignGilbert Targetting = ScriptableObject.CreateInstance<Targetting_OrignGilbert>();
            Targetting.OrginTargeting = orgin;
            return Targetting;
        }
    }
}
