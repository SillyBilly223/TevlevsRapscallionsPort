using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using TevlevsRapscallionsNEW.CustomePopUpsActions;
using static UnityEngine.UI.CanvasScaler;

namespace TevlevsRapscallionsNEW.Effects
{
    public class ParasiteProducingDamageEffect : EffectSO
    {
        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        public bool _usePreviousExitValue;

        public bool _ignoreShield;

        public bool _returnKillAsSuccess;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
            {
                entryVariable *= base.PreviousExitValue;
            }

            exitAmount = 0;
            bool flag = false;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int targetSlotOffset = (areTargetSlots ? (targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID) : (-1));
                    int amount = entryVariable;
                    amount = caster.WillApplyDamage(amount, targetSlotInfo.Unit);
                    DamageInfo damageInfo = targetSlotInfo.Unit.Damage(amount, caster, _DeathTypeID, targetSlotOffset, addHealthMana: true, directDamage: true, _ignoreShield);

                    flag |= damageInfo.beenKilled;
                    exitAmount += damageInfo.damageAmount;

                    if (damageInfo.damageAmount > 0)
                        targetSlotInfo.Unit.ApplyEmptyParasiteToUnit(damageInfo.damageAmount);
                }
            }

            if (exitAmount > 0)
                caster.DidApplyDamage(exitAmount);

            if (!_returnKillAsSuccess)
                return exitAmount > 0;

            return flag;
        }
    }
}
