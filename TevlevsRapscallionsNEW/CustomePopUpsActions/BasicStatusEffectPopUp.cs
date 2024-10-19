using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomePopUpsActions
{
    public class BasicStatusEffectPopUp : CombatAction
    {
        public int Amount;
        public IUnit Unit;
        public string UpdateSound;
        public Sprite PopUpSprite;
        public StatusFieldEffectPopUpType PopUpType;

        public BasicStatusEffectPopUp(int amount, IUnit unit, string updateSound, Sprite Sprite, StatusFieldEffectPopUpType type)
        {
            Amount = amount;
            Unit = unit;
            UpdateSound = updateSound;
            PopUpSprite = Sprite;
            PopUpType = type;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            if (Unit.IsUnitCharacter && CombatManager._instance._combatUI._charactersInCombat.TryGetValue(Unit.ID, out var character))
            {
                Vector3 characterPosition = CombatManager._instance._combatUI._characterZone.GetCharacterPosition(character.FieldID);
                CombatManager._instance._combatUI.PlaySoundOnPosition(UpdateSound, characterPosition);
                yield return CombatManager._instance._combatUI._popUpHandler3D.StartStatusFieldShowcase(false, characterPosition, PopUpType, Amount, PopUpSprite);
            }
            else if (!Unit.IsUnitCharacter)
            {
                Vector3 characterPosition = CombatManager._instance._combatUI._enemyZone.CalculateEnemyPosition(Unit.SlotID, Unit.Size);
                CombatManager._instance._combatUI.PlaySoundOnPosition(UpdateSound, characterPosition);
                yield return CombatManager._instance._combatUI._popUpHandler3D.StartStatusFieldShowcase(true, characterPosition, PopUpType, Amount, PopUpSprite);

            }
        }
    }
}
