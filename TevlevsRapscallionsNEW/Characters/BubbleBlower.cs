using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.Effects;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Characters
{
    public class BubbleBlower
    {
        
        public static void Add()
        {
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("BubbleBlower", Pigments.Red, EXOP._bimini.damageSound, EXOP._bimini.deathSound, EXOP._bimini.dxSound), "BubbleBlower");

            

            ScaledAbility scaledAbility2 = new ScaledAbility(new Ability("Bubble Spit", "BubbleSpit_AB") 
            {
                AbilitySprite = ResourceLoader.LoadSprite("SkillBubblespit"),
                Description = "Inflict 1-2 Bubbles to the left ally's position. Remove All Fire from the left ally's position\nIncreases all Bubbles on the ally side by 1-2.",
                Cost = new ManaColorSO[] { Pigments.Blue, Pigments.Grey },
                Effects = new EffectInfo[]
                {
                    new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 1, targets = Targeting.Slot_AllyRight },
                }
            }
            , 3, true);
            scaledAbility2.SetName = "Kissy Kissy";
            scaledAbility2.AddonName = new string[] { "Vagary", "Exuberant", "Jovial" };
            scaledAbility2.Description = new string[]
            {
                "Apply 4 parasitism to the opposing enemy. Remove all negative status effects from this party member.\n50% chance to refresh this party member.",
                "Apply 4 parasitism to the opposing enemy. Remove all negative status effects from this party member.\n50% chance to refresh this party member.",
                "Apply 5 parasitism to the opposing enemy. Remove all negative status effects from this party member.\n50% chance to refresh this party member.",
            };
            scaledAbility2.SetEffectScaleFromIndex(1, 0, ScriptableObject.CreateInstance<StatusEffect_RemoveAllNegative_Effect>());
            scaledAbility2.EntryValueScale[1] = new int[3] { 4, 4, 5 };
            scaledAbility2.Scale();
        }
    }
}
