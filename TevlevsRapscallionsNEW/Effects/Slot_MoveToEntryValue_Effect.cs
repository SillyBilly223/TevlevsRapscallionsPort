using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Slot_MoveToEntryValue_Effect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (entryVariable < 0 || entryVariable > 4) return false;

            int direction = entryVariable > caster.SlotID ? 1 : -1;
            int moveAmount = Math.Abs(caster.SlotID - entryVariable);
            for (int i = 0; i < moveAmount; i++)
            {
                if (caster.IsUnitCharacter)
                {
                    if (caster.SlotID + direction >= 0 && caster.SlotID + direction < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(caster.SlotID, caster.SlotID + direction, isMandatory: true))
                    {
                        exitAmount++;
                    }
                }
                else
                {
                    int direction2 = entryVariable > caster.SlotID ? caster.Size : -1;
                    if (stats.combatSlots.CanEnemiesSwap(caster.SlotID, caster.SlotID + direction2, out var firstSlotSwap, out var secondSlotSwap) && stats.combatSlots.SwapEnemies(caster.SlotID, firstSlotSwap, caster.SlotID + direction2, secondSlotSwap))
                    {
                        exitAmount++;
                    }

                }
            }

            return exitAmount > 0;
        }
    }
}
