using BrutalAPI;
using TevlevsRapscallionsNEW.Conditions;
using TevlevsRapscallionsNEW.Effects;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Characters
{
    public class Nails
    {
        public static void Add()
        {
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Nails", Pigments.Purple, EXOP._bimini.damageSound, EXOP._bimini.deathSound, EXOP._bimini.dxSound), "Nails");
            character.AddPassive(Passives.Delicate);

            #region ScriptableObjects

            LoadedDBsHandler._StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Scars_ID.ToString(), out StatusEffect_SO Scars);

            StatusEffect_Apply_Effect ApplyScars = ScriptableObject.CreateInstance<StatusEffect_Apply_Effect>();
            ApplyScars._Status = Scars;

            Heal_BasedOnStatus_Effect HealBaseOffScars = ScriptableObject.CreateInstance<Heal_BasedOnStatus_Effect>();
            HealBaseOffScars._Status = Scars;

            HealEffect HealthUsePrevious = ScriptableObject.CreateInstance<HealEffect>();
            HealthUsePrevious.usePreviousExitValue = true;

            #endregion ScriptableObjects

            Ability ability = new Ability("Art Of Pain", "ArtOfPain_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("SkillNailing");
            ability.Description = "Deal 2 damage to the Left ally. Heal this party member and the Left ally for the amount of damage done.\nMake Left ally perform a random ability.";
            ability.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Yellow };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = 2, targets = Targeting.Slot_AllyLeft },
                new EffectInfo() { effect = HealthUsePrevious, entryVariable = 1, targets = Targeting.Slot_SelfAndLeft },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<PerformRandomAbilityEffect>(), entryVariable = 1, targets = Targeting.Slot_AllyLeft },
            };
            ability.AnimationTarget = Targeting.Slot_AllyLeft;
            ability.Visuals = EXOP._fennec.rankedData[0].rankAbilities[1].ability.visuals;
            ability.AddIntentsToTarget(Targeting.Slot_AllyLeft, new string[] { "Damage_1_2", "Misc", "Heal_1_4" });
            ability.AddIntentsToTarget(Targeting.Slot_SelfAll, new string[] { "Heal_1_4" });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Art Of";
            scaledAbility.AddonName = new string[] { "Misery", "Torment", "War" };
            scaledAbility.Description = new string[]
            {
                "Deal 3 damage to the Left ally. Heal this party member and the Left ally for the amount of damage done.\nMake Left ally perform a random ability.",
                "Deal 5 damage to the Left ally. Heal this party member and the Left ally for the amount of damage done.\nMake Left ally perform a random ability.",
                "Deal 6 damage to the Left ally. Heal this party member and the Left ally for the amount of damage done.\nMake Left ally perform a random ability.",
            };
            scaledAbility.EntryValueScale[0] = new int[3] { 3, 5, 6 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(0, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Order, Do Something!", "Order,DoSomething!_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("SkillOrderCharge");
            ability2.Description = "Refresh the Right ally abilities. Inflicts 1 scar to the right ally and heal them for their amount of Scars.\n20% chance to refresh this party member's abilities.";
            ability2.Cost = new ManaColorSO[] { Pigments.Yellow, Pigments.Purple };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 1, targets = Targeting.Slot_AllyRight },
                new EffectInfo() { effect = ApplyScars, entryVariable = 1, targets = Targeting.Slot_AllyRight },
                new EffectInfo() { effect = HealBaseOffScars, entryVariable = 1, targets = Targeting.Slot_AllyRight },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot, condition = RandomChanceCondition.Chance(20)},
            };
            ability2.AnimationTarget = Targeting.Slot_AllyRight;
            ability2.Visuals = EXOP._clive.rankedData[0].rankAbilities[0].ability.visuals;
            ability2.AddIntentsToTarget(Targeting.Slot_AllyRight, new string[] { "Other_Refresh", "Status_Scars", "Heal_1_4" });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfAll, new string[] { "Other_Refresh" });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Order,";
            scaledAbility2.AddonName = new string[] { "Attack!", "Charge!", "KILL!" };
            scaledAbility2.Description = new string[]
            {
                "Refresh the Right ally abilities. Inflicts 1 scar to the right ally and heal them for their amount of Scars.\n25% chance to refresh this party member's abilities.",
                "Refresh the Right ally abilities. Inflicts 1 scar to the right ally and heal them for their amount of Scars.\n30% chance to refresh this party member's abilities.",
                "Refresh the Right ally abilities. Inflicts 1 scar to the right ally and heal them for their amount of Scars.\n40% chance to refresh this party member's abilities.",
            };
            scaledAbility2.Scale();
            scaledAbility2.abilities[0].ability.effects[3].condition = RandomChanceCondition.Chance(25);
            scaledAbility2.abilities[1].ability.effects[3].condition = RandomChanceCondition.Chance(30);
            scaledAbility2.abilities[2].ability.effects[3].condition = RandomChanceCondition.Chance(40);

            Ability ability3 = new Ability("Order, Run!", "Order,Run!_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("SkillOrderRetreat");
            ability3.Description = "Refreshes this party member and the Left and Right allies' movement. Heal all allies 1-2 health.\n30% chance to refresh this party member's abilites.";
            ability3.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Blue };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RestoreSwapUseEffect>(), entryVariable = 1, targets = Targeting.GenerateSlotTarget(new int[] { -1, 0, 1 }, true) },
                new EffectInfo() { effect = Heal_Random_Effect.Generate(1, 2), entryVariable = 2, targets = Targeting.Unit_AllAllies },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot, condition = RandomChanceCondition.Chance(30)},
            };
            ability3.AnimationTarget = Targeting.GenerateSlotTarget(new int[] { -1, 1 }, true);
            ability3.Visuals = EXOP._flaMinGoa.abilities[1].ability.visuals;
            ability3.AddIntentsToTarget(Targeting.GenerateSlotTarget(new int[] { -1, 0, 1 }, true), new string[] { "Other_RestoreMovement", });
            ability3.AddIntentsToTarget(Targeting.Unit_AllAllies, new string[] { "Heal_1_4" });
            ability3.AddIntentsToTarget(Targeting.Slot_SelfAll, new string[] { "Other_Refresh" });

            Ability ability4 = new Ability(ability3.ability, "Order,Move!_AB", new ManaColorSO[] { Pigments.Red, Pigments.Blue });
            ability4.Name = "Order, Move!";
            ability4.Description = "Refreshes all allies' movement. Heal all allies 2-3 health.\n30% chance to refresh this party member's abilites.";
            ability4.Effects[0].targets = Targeting.Unit_AllAllies;
            ability4.Effects[1].effect = Heal_Random_Effect.Generate(2, 3);
            ability4.Effects[2].condition = RandomChanceCondition.Chance(30);
            ability4.ability.intents[0].targets = Targeting.Unit_AllAllies;

            Ability ability5 = new Ability(ability3.ability, "Order,Retreat!_AB", new ManaColorSO[] { Pigments.Red, Pigments.Blue });
            ability5.Name = "Order, Retreat!";
            ability5.Description = "Refreshes all allies' movement. Heal all allies 2-4 health.\n35% chance to refresh this party member's abilites.";
            ability5.Effects[0].targets = Targeting.Unit_AllAllies;
            ability5.Effects[1].effect = Heal_Random_Effect.Generate(2, 4);
            ability5.Effects[2].condition = RandomChanceCondition.Chance(35);
            ability5.ability.intents[0].targets = Targeting.Unit_AllAllies;

            Ability ability6 = new Ability(ability3.ability, "Order,Scatter!", new ManaColorSO[] { Pigments.Red, Pigments.Blue });
            ability6.Name = "Order, Scatter!";
            ability6.Description = "Refreshes all allies' movement. Heal all allies 2-6 health.\n40% chance to refresh this party member's abilites.";
            ability6.Effects[0].targets = Targeting.Unit_AllAllies;
            ability6.Effects[1].effect = Heal_Random_Effect.Generate(2, 6);
            ability6.Effects[2].condition = RandomChanceCondition.Chance(40);
            ability6.ability.intents[1].intents[0] = IntentType_GameIDs.Heal_5_10.ToString();
            ability6.ability.intents[0].targets = Targeting.Unit_AllAllies;

            character.AddLevelData(9, new Ability[]
            {
                ability,
                ability2,
                ability3
            });
            character.AddLevelData(12, new Ability[]
            {
                scaledAbility.abilities[0],
                scaledAbility2.abilities[0],
                ability4
            });
            character.AddLevelData(16, new Ability[]
            {
                scaledAbility.abilities[1],
                scaledAbility2.abilities[1],
                ability5,
            });
            character.AddLevelData(18, new Ability[]
            {
                scaledAbility.abilities[2],
                scaledAbility2.abilities[2],
                ability6,
            });
            character.SetMenuCharacterAsFullSupport();
            character.AddCharacter(true);
        }
    }
}
