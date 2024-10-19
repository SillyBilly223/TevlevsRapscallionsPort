using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomeTargetting
{
    public class Targetting_GilbertFlip : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO OrginTargetting;
        public override bool AreTargetAllies => true;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            isCasterCharacter = false;
            List<TargetSlotInfo> Targets = new List<TargetSlotInfo>();
            if (OrginTargetting != null)
            {
                TargetSlotInfo[] RefTargets = OrginTargetting.GetTargets(slots, casterSlotID, isCasterCharacter);
                for (int i = 0; i < RefTargets.Length; i++)
                {
                    if (RefTargets[i].IsTargetCharacterSlot != isCasterCharacter && RefTargets[i].SlotID == casterSlotID)
                        Targets.AddRange(GetLeftRighTargets(slots, casterSlotID, isCasterCharacter));
                    else
                    {
                        TargetSlotInfo Target = GetTarget(slots, RefTargets[i].SlotID, RefTargets[i].IsTargetCharacterSlot == isCasterCharacter);
                        if (Target != null)
                            Targets.Add(Target);
                    }
                }
            }
            else
            {
                Targets.AddRange(GetLeftRighTargets(slots, casterSlotID, isCasterCharacter));
            }
            return Targets.ToArray();
        }

        public List<TargetSlotInfo> GetLeftRighTargets(SlotsCombat slots, int orginSlotID, bool targetCharacter)
        {
            List<TargetSlotInfo> Targets = new List<TargetSlotInfo>();
            TargetSlotInfo target = slots.GetAllySlotTarget(orginSlotID, 1, targetCharacter);
            if (target != null) Targets.Add(target);
            target = slots.GetAllySlotTarget(orginSlotID, -1, targetCharacter);
                if (target != null) Targets.Add(target);
            return Targets;
        }

        public TargetSlotInfo GetTarget(SlotsCombat slots, int targetSlotID, bool targetCharacter)
        {
            if (targetCharacter)
               return slots.GetCharacterTargetSlot(targetSlotID, 0);
            else
                return slots.GetEnemyTargetSlot(targetSlotID, 0);
        }

        public static Targetting_GilbertFlip Generate(BaseCombatTargettingSO Orgin)
        {
            Targetting_GilbertFlip targetting_Flip = ScriptableObject.CreateInstance<Targetting_GilbertFlip>();
            targetting_Flip.OrginTargetting = Orgin;
            return targetting_Flip;
        }
    }
}
