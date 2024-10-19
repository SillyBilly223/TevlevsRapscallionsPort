using UnityEngine;

namespace TevlevsRapscallionsNEW.Conditions
{
    public class SetEntryValueToIndexExitValueCondition : EffectConditionSO
    {
        public int EffectIndex = 0;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            if (EffectIndex < 0 || effects.Length - 1 < EffectIndex + 1) return false;
            effects[currentIndex].entryVariable = effects[EffectIndex + 1].effect.PreviousExitValue;
            return true;
        }
    }
}
