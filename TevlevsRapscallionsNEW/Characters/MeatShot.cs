using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.Conditions;
using TevlevsRapscallionsNEW.CustomePassives.CustomeParasite;
using TevlevsRapscallionsNEW.CustomePassives.EmptyParasite;
using TevlevsRapscallionsNEW.Effects;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Characters
{
    public class MeatShot
    {
        public static void Add()
        {
            #region Intents

            IntentInfoBasic IntentPLB = new IntentInfoBasic();
            IntentPLB.id = "IntentMMS_ID";
            IntentPLB._sprite = Passives.ParasiteMutualism.passiveIcon;
            LoadedDBsHandler.IntentDB.AddNewBasicIntent(IntentPLB.id, IntentPLB);

            #endregion Intents

            #region SlapReplacement

            CustomeParasitePassiveAbility ConsumeAllyToMutualismMeatShot = ScriptableObject.CreateInstance<CustomeParasitePassiveAbility>();
            ConsumeAllyToMutualismMeatShot.TryCopySimilarFields(Passives.ParasiteMutualism);
            ConsumeAllyToMutualismMeatShot.m_PassiveID = "Parasite_MS";

            LoadedDBsHandler.PassiveDB.AddNewPassive(ConsumeAllyToMutualismMeatShot.m_PassiveID, ConsumeAllyToMutualismMeatShot);

            #endregion SlapReplacement

            #region Passive

            Apply_Mutualism_Effect ApplyEmptyMutualismPreviousEffect = ScriptableObject.CreateInstance<Apply_Mutualism_Effect>();
            ApplyEmptyMutualismPreviousEffect.ApplyEmptyMutualism = true;
            ApplyEmptyMutualismPreviousEffect.UsePrevious = true;

            Ability abilityS = new Ability("Flesh Recalibrate", "FleshRecalibrate_A");
            abilityS.AbilitySprite = ResourceLoader.LoadSprite("SkillFleshRecalibrate");
            abilityS.Description = "Convert 5 hp from this party member as mutualism.";
            abilityS.Cost = new ManaColorSO[1] { Pigments.Grey };
            abilityS.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Health_Reduce_Effect>(), entryVariable = 5, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ApplyEmptyMutualismPreviousEffect, entryVariable = 1, targets = Targeting.Slot_SelfSlot },
            };
            abilityS.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Damage_3_6", "IntentMMS_ID" });
            abilityS.Visuals = EXOP._unfinishedHeir.abilities[2].ability.visuals;

            #endregion passive

            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Meatshot", Pigments.Red, EXOP._xiphactinus.damageSound, EXOP._xiphactinus.deathSound, EXOP._xiphactinus.damageSound), "MeatShot");

            character.AddLevelData(22, new Ability[] { abilityS });

            character.AddLevelData(25, new Ability[] { });

            character.AddLevelData(30, new Ability[] { });

            character.AddLevelData(35, new Ability[] { });

            character.AddCharacter(true);

        }
    }
}
