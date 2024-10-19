using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomePassives.EmptyParasite
{
    public class EmptyFriendlyParasiteIntValueModifier : IntValueModifier
    {
        public readonly EmptyParasitePassiveAbility passive;

        public readonly IUnit unit;

        public EmptyFriendlyParasiteIntValueModifier(EmptyParasitePassiveAbility passive, IUnit unit)
            : base(54)
        {
            this.passive = passive;
            this.unit = unit;
        }

        public override int Modify(int value)
        {
            int num = passive.CalculateParasiteBlockModifier(unit, value);
            return Mathf.Max(0, value - num);
        }
    }
}
