using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FiendishFools.Effects
{
    public class MoveToSlotEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                int num = targetSlotInfo.SlotID > caster.SlotID ? 1 : -1;
                int moveAmount = Math.Abs(caster.SlotID - targetSlotInfo.SlotID);
                for (int i = 0; i < moveAmount; i++)
                {
                    if (caster.IsUnitCharacter)
                    {
                        if (caster.SlotID + num >= 0 && caster.SlotID + num < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(caster.SlotID, caster.SlotID + num, isMandatory: true))
                        {
                            exitAmount++;
                        }
                    }
                    else
                    {
                        int num2 = targetSlotInfo.SlotID > caster.SlotID ? caster.Size : -1;
                        if (stats.combatSlots.CanEnemiesSwap(caster.SlotID, caster.SlotID + num2, out var firstSlotSwap, out var secondSlotSwap) && stats.combatSlots.SwapEnemies(caster.SlotID, firstSlotSwap, caster.SlotID + num2, secondSlotSwap))
                        {
                            exitAmount++;
                        }

                    }
                }
            }
            exitAmount = exitAmount * entryVariable;
            return exitAmount > 0;
        }
    }
}
