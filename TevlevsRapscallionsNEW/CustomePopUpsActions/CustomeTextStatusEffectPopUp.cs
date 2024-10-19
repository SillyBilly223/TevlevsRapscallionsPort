using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomePopUpsActions
{
    public class CustomeTextStatusEffectPopUp : CombatAction
    {
        public string Text;
        public IUnit Unit;
        public string UpdateSound;
        public Sprite PopUpSprite;

        public CustomeTextStatusEffectPopUp(string text, IUnit unit, string updateSound, Sprite Sprite)
        {
            Text = text;
            Unit = unit;
            UpdateSound = updateSound;
            PopUpSprite = Sprite;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            if (Unit.IsUnitCharacter && CombatManager._instance._combatUI._charactersInCombat.TryGetValue(Unit.ID, out var character))
            {
                Vector3 characterPosition = CombatManager._instance._combatUI._characterZone.GetCharacterPosition(character.FieldID);
                CombatManager._instance._combatUI.PlaySoundOnPosition(UpdateSound, characterPosition);
                yield return CombatManager._instance._combatUI._popUpHandler3D.StartStatusFieldShowcase(false, characterPosition, Text, PopUpSprite);
            }
            else if (!Unit.IsUnitCharacter)
            {
                Vector3 characterPosition = CombatManager._instance._combatUI._enemyZone.CalculateEnemyPosition(Unit.SlotID, Unit.Size);
                CombatManager._instance._combatUI.PlaySoundOnPosition(UpdateSound, characterPosition);
                yield return CombatManager._instance._combatUI._popUpHandler3D.StartStatusFieldShowcase(true, characterPosition, Text, PopUpSprite);

            }
        }
    }
}
