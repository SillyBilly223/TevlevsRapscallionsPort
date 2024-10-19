using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomeTargetting
{
    public class Targetting_Flip : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO OrginTargetting;
        public override bool AreTargetAllies => OrginTargetting.AreTargetAllies != true;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> Targets = new List<TargetSlotInfo>();
            if (OrginTargetting != null)
            {
                TargetSlotInfo[] RefTargets = OrginTargetting.GetTargets(slots, casterSlotID, isCasterCharacter);
                for (int i = 0; i < RefTargets.Length; i++)
                {
                    TargetSlotInfo Target = GetTarget(slots, RefTargets[i].SlotID, RefTargets[i].IsTargetCharacterSlot != isCasterCharacter);
                    if (Target != null) Targets.Add(Target);
                }
            }
            else
            {
                return new TargetSlotInfo[0];
            }
            return Targets.ToArray();
        }

        public TargetSlotInfo GetTarget(SlotsCombat slots, int targetSlotID, bool targetCharacter)
        {
            return targetCharacter? slots.GetCharacterTargetSlot(targetSlotID, 0) : slots.GetEnemyTargetSlot(targetSlotID, 0);
        }

        public static Targetting_GilbertFlip Generate(BaseCombatTargettingSO Orgin)
        {
            Targetting_GilbertFlip targetting_Flip = ScriptableObject.CreateInstance<Targetting_GilbertFlip>();
            targetting_Flip.OrginTargetting = Orgin;
            return targetting_Flip;
        }
    }
}
