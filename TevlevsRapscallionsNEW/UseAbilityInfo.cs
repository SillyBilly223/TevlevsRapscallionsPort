using System;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW
{
    public class UseAbilityInfo
    {
        public IUnit User;

        public CombatAbility Ability;

        public int AbilityID;

        public UseAbilityInfo(IUnit user, CombatAbility ability, int ID)
        {
            User = user;
            Ability = ability;
            AbilityID = ID;
        }
    }
}
