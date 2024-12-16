using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Damage_BasedOnHealthPlusConstricted_Effect : EffectSO
    {
        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        public bool _ignoreShield;

        public bool _indirect;

        public bool _returnKillAsSuccess;

        public float precentageAmount = 50f;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {

            exitAmount = 0;
            bool flag = false;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int targetSlotOffset = (areTargetSlots ? (targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID) : (-1));
                    DamageInfo damageInfo;
                    int amount = 0;
                    if (ContainsConstricted(stats, targetSlotInfo.Unit))
                    {
                        amount = caster.CurrentHealth;
                    }
                    else
                    {
                        float f = precentageAmount * caster.CurrentHealth / 100f;
                        amount = Mathf.Max(1, Mathf.FloorToInt(f));
                    }
                    if (_indirect)
                    {
                        damageInfo = targetSlotInfo.Unit.Damage(amount, null, _DeathTypeID, targetSlotOffset, addHealthMana: false, directDamage: false, ignoresShield: true);
                    }
                    else
                    {
                        amount = caster.WillApplyDamage(amount, targetSlotInfo.Unit);
                        damageInfo = targetSlotInfo.Unit.Damage(amount, caster, _DeathTypeID, targetSlotOffset, addHealthMana: true, directDamage: true, _ignoreShield);
                    }

                    flag |= damageInfo.beenKilled;
                    exitAmount += damageInfo.damageAmount;
                }
            }

            if (!_indirect && exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            if (!_returnKillAsSuccess)
            {
                return exitAmount > 0;
            }

            return flag;
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
