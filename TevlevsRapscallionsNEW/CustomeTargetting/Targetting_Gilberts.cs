using HarmonyLib;
using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.CustomeTargetting
{
    public class Targetting_Gilberts : BaseCombatTargettingSO
    {
        public override bool AreTargetAllies => false;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> Targetting = new List<TargetSlotInfo>();

            foreach (EnemyCombat Enemies in CombatManager._instance._stats.EnemiesOnField.Values)
                if (Enemies.ContainsPassiveAbility("GilbertEnemy_ID"))
                    Targetting.Add(slots.GetEnemyTargetSlot(Enemies.SlotID, 0));

            return Targetting.ToArray();
        }
    }
}
