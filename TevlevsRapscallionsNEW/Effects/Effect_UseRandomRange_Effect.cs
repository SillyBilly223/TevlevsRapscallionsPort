using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.Effects
{

    public class Effect_UseRandomRange_Effect : EffectSO
    {
        public EffectSO Effect;

        public Vector2 Range = Vector2.one;
        
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            return Effect.PerformEffect(stats, caster, targets, areTargetSlots, Mathf.RoundToInt(Random.Range(Range.x, Range.y + 1)), out exitAmount);
        }
    }
}
