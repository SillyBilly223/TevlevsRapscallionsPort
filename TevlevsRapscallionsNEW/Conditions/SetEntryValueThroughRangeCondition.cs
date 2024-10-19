using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.Conditions
{
    public class SetEntryValueThroughRangeCondition : EffectConditionSO
    {
        public int Min, Max;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            effects[currentIndex].entryVariable = Random.Range(Min, Max + 1);
            return true;
        }
    }
}
