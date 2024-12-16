using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.CustomeTargetting;
using TevlevsRapscallionsNEW.Effects;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Characters
{
    public class Brain
    {
        public static void Add()
        {
            #region ExtraSprites

            ExtraCCSprites_ArraySO BrainExtraCCSprites = ScriptableObject.CreateInstance<ExtraCCSprites_ArraySO>();
            BrainExtraCCSprites._DefaultID = "BrainDS_ID";
            BrainExtraCCSprites._SpecialID = "BrainSS_ID";
            BrainExtraCCSprites._doesLoop = false;
            BrainExtraCCSprites._backSprite = new Sprite[] { ResourceLoader.LoadSprite("BrianBackDamaged2"), ResourceLoader.LoadSprite("BrianBackDamaged3") };
            BrainExtraCCSprites._frontSprite = new Sprite[] { ResourceLoader.LoadSprite("BrianFrontDamaged2"), ResourceLoader.LoadSprite("BrianFrontDamaged3") };

            #endregion ExtraSprites

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Brian", Pigments.Purple, EXOP._Mobius_BOSS.damageSound, EXOP._Mobius_BOSS.deathSound, EXOP._Mobius_BOSS.damageSound), "Brian");
            character.ExtraSprites = BrainExtraCCSprites;

            //SetCasterExtraSpritesEffect

            Ability ability = new Ability("Cheeky Whack", "CheekyWhack_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillWhack");
            ability.Description = "Deal 3 damage to the Opposing enemy twice.\nApply 2 constricted to the Opposing enemy position.\nApplies 1 less constricted and deals 50% more damage against constricted enemies.";
            ability.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Damage_MoreIfConstricted_Effect>(), entryVariable = 3, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Damage_MoreIfConstricted_Effect>(), entryVariable = 3, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<FieldEffect_ApplyLessIfContain_Effect>().AutoSetFieldEffectEffects("Constricted_ID"), entryVariable = 2, targets = Targeting.Slot_Front },
            };
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.Visuals = EXOP._anton.rankedData[0].rankAbilities[1].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Damage_3_6.ToString(), IntentType_GameIDs.Damage_3_6.ToString(), IntentType_GameIDs.Field_Constricted.ToString() });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3, true);
            scaledAbility.SetName = "Whack";
            scaledAbility.AddonName = new string[] { "Miffed", "Bloody", "Bonkers" };
            scaledAbility.Description = new string[]
            {
                "Deal 4 damage to the Opposing enemy twice.\nApply 2 constricted to the Opposing enemy position.\nApplies 1 less constricted and deals 50% more damage against constricted enemies.",
                "Deal 5 damage to the Opposing enemy twice.\nApply 2 constricted to the Opposing enemy position.\nApplies 1 less constricted and deals 50% more damage against constricted enemies.",
                "Deal 6 damage to the Opposing enemy twice.\nApply 2 constricted to the Opposing enemy position.\nApplies 1 less constricted and deals 50% more damage against constricted enemies.",
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 4, 5, 6 };
            scaledAbility.EntryValueScale[1] = new int[3] { 4, 5, 6 };
            scaledAbility.EntryValueScale[2] = new int[3] { 2, 2, 2 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility.intentTypeScale[0][1] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Scant Cannonball", "ScantCannonball_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillCannonball");
            ability2.Description = "Deal an amount of damage equal to half of this party members current health to the Opposing enemy.\nIf the Opposing enemy is constricted instead deal damage equal to this party member's current health.";
            ability2.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Blue, Pigments.Yellow };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Damage_BasedOnHealthPlusConstricted_Effect>(), entryVariable = 0, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ExtraUtils.InitSetEffectOfType<SetCasterExtraSpritesEffect>("BrainSS_ID"), entryVariable = 0, targets = Targeting.Slot_Front, condition = BrutalAPI.Effects.CheckPreviousEffectCondition(true, 0) },
            };
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.Visuals = EXOP._flaMinGoa.abilities[2].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Damage_3_6.ToString() });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3, true);
            scaledAbility2.SetName = "Cannonball";
            scaledAbility2.AddonName = new string[] { "Chuffed", "Peng", "Barmy" };
            scaledAbility2.Description = new string[]
            {
                "Deal an amount of damage equal to half of this party members current health to the Opposing enemy.\nIf the Opposing enemy is constricted instead deal damage equal to this party member's current health.",
                "Deal an amount of damage equal to half of this party members current health to the Opposing enemy.\nIf the Opposing enemy is constricted instead deal damage equal to this party member's current health.",
                "Deal an amount of damage equal to half of this party members current health to the Opposing enemy.\nIf the Opposing enemy is constricted instead deal damage equal to this party member's current health.",
            };
            scaledAbility2.Scale();


            Targetting_WeakestOrStrongest TargetAllyWeakest = ScriptableObject.CreateInstance<Targetting_WeakestOrStrongest>();
            TargetAllyWeakest.getAllies = true;
            TargetAllyWeakest.targetWeakest = true;

            Targetting_Flip TargetAllyWeakestFlip = ScriptableObject.CreateInstance<Targetting_Flip>();
            TargetAllyWeakestFlip.OrginTargetting = TargetAllyWeakest;

            Ability ability3 = new Ability("Knackered Whistle", "KnackeredWhistle_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillWhistle");
            ability3.Description = "Shifts all stored pigment.\nApply 4 shield to the lowest health party member and Inflict 2 constricted to their Opposing.";
            ability3.Cost = new ManaColorSO[] { Pigments.Yellow, Pigments.Blue };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Pigments_Shift_Effect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<FieldEffect_Apply_Effect>().AutoSetFieldEffectEffects("Shield_ID"), entryVariable = 4, targets = TargetAllyWeakest },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<FieldEffect_Apply_Effect>().AutoSetFieldEffectEffects("Constricted_ID"), entryVariable = 2, targets = TargetAllyWeakestFlip },
            };
            ability3.AnimationTarget = TargetAllyWeakestFlip;
            ability3.Visuals = EXOP._agon.rankedData[0].rankAbilities[0].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Mana_Modify.ToString() });
            ability3.AddIntentsToTarget(TargetAllyWeakest, new string[] { IntentType_GameIDs.Field_Shield.ToString() });
            ability3.AddIntentsToTarget(TargetAllyWeakestFlip, new string[] { IntentType_GameIDs.Field_Constricted.ToString() });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3, true);
            scaledAbility3.SetName = "Whistle";
            scaledAbility3.AddonName = new string[] { "Antwackie", "Ace", "Gobby" };
            scaledAbility3.Description = new string[]
            {
                "Shifts all stored pigment.\nApply 6 shield to the lowest health party member and Inflict 2 constricted to their Opposing.",
                "Shifts all stored pigment.\nApply 8 shield to the lowest health party member and Inflict 2 constricted to their Opposing.",
                "Shifts all stored pigment.\nApply 11 shield to the lowest health party member and Inflict 2 constricted to their Opposing.",
            };
            scaledAbility3.EntryValueScale[1] = new int[3] { 6, 8, 11 };
            scaledAbility3.EntryValueScale[2] = new int[3] { 2, 2, 2 };
            scaledAbility3.Scale();

            character.AddLevelData(16, new Ability[] 
            { 
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(20, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                scaledAbility3.abilities[0],
            });
            character.AddLevelData(22, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                scaledAbility3.abilities[1],
            });
            character.AddLevelData(26, new Ability[]
            {
                scaledAbility.abilities[2],
                scaledAbility2.abilities[2],
                scaledAbility3.abilities[2],
            });

            character.AddCharacter(true);
        }
    }
}
