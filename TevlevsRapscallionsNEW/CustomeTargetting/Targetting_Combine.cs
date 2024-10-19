using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomeTargetting
{
    public class Targetting_Combine : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO OrginTargetting;
        public BaseCombatTargettingSO Orgin2Targetting;

        //ERM idk what to set these to
        public override bool AreTargetAllies => false;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> Targets = OrginTargetting.GetTargets(slots, casterSlotID, isCasterCharacter).ToList();
            Targets.AddRange(Orgin2Targetting.GetTargets(slots, casterSlotID, isCasterCharacter).ToList());
            return Targets.ToArray();
        }
    }

}
