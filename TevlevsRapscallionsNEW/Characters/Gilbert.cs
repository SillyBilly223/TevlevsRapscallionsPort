using BrutalAPI;
using FiendishFools.Condition;
using System.Collections.Generic;
using System.Text;
using TevlevsRapscallionsNEW.AbilitySelectors;
using TevlevsRapscallionsNEW.CustomePassives;
using TevlevsRapscallionsNEW.CustomeTargetting;
using TevlevsRapscallionsNEW.Effects;
using UnityEngine;

namespace TevlevsRapscallionsNEW.Characters
{
    public static class Gilbert
    {
        public static CharacterSO Char_Gilbert;

        public static void Add()
        {
            #region Passives
            /*
            hanatosPassive thanatosPassive = ScriptableObject.CreateInstance<ThanatosPassive>();
            thanatosPassive._passiveName = "Deaths List";
            thanatosPassive.passiveIcon = ResourceLoader.LoadSprite("PassivesIconDeathsList");
            thanatosPassive.m_PassiveID = "Deaths List";
            thanatosPassive._enemyDescription = "Smelelr.";
            thanatosPassive._characterDescription = "If there no are no enemies inflicted with deaths touch, inflict deaths touch to a random enemy.";
            */

            //This enemy is Gilbert. This enemy will attempt to copy Gilbert's actions.\nGilbert deals 50% less damage to Gilbert.\nOn death, return this enemy's maximum health back to Gilbert

            GilbertPassive gilbertPassive = ScriptableObject.CreateInstance<GilbertPassive>();
            gilbertPassive._passiveName = "Gilbert";
            gilbertPassive.passiveIcon = ResourceLoader.LoadSprite("GilbertPassive");
            gilbertPassive._enemyDescription = "This enemy is NOT gilbert get this shit off of them.";
            gilbertPassive._characterDescription = "This party member is Gilbert. When this party member performs an action, Gilbert will attempt to copy it.\nGilbert deals 50% less damage to Gilbert.\nWhen Gilbert dies, their max health will be returned to this party member.";
            gilbertPassive.m_PassiveID = "GilbertAlly_ID";

            Passives.AddCustomPassiveToPool(gilbertPassive.m_PassiveID, "Gilbert", gilbertPassive);
            LoadedDBsHandler.GlossaryDB.AddNewPassive(new GlossaryPassives("Gilbert", "This party member/enemy is Gilbert. When a ally Gilbert performs an action, all enemy Gilbert's will attempt to copy.\nGilbert deals 50% less damage to Gilbert.\nWhen a enemy Gilbert dies, their max health will return to their original Gilbert.", ResourceLoader.LoadSprite("GilbertPassive")));

            #endregion Passives

            EnemySO EnemyRef = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN");
            Character character = EXOP.CharacterSpriteSetterAddon(EXOP.CharacterInfoSetter("Gilbert", Pigments.Red, EnemyRef.damageSound, EnemyRef.deathSound, EnemyRef.damageSound), "Gilbert");
            character.AddUnitType("GilbertType");
            character.AddPassive(gilbertPassive);

            #region ScriptableObjects

            IndexEffectConditon IfFirstEffectTrue = ScriptableObject.CreateInstance<IndexEffectConditon>();

            IndexEffectConditon IfFirstEffectFalse = ScriptableObject.CreateInstance<IndexEffectConditon>();
            IfFirstEffectFalse.wasSuccessful = false;

            PreviousEffectCondition PreviousEffectTrue = ScriptableObject.CreateInstance<PreviousEffectCondition>();

            DamageEffect UsePreviousDamageEffect = ScriptableObject.CreateInstance<DamageEffect>();
            UsePreviousDamageEffect._usePreviousExitValue = true;

            #endregion ScriptableObjects

            Ability ability = new Ability("Liquid Pummel", "LiquidPummel_AB");
            ability.AbilitySprite = ResourceLoader.LoadSprite("GilbertLiquidSkill");
            ability.Description = "If there is an Opposing enemy, deal 6 damage to them.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.";
            ability.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red };
            ability.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Check_HasOpponent_Effect>(), targets = Targeting.Slot_Front, entryVariable = 0, },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), targets = Targeting.Slot_Front, entryVariable = 6, condition = IfFirstEffectTrue },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Spawn_GilbertAnywhere_Effect>(), targets = Targeting.Slot_Front, entryVariable = 1, condition = IfFirstEffectFalse },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), targets = Targeting.Slot_SelfSlot, entryVariable = 1, condition = PreviousEffectTrue }
            };
            ability.Visuals = LoadedAssetsHandler.GetEnemyAbility("Bash_A").visuals;
            ability.AnimationTarget = Targeting.Slot_Front;
            ability.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6", IntentType_GameIDs.Other_Spawn.ToString() });
            ability.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Other_Refresh.ToString() });

            ScaledAbility scaledAbility = new ScaledAbility(ability, 3);
            scaledAbility.SetName = "Liquid";
            scaledAbility.AddonName = new string[] { "Fists", "Strike", "Beatwodn" };
            scaledAbility.Description = new string[]
            {
                "If there is an Opposing enemy, deal 8 damage to them.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.",
                "If there is an Opposing enemy, deal 10 damage to them.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.",
                "If there is an Opposing enemy, deal 12 damage to them.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.",
            };
            scaledAbility.EntryValueScale[1] = new int[3] { 8, 10, 12 };
            scaledAbility.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility.Scale();

            Ability ability2 = new Ability("Baloooo Bibidy", "BalooooBibidy_AB");
            ability2.AbilitySprite = ResourceLoader.LoadSprite("GilbertBalooSkill");
            ability2.Description = "If there is an Opposing enemy, deal 3 damage to them and heal this character the amount of damage dealt.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.";
            ability2.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Yellow };
            ability2.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Check_HasOpponent_Effect>(), targets = Targeting.Slot_Front, entryVariable = 0, },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Damage_Leech_Effect>(), targets = Targeting.Slot_Front, entryVariable = 3, condition = IfFirstEffectTrue },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Spawn_GilbertAnywhere_Effect>(), targets = Targeting.Slot_Front, entryVariable = 1, condition = IfFirstEffectFalse },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), targets = Targeting.Slot_SelfSlot, entryVariable = 1, condition = PreviousEffectTrue }
            };
            ability2.Visuals = LoadedAssetsHandler.GetCharacterAbility("Huff_1_A").visuals;
            ability2.AnimationTarget = Targeting.Slot_Front;
            ability2.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6", IntentType_GameIDs.Other_Spawn.ToString() });
            ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Heal_1_4.ToString(), IntentType_GameIDs.Other_Refresh.ToString() });

            ScaledAbility scaledAbility2 = new ScaledAbility(ability2, 3);
            scaledAbility2.SetName = "Baloooo";
            scaledAbility2.AddonName = new string[] { "Vavedour", "Elemas", "Bephelement" };
            scaledAbility2.Description = new string[]
            {
                "If there is an Opposing enemy, deal 4 damage to them and heal this character the amount of damage dealt.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.",
                "If there is an Opposing enemy, deal 5 damage to them and heal this character the amount of damage dealt.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.",
                "If there is an Opposing enemy, deal 6 damage to them and heal this character the amount of damage dealt.\nOtherwise, halve this party member's maximum health and summon a Gilbert, refreshing this party member's ability usage if successful.",
            };
            scaledAbility2.EntryValueScale[1] = new int[3] { 4, 5, 6 };
            scaledAbility2.intentTypeScale[0][0] = new ScaledAbility.IntentTypeScalePointer(1, IntentTypeScale.Damage);
            scaledAbility2.Scale();
            scaledAbility2.abilities[0].Cost = new ManaColorSO[] { Pigments.Red, Pigments.SplitPigment(Pigments.Red, Pigments.Yellow) };
            scaledAbility2.abilities[1].Cost = new ManaColorSO[] { Pigments.Red, Pigments.SplitPigment(Pigments.Red, Pigments.Yellow) };
            scaledAbility2.abilities[2].Cost = new ManaColorSO[] { Pigments.Red, Pigments.SplitPigment(Pigments.Red, Pigments.Yellow) };

            Ability ability3 = new Ability("Corpse Pyre", "CorpsePyre_AB");
            ability3.AbilitySprite = ResourceLoader.LoadSprite("GilbertPyreSkill");
            ability3.Description = "Increase this party member's maximum health by 1.\nIf the Opposing enemy is not Gilbert, destroy all Gilberts and deal damage to the Opposing enemy equivalent to the combined maximum health of all Gilberts destroyed.\nHeal this party member 4 health.";
            ability3.Cost = new ManaColorSO[] { Pigments.Red, Pigments.Red, Pigments.Red };
            ability3.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ChangeMaxHealthEffect>(), targets = Targeting.Slot_SelfSlot, entryVariable = 1, },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Misc_GilbertDestruct_Effect>(), targets = Targeting.Slot_Front, entryVariable = 0, },
                new EffectInfo() { effect = UsePreviousDamageEffect, targets = Targeting.Slot_Front, entryVariable = 1, condition = PreviousEffectTrue },
                Effect_SetAsSubAction_Effect.Gen(new EffectInfo() { effect = ScriptableObject.CreateInstance<HealEffect>(), targets = Targeting.Slot_SelfSlot, entryVariable = 4 })
            };
            ability3.Visuals = LoadedAssetsHandler.GetCharacterAbility("Amalgam_1_A").visuals;
            ability3.AnimationTarget = ScriptableObject.CreateInstance<Targetting_Gilberts>();
            ability3.AddIntentsToTarget(Targeting.Slot_Front, new string[] { "Damage_3_6" });
            ability3.AddIntentsToTarget(ScriptableObject.CreateInstance<Targetting_Gilberts>(), new string[] { IntentType_GameIDs.Damage_Death.ToString() });
            ability3.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Other_MaxHealth.ToString(), IntentType_GameIDs.Heal_1_4.ToString() });

            ScaledAbility scaledAbility3 = new ScaledAbility(ability3, 3, true);
            scaledAbility3.SetName = "Pyre";
            scaledAbility3.AddonName = new string[] { "Fluids", "Gore", "Funeral" };
            scaledAbility3.Description = new string[]
            {
                "Increase this party member's maximum health by 1.\nIf the Opposing enemy is not Gilbert, destroy all Gilberts and deal damage to the Opposing enemy equivalent to the combined maximum health of all Gilberts destroyed.\nHeal this party member 5 health.",
                "Increase this party member's maximum health by 2.\nIf the Opposing enemy is not Gilbert, destroy all Gilberts and deal damage to the Opposing enemy equivalent to the combined maximum health of all Gilberts destroyed.\nHeal this party member 6 health.",
                "Increase this party member's maximum health by 3.\nIf the Opposing enemy is not Gilbert, destroy all Gilberts and deal damage to the Opposing enemy equivalent to the combined maximum health of all Gilberts destroyed.\nHeal this party member 7 health.",
            };
            scaledAbility3.EntryValueScale[0] = new int[3] { 1, 2, 3 };
            scaledAbility3.EntryValueScale[3] = new int[3] { 5, 6, 7 };
            scaledAbility3.intentTypeScale[2][1] = new ScaledAbility.IntentTypeScalePointer(3, IntentTypeScale.Health);
            scaledAbility3.Scale();

            character.AddLevelData(16, new Ability[]
            {
                ability,
                ability2,
                ability3,
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
            character.AddLevelData(24, new Ability[]
            {
                scaledAbility.abilities[2],
                scaledAbility2.abilities[2],
                scaledAbility3.abilities[2],
            });
            character.SetMenuCharacterAsFullDPS();
            character.AddCharacter(true);
            Char_Gilbert = character.character;
        }

        public static List<EnemySO> EnemyGilbers = null;

        public static void SetUpEnemyGilberts()
        {
            if (EnemyGilbers != null) return;
            EnemyGilbers = new List<EnemySO>();

            GilbertPassive gilbertPassive = ScriptableObject.CreateInstance<GilbertPassive>();
            gilbertPassive._passiveName = "Gilbert";
            gilbertPassive.passiveIcon = ResourceLoader.LoadSprite("GilbertPassive");
            gilbertPassive._enemyDescription = "This enemy is Gilbert. This enemy will attempt to copy Gilbert's actions.\nGilbert deals 50% less damage to Gilbert.\nOn death, return this enemy's maximum health back to Gilbert.";
            gilbertPassive._characterDescription = "Gulp!";
            gilbertPassive.m_PassiveID = "GilbertEnemy_ID";
            gilbertPassive.IsEnemyGilbert = true;

            Passives.AddCustomPassiveToPool(gilbertPassive.m_PassiveID, "Gilbert", gilbertPassive);

            #region ScriptableObjects

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Focused_ID.ToString(), out StatusEffect_SO Focus);

            StatusEffect_Apply_Effect ApplyFocusEffect = ScriptableObject.CreateInstance<StatusEffect_Apply_Effect>();
            ApplyFocusEffect._Status = Focus;

            IndexEffectConditon IfThirdEffectTrue = ScriptableObject.CreateInstance<IndexEffectConditon>();
            IfThirdEffectTrue.EffectIndex = 2;

            #endregion ScriptableObjects

            #region UnChangedAbilities

            Ability FakeSlap = new Ability("Unintelligible Slap-Like Activities", "UnintelligibleSlap-LikeActivitiesEN_AB");
            FakeSlap.AbilitySprite = LoadedAssetsHandler.GetCharacterAbility("Slap_A").abilitySprite;
            FakeSlap.Description = "Perform a random ability from this enemy's moveset.";
            FakeSlap.Rarity.rarityValue = 5;
            FakeSlap.ability.priority.priorityValue = 5;
            FakeSlap.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<EnemyTurn_PerformRandomAction_Effect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
            };
            FakeSlap.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Misc_Hidden.ToString() });
            FakeSlap.Visuals = LoadedAssetsHandler.GetCharacterAbility("Slap_A").visuals;
            FakeSlap.AnimationTarget = Targeting.Slot_SelfSlot;

            Ability Schizoid = new Ability("Schizoid Homunculus", "SchizoidHomunculusEN_AB");
            Schizoid.AbilitySprite = ResourceLoader.LoadSprite("GilbertPyreSkill");
            Schizoid.Description = "This enemy gains Focused and increases its maximum health by 1-2. If there is an available space, halve this enemy's maximum health and summon a Gilbert with an equal amount of health.";
            Schizoid.Rarity.rarityValue = 5;
            Schizoid.ability.priority.priorityValue = 5;
            Schizoid.Effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ApplyFocusEffect, entryVariable = 1, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ExtraUtils.RandomRangeEffectGen(ScriptableObject.CreateInstance<ChangeMaxHealthEffect>(), new Vector2(1, 2)), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Check_IfAvaiableEnemySpawn_Effect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<Spawn_GilbertAnywhere_Effect>(), entryVariable = 0, targets = Targeting.Slot_SelfSlot, condition = IfThirdEffectTrue },
            };
            Schizoid.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { "Status_Focused", "Other_MaxHealth", "Other_Spawn" });
            Schizoid.Visuals = LoadedAssetsHandler.GetCharacterAbility("Amalgam_1_A").visuals;
            Schizoid.AnimationTarget = Targeting.Slot_SelfSlot;

            #endregion UnChangedAbilities

            for (int i = 0; i < 4; i++) 
            {
                Enemy enemy = EXOP.EnemyInfoSetter("Gilbert", 12, Pigments.Red, LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN"));
                enemy.enemy.name = "Gilbert_EN" + i.ToString();
                enemy.enemy.enemyTemplate = GilbertLayout;
                enemy.CombatSprite = ResourceLoader.LoadSprite("GilbertIcon");
                enemy.OverworldAliveSprite = ResourceLoader.LoadSprite("GilbertOverWorld", new Vector2?(new Vector2(0.5f, 0.05f)));
                enemy.OverworldDeadSprite = ResourceLoader.LoadSprite("GilbertOverWorld", new Vector2?(new Vector2(0.5f, 0.05f)));
                enemy.AddPassives(new BasePassiveAbilitySO[] { gilbertPassive, Passives.Withering });
                enemy.AddUnitType("GilbertType");
                enemy.AbilitySelector = ScriptableObject.CreateInstance<AbilitySelector_ReturnNone>();
                enemy.AddLootData(null);

                string LiquidNameScale = ScaleString(i, new string[] { "Pummel", "Fists", "Strike", "Beatwodn" });

                string Baloooo = ScaleString(i, new string[] { "Bibidy", "Vavedour", "Elemas", "Bephelement" });

                Ability Ability = new Ability("Liquid " + LiquidNameScale, "Liquid" + LiquidNameScale + "EN_AB");
                Ability.AbilitySprite = ResourceLoader.LoadSprite("GilbertLiquidSkill");
                Ability.Description = "Deal a " + ScaleString(i, new string[] { "Painful", "Agonizing", "Agonizing", "Deadly" }) + " amount of damage to the Left and Right enemies.";
                Ability.Rarity.rarityValue = 5;
                Ability.ability.priority.priorityValue = 5;
                Ability.Effects = new EffectInfo[] { new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = ScaleInt(i, new int[] { 6, 8, 10, 12 }), targets = Targeting.Slot_AllySides }, };
                Ability.AddIntentsToTarget(Targeting.Slot_AllySides, new string[] { EXOP.GetDamageIntent(Ability.Effects[0].entryVariable) });
                Ability.Visuals = LoadedAssetsHandler.GetEnemyAbility("Bash_A").visuals;
                Ability.AnimationTarget = Targeting.Slot_AllySides;

                Ability Ability2 = new Ability("Baloooo " + Baloooo, "Baloooo" + Baloooo + "EN_AB");
                Ability2.AbilitySprite = ResourceLoader.LoadSprite("GilbertBalooSkill");
                Ability2.Description = "Consume 6 random Pigment from the Pigment tray. Deal a " + ScaleString(i, new string[] { "Painful", "Painful", "Painful", "Agonizing" }) + " amount of damage to the enemies to the Left, Right, and Opposition of Gilbert.";
                Ability2.Rarity.rarityValue = 5;
                Ability2.ability.priority.priorityValue = 5;
                Ability2.Effects = new EffectInfo[]
                {
                    new EffectInfo() { effect = ScriptableObject.CreateInstance<ConsumeRandomManaEffect>(), entryVariable = 6, targets = Targeting.Slot_SelfSlot },
                    new EffectInfo() { effect = ScriptableObject.CreateInstance<DamageEffect>(), entryVariable = ScaleInt(i, new int[] { 4, 5, 6, 8 }), targets = Targetting_OrignGilbert.Generate(Targeting.GenerateSlotTarget(new int[] { -1, 0, 1 })) }
                };
                Ability2.AddIntentsToTarget(Targeting.Slot_SelfSlot, new string[] { IntentType_GameIDs.Mana_Consume.ToString() });
                Ability2.AddIntentsToTarget(Targetting_OrignGilbert.Generate(Targeting.GenerateSlotTarget(new int[] { -1, 0, 1 }, true)), new string[] { EXOP.GetDamageIntent(Ability2.Effects[1].entryVariable) });
                Ability2.Visuals = LoadedAssetsHandler.GetCharacterAbility("Huff_1_A").visuals;
                Ability2.AnimationTarget = Targetting_OrignGilbert.Generate(Targeting.GenerateSlotTarget(new int[] { -1, 0, 1 }));

                enemy.Abilities = new Ability[] { FakeSlap, Ability, Ability2, Schizoid };
                enemy.AddEnemy();
                EnemyGilbers.Add(enemy.enemy);
            }
        }

        public static EnemyInFieldLayout GilbertLayout
        {
            get
            {
                if (_GilbertLayout == null)
                {
                    GameObject gameObject = MainClass.assetBundle.LoadAsset<GameObject>("Assets/TevlevMod/EpicGilbert/Gilbert.prefab");
                    EnemyInFieldLayout enemyInFieldLayout = gameObject.AddComponent<EnemyInFieldLayout>();
                    EnemyInFieldLayout_Data enemyInFieldLayout_Data = gameObject.GetComponent<EnemyInFieldLayout_Data>();
                    if (enemyInFieldLayout_Data == null)
                    {
                        enemyInFieldLayout_Data = gameObject.AddComponent<EnemyInFieldLayout_Data>();
                        enemyInFieldLayout_Data.SetDefaultData();
                    }

                    enemyInFieldLayout.m_Data = enemyInFieldLayout_Data;
                    _GilbertLayout = enemyInFieldLayout;
                }
                return _GilbertLayout;
            }
        }

        public static EnemyInFieldLayout _GilbertLayout;

        public static string ScaleString(int Num, string[] Scale )
        {
            return Scale[Num];
        }

        public static int ScaleInt(int Num, int[] Scale)
        {
            return Scale[Num];
        }
    }
}
