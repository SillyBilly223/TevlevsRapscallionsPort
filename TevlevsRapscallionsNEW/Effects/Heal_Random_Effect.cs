using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.Effects
{
    public class Heal_Random_Effect : EffectSO
    {
        public bool entryAsPercentage;

        public bool _directHeal = true;

        public bool _onlyIfHasHealthOver0;

        public int Min, Max;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {

            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit && (!_onlyIfHasHealthOver0 || targetSlotInfo.Unit.CurrentHealth > 0))
                {
                    int num = Random.Range(Min, Max); ;
                    if (entryAsPercentage)
                    {
                        num = targetSlotInfo.Unit.CalculatePercentualAmount(num);
                    }

                    if (_directHeal)
                    {
                        num = caster.WillApplyHeal(num, targetSlotInfo.Unit);
                    }

                    exitAmount += targetSlotInfo.Unit.Heal(num, caster, _directHeal);
                }
            }

            return exitAmount > 0;
        }

        public static EffectSO Generate(int min, int max)
        {
            Heal_Random_Effect healRandomEffect = ScriptableObject.CreateInstance<Heal_Random_Effect>();
            healRandomEffect.Min = min;
            healRandomEffect.Max = max + 1;
            return healRandomEffect;
        }
    }
}
