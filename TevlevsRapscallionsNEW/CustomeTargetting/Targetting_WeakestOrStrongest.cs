using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.CustomeTargetting
{
    public class Targetting_WeakestOrStrongest : BaseCombatTargettingSO
    {
        public bool getAllies;

        public bool targetWeakest;

        public override bool AreTargetAllies => getAllies;

        public override bool AreTargetSlots => false;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            CombatStats Stats = CombatManager._instance._stats;

            List<TargetSlotInfo> Targets = new List<TargetSlotInfo>();
            int HealthThresh = -1;
            if (getAllies)
            {
                foreach (CharacterCombat character in Stats.CharactersOnField.Values)
                {
                    if (HealthThresh < 0)
                    {
                        HealthThresh = character._currentHealth;
                        Targets.Add(slots.GetCharacterTargetSlot(character.SlotID, 0));
                        continue;
                    }
                    if (targetWeakest && character.CurrentHealth < HealthThresh)
                    {
                        Targets.Clear();
                        Targets.Add(slots.GetCharacterTargetSlot(character.SlotID, 0));
                        HealthThresh = character._currentHealth;
                    }
                    else if (!targetWeakest && character.CurrentHealth > HealthThresh)
                    {
                        Targets.Clear();
                        Targets.Add(slots.GetCharacterTargetSlot(character.SlotID, 0));
                        HealthThresh = character._currentHealth;
                    }
                }
            }
            else
            {
                foreach (EnemyCombat enemies in Stats.EnemiesOnField.Values)
                {
                    if (HealthThresh < 0)
                    {
                        HealthThresh = enemies._currentHealth;
                        Targets.Add(slots.GetEnemyTargetSlot(enemies.SlotID, 0));
                        continue;
                    }
                    if (targetWeakest && enemies.CurrentHealth < HealthThresh)
                    {
                        Targets.Clear();
                        Targets.Add(slots.GetEnemyTargetSlot(enemies.SlotID, 0));
                        HealthThresh = enemies._currentHealth;
                    }
                    else if (!targetWeakest && enemies.CurrentHealth > HealthThresh)
                    {
                        Targets.Clear();
                        Targets.Add(slots.GetEnemyTargetSlot(enemies.SlotID, 0));
                        HealthThresh = enemies._currentHealth;
                    }
                }
            }

            return Targets.ToArray();
        }
    }
}
