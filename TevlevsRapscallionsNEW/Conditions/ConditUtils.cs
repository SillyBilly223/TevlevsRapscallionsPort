using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Conditions
{
    public static class ConditUtils
    {
        public static SetEntryValueThroughRangeCondition RandomRangeCondit(int Min, int Max)
        {
            var Condition = ScriptableObject.CreateInstance<SetEntryValueThroughRangeCondition>();
            Condition.Min = Min; Condition.Max = Max;
            return Condition;
        }
    }
}
