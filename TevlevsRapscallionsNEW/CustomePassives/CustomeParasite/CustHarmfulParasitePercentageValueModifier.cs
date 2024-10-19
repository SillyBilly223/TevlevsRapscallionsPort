using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomePassives.CustomeParasite
{
    public class CustHarmfulParasitePercentageValueModifier : IntValueModifier
    {
        public readonly CustomeParasitePassiveAbility passive;

        public readonly IUnit unit;

        public CustHarmfulParasitePercentageValueModifier(CustomeParasitePassiveAbility passive, IUnit unit)
            : base(64)
        {
            this.passive = passive;
            this.unit = unit;
        }

        public override int Modify(int value)
        {
            int num = passive.CalculateParasiteMultiplierModifier(unit, value);
            if (num <= 0 || value <= 0)
            {
                return value;
            }

            float f = num * value / 100;
            int num2 = Mathf.Max(1, Mathf.FloorToInt(f));
            return value + num2;
        }
    }
}
