using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.ValueModifiers
{
    public class GilbertValueModifier : IntValueModifier
    {
        public readonly float percentage;

        public GilbertValueModifier(bool dmgDealt, int percentage)
            : base(dmgDealt ? 4 : 62)
        {
            this.percentage = Mathf.Max(percentage, 0);
        }

        public override int Modify(int value)
        {
            float f = percentage * (float)value / 100f;
            int num = Mathf.Max(1, Mathf.FloorToInt(f));
            return Math.Max(value - num, 1);
        }
    }
}
