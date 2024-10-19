using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.Actions;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Effects
{

    public class SetEffectAsSubActionEffect : EffectSO
    {
        public EffectSO Effect;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (Effect == null)
            { return false; }

            CombatManager._instance.AddSubAction(new PerformEffectAction(Effect, caster, targets, areTargetSlots, entryVariable, PreviousExitValue));

            return true;
        }

        public static EffectInfo Gen(EffectInfo Effect)
        {
            SetEffectAsSubActionEffect SubEffect = ScriptableObject.CreateInstance<SetEffectAsSubActionEffect>();
            SubEffect.Effect = Effect.effect;
            return new EffectInfo { effect = SubEffect, entryVariable = Effect.entryVariable, targets = Effect.targets.Clone(), condition = Effect.condition != null? Effect.condition.Clone() : null };
        }
    }
}
