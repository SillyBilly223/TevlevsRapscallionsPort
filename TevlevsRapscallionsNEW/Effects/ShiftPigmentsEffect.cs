using BrutalAPI;
using MUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Effects
{
    public class ShiftPigmentsEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = ShiftMana(stats.MainManaBar);
            return exitAmount > 0;
        }

        public int ShiftMana(ManaBar Bar)
        {
            List<int> list = new List<int>();
            List<ManaColorSO> list2 = new List<ManaColorSO>();
            foreach (ManaBarSlot manaBarSlot in Bar.ManaBarSlots)
            {
                if (!manaBarSlot.IsEmpty)
                {
                    manaBarSlot.SetMana(GetMana(manaBarSlot.ManaColor));
                    list.Add(manaBarSlot.SlotIndex);
                    list2.Add(manaBarSlot.ManaColor);
                }
            }

            if (list.Count > 0)
            {
                CombatManager.Instance.AddUIAction(new ModifyManaSlotsUIAction(Bar.ID, list.ToArray(), list2.ToArray()));
            }

            return list.Count;
        }

        public ManaColorSO GetMana(ManaColorSO CurrentMana)
        {
            if (CurrentMana.pigmentTypes.Count == 0) return Pigments.Red;
            if (CurrentMana.pigmentTypes.Count == 1) return ShiftMana(CurrentMana.pigmentTypes[0]);
            ManaColorSO[] ColorTypes = new ManaColorSO[CurrentMana.pigmentTypes.Count];
            for (int i = 0; i < ColorTypes.Length; i++)
                ColorTypes[i] = ShiftMana(CurrentMana.pigmentTypes[i]);
            return Pigments.SplitPigment(ColorTypes);
        }

        public ManaColorSO ShiftMana(string Type)
        {
            switch (Type)
            {
                case "Red":
                    return Pigments.Blue;
                case "Blue":
                    return Pigments.Purple;
                case "Purple":
                    return Pigments.Yellow;
                case "Yellow":
                    return Pigments.Red;
                default:
                    return Pigments.Red;
            }
        }
    }
}
