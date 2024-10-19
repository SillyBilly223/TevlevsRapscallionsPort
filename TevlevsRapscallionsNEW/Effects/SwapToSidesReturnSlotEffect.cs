using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.Effects
{
    public class SwapToSidesReturnSlotEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = caster.SlotID;
            int spawmAmount = 0;
            List<IUnit> list = new List<IUnit>();
            List<IUnit> list2 = new List<IUnit>();
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].HasUnit)
                {
                    IUnit unit = targets[i].Unit;
                    if (unit.IsUnitCharacter && !list.Contains(unit))
                    {
                        list.Add(unit);
                    }
                    else if (!unit.IsUnitCharacter && !list2.Contains(unit))
                    {
                        list2.Add(unit);
                    }
                }
            }

            foreach (IUnit item in list)
            {
                int num = Random.Range(0, 2) * 2 - 1;
                if (item.SlotID + num >= 0 && item.SlotID + num < stats.combatSlots.CharacterSlots.Length)
                {
                    if (stats.combatSlots.SwapCharacters(item.SlotID, item.SlotID + num, isMandatory: true))
                    {
                        spawmAmount++;
                    }

                    continue;
                }

                num *= -1;
                if (item.SlotID + num >= 0 && item.SlotID + num < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(item.SlotID, item.SlotID + num, isMandatory: true))
                {
                    spawmAmount++;
                }
            }

            foreach (IUnit item2 in list2)
            {
                int num = Random.Range(0, 2) * (item2.Size + 1) - 1;
                if (stats.combatSlots.CanEnemiesSwap(item2.SlotID, item2.SlotID + num, out var firstSlotSwap, out var secondSlotSwap))
                {
                    if (stats.combatSlots.SwapEnemies(item2.SlotID, firstSlotSwap, item2.SlotID + num, secondSlotSwap, isMandatory: true))
                    {
                        spawmAmount++;
                    }

                    continue;
                }

                num = ((num < 0) ? item2.Size : (-1));
                if (stats.combatSlots.CanEnemiesSwap(item2.SlotID, item2.SlotID + num, out firstSlotSwap, out secondSlotSwap) && stats.combatSlots.SwapEnemies(item2.SlotID, firstSlotSwap, item2.SlotID + num, secondSlotSwap, isMandatory: true))
                {
                    spawmAmount++;
                }
            }

            return spawmAmount > 0;
        }
    }
}
