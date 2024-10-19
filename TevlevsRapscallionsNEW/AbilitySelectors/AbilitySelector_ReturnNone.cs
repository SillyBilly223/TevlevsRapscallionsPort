using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.AbilitySelectors
{
    public class AbilitySelector_ReturnNone : BaseAbilitySelectorSO
    {
        public override bool UsesRarity => false;

        public override int GetNextAbilitySlotUsage(List<CombatAbility> abilities, IUnit unit)
        {
            return -1;
        }
    }
}
