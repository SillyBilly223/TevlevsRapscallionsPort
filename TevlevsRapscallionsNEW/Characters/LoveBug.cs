using BrutalAPI;
using FiendishFools.Effects;
using FiendishFools.Targetting;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.Conditions;
using TevlevsRapscallionsNEW.CustomePassives.EmptyParasite;
using TevlevsRapscallionsNEW.Effects;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Characters
{
    public class LoveBug
    {
        //public static int SavePosition;

        public static void Add()
        {
            #region Intents

            IntentInfoBasic IntentPLB = new IntentInfoBasic();
            IntentPLB.id = "IntentPLB_ID";
            IntentPLB._sprite = Passives.ParasiteParasitism.passiveIcon;
            LoadedDBsHandler.IntentDB.AddNewBasicIntent(IntentPLB.id, IntentPLB);

            #endregion Intents

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Lovebug", Pigments.Purple, EXOP._jumbleGutsWaning.damageSound, EXOP._jumbleGutsWaning.deathSound, EXOP._jumbleGutsWaning.damageSound), "Lovebug");
            character.AddPassive(Passives.Skittish2);

            #region ScriptableObjects

            SetEntryValueToIndexExitValueCondition GrabSecondEffectEntryValue = ScriptableObject.CreateInstance<SetEntryValueToIndexExitValueCondition>();
            GrabSecondEffectEntryValue.EffectIndex = 0;

            #endregion ScriptableObjects

            Ability ability = new Ability("Playful Peekabo", "PlayfulPeekabo_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillPeekabo");
            ability.Description = "Move Left or Right. Deal 4 damage to the Opposing enemy and apply Parasitism for the amount of damage done.\nAttempt to return to the original position.";
            ability.Cost = new ManaColorSO[] { Pigments.Yellow };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<SwapToSidesReturnSlotEffect>(), entryVariable = 3, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ParasiteProducingDamageEffect>(), entryVariable = 4, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<MoveToEntryValueSlotEffect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot, condition = GrabSecondEffectEntryValue },
            };
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.Visuals = EXOP._splig.rankedData[0].rankAbilities[2].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Damage_3_6.ToString(), "IntentPLB_ID" });
            ability.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Swap_Sides.ToString(), IntentType_GameIDs.Swap_Sides.ToString() });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3, true);
            scaledAbility.SetName = "Peekabo";
            scaledAbility.AddonName = new string[] { "Frivolous", "Effervescent", "Mirthful" };
            scaledAbility.SetFormatDescription("Move Left or Right. Deal {0} damage to the Opposing enemy and apply Parasitism for the amount of damage done.\nAttempt to return to the original position.", new object[] { 6, 8, 9 });
            scaledAbility.SetCostScaleFromIndex(0, new ManaColorSO[] { Pigments.RedYellow });
            scaledAbility.EntryValueScale[1] = new int[3] { 6, 8, 9 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Dappy Kissy Kissy", "DappyKissyKissy_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillKissyKissy");
            ability2.Description = "Apply 3 parasitism to the opposing enemy. Remove all Ruptured from this party member.\n50% chance to refresh this party member.";
            ability2.Cost = new ManaColorSO[] { Pigments.YellowPurple };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ApplyEmptyParasitismEffect>(), entryVariable = 3, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>().AutoSetStatusEffectEffects("Ruptured_ID"), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot, condition = RandomChanceCondition.Chance(20)},
            };
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.Visuals = EXOP._flaMinGoa.abilities[0].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "IntentPLB_ID" });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Rem_Status_Ruptured.ToString(), IntentType_GameIDs.Other_Refresh.ToString() });

            object[] LoveBugDesStatus = new object[3] { "Remove all Ruptured from this party member.", "Remove all negative status effects from this party member.", "Remove all negative status effects from this party member." };

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3, true);
            scaledAbility2.SetName = "Kissy Kissy";
            scaledAbility2.AddonName = new string[] { "Vagary", "Exuberant", "Jovial" };
            scaledAbility2.Description = new string[]
            {
                "Apply 4 parasitism to the opposing enemy. Remove all negative status effects from this party member.\n50% chance to refresh this party member.",
                "Apply 4 parasitism to the opposing enemy. Remove all negative status effects from this party member.\n50% chance to refresh this party member.",
                "Apply 5 parasitism to the opposing enemy. Remove all negative status effects from this party member.\n50% chance to refresh this party member.",
            };
            scaledAbility2.SetEffectScaleFromIndex(1, 0, ScriptableObject.CreateInstance<RemoveAllNegativeStatusEffectsEffect>());
            scaledAbility2.EntryValueScale[1] = new int[3] { 4, 4, 5 };
            scaledAbility2.Scale();

            //ContainsParasiteCondition
            Ability ability3 = new Ability("Jovial Pet", "JovialPet_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillPet");
            ability3.Description = "Deal 10 damage to the Opposing enemy.\nIf the Opposing enemy does not have parasitism, deal 6 damage to this party member.";
            ability3.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 10, targets = Targeting.Slot_Front },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 6, targets = Targeting.Slot_SelfSlot, condition = ScriptableObject.CreateInstance<ContainsParasiteCondition>() },
            };
            ability3.AnimationTarget = Targeting.Slot_Front;
            ability3.Visuals = EXOP._OsmanSinnoks.abilities[0].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.Slot_Front, new string[] { IntentType_GameIDs.Damage_7_10.ToString() });
            ability3.AddIntentsToTarget(Targetting_BySlot_Index_Conditional.Generate(new int[] { 0 }, true, false, ScriptableObject.CreateInstance<ContainsParasiteCondition>()), new string[] { IntentType_GameIDs.Damage_3_6.ToString() });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3, true);
            scaledAbility3.SetName = "Pet";
            scaledAbility3.AddonName = new string[] { "Jocund", "Joyous", "Radiant" };
            scaledAbility3.SetFormatDescription("Deal {0} damage to the Opposing enemy.\nIf the Opposing enemy does not have parasitism, deal 6 damage to this party member.", new object[] { 12, 16, 20 });
            scaledAbility3.EntryValueScale[0] = new int[3] { 12, 16, 20 };
            scaledAbility3.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility3.Scale();

            character.AddLevelData(14, new Ability[] { ability, ability2, ability3 });

            character.AddLevelData(17, ScaledAbility.GetscaleAbilities(new ScaledAbility[] { scaledAbility, scaledAbility2, scaledAbility3 } ));

            character.AddLevelData(20, ScaledAbility.GetscaleAbilities(new ScaledAbility[] { scaledAbility, scaledAbility2, scaledAbility3 } ));

            character.AddLevelData(25, ScaledAbility.GetscaleAbilities(new ScaledAbility[] { scaledAbility, scaledAbility2, scaledAbility3 } ));

            character.SetMenuCharacterAsFullDPS();
            character.AddCharacter(true);
        }
    }
}
