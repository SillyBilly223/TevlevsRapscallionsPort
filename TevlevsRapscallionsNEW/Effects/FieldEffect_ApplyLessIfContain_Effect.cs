using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


namespace TevlevsRapscallionsNEW.Effects
{
    public class FieldEffect_ApplyLessIfContain_Effect : EffectSO
    {
        [Header("Field")]
        public FieldEffect_SO _Field;

        [Header("Previous Random Option Data")]
        public bool _UseRandomBetweenPrevious;

        [Header("Previous Multiplier Option Data")]
        public bool _UsePreviousExitValueAsMultiplier;

        public int _PreviousExtraAddition;

        public int IfContainsAmount = 1;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (_Field == null)
            {
                return false;
            }

            if (_UsePreviousExitValueAsMultiplier)
            {
                entryVariable = _PreviousExtraAddition + entryVariable * base.PreviousExitValue;
            }

            for (int i = 0; i < targets.Length; i++)
            {
                int Amount = targets[i].HasUnit && ContainsConstricted(stats, targets[i].Unit)? IfContainsAmount : entryVariable;
                exitAmount += ApplyFieldEffect(stats, targets[i], Amount);
            }

            return exitAmount > 0;
        }

        public int ApplyFieldEffect(CombatStats stats, TargetSlotInfo target, int entryVariable)
        {
            int num = (_UseRandomBetweenPrevious ? Random.Range(base.PreviousExitValue, entryVariable + 1) : entryVariable);
            if (num < _Field.MinimumRequiredToApply)
            {
                return 0;
            }

            if (!stats.combatSlots.ApplyFieldEffect(target.SlotID, target.IsTargetCharacterSlot, _Field, num))
            {
                return 0;
            }

            return num;
        }

        public bool ContainsConstricted(CombatStats stats, IUnit Unit)
        {
            for (int i = 0; i < Unit.Size; i++)
                if (stats.combatSlots.UnitInSlotContainsFieldEffect(Unit.SlotID + i, Unit.IsUnitCharacter, TempFieldEffectID.Constricted_ID.ToString()))
                    return true;
            return false;
        }
    }
}
