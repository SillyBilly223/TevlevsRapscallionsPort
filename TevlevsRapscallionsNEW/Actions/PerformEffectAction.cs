using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Actions
{
    public class PerformEffectAction : CombatAction
    {
        public EffectSO Effect;
        public IUnit Caster;
        public TargetSlotInfo[] Targets;
        public bool AreTargetSlots;
        public int EntryValue;
        public int PreviousEffectAmount;

        public PerformEffectAction(EffectSO effect, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, int previousEffectAmount)
        {
            Effect = effect;
            Caster = caster;
            Targets = targets;
            AreTargetSlots = areTargetSlots;
            EntryValue = entryVariable;
            PreviousEffectAmount = previousEffectAmount;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            Effect.PreviousExitValue = PreviousEffectAmount;
            Effect.PerformEffect(stats, Caster, Targets, AreTargetSlots, EntryValue, out int ExitAmount);
            yield break;
        }
    }
}
