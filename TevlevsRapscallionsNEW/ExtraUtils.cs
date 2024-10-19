using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TevlevsRapscallionsNEW.Effects;
using TevlevsRapscallionsNEW.CustomeTargetting;
using UnityEngine;
using MUtility;
using TevlevsRapscallionsNEW.Actions;
using TevlevsRapscallionsNEW.CustomePopUpsActions;
using TevlevsRapscallionsNEW.CustomePassives.EmptyParasite;
using DG.Tweening;
using TevlevsRapscallionsNEW.Conditions;

namespace TevlevsRapscallionsNEW
{
    public static class ExtraUtils
    {
        #region GilbertManager
        public static Dictionary<int, List<int>> Gilberts = new Dictionary<int, List<int>>();
        public static List<int> EnemyGilberID = new List<int>();

        public static void AddEnemyGilber(int GilbertID, int EnemyGilbertID)
        {
            if (GilbertID == -1)
                return;
            if (Gilberts.TryGetValue(GilbertID, out List<int> GilbertEnemies))
            {
                GilbertEnemies.Add(EnemyGilbertID);
                EnemyGilberID.Add(EnemyGilbertID);
                return;
            }
            Gilberts.Add(GilbertID, new List<int> { EnemyGilbertID });
            EnemyGilberID.Add(EnemyGilbertID);
        }

        public static void AddGilbert(int ID)
        {
            if (Gilberts.ContainsKey(ID)) return;
            Gilberts.Add(ID, new List<int>());
        }

        public static int ContainsEnemyGilbert(int ID)
        {
            foreach (KeyValuePair<int, List<int>> GilberPairs in Gilberts)
                if (GilberPairs.Value.Contains(ID))
                    return GilberPairs.Key;
            return -1;
        }

        public static bool TryRemoveEnemyGilbert(int ID, out int GilberID)
        {
            GilberID = -1;
            List<int> GilList = null;
            foreach (KeyValuePair<int, List<int>> GilberPairs in Gilberts)
            {
                if (GilberPairs.Value.Contains(ID))
                {
                    GilList = GilberPairs.Value;
                    GilberID = GilberPairs.Key;
                    return true;
                }
            }
            if (GilList != null) 
            {
                GilList.Remove(ID); 
                return TryRemoveEnemyGilbert(ID); 
            };
            return false;
        }

        public static void TryRemoveGilbert(int ID)
        {
            if (Gilberts.ContainsKey(ID)) Gilberts.Remove(ID);
        }

        private static bool TryRemoveEnemyGilbert(int ID)
        {
            if (EnemyGilberID.Contains(ID)) { EnemyGilberID.Remove(ID); return true; }
            return false;
        }

        #endregion GilbertManager

        #region GilbertAbility

        public static Dictionary<string, ExtraAbilityInfo> TranslatedAttacks = new Dictionary<string, ExtraAbilityInfo>();

        public static ExtraAbilityInfo GilbertAbilityTranslate(CombatAbility Ability)
        {
            if (TranslatedAttacks.TryGetValue(Ability.ability.name, out ExtraAbilityInfo SavedAbility))
                return SavedAbility;

            ExtraAbilityInfo extraAbilityInfo = new ExtraAbilityInfo();
            extraAbilityInfo.ability = Ability.ability.Clone();
            extraAbilityInfo.ability.name = extraAbilityInfo.ability.name + "GB";
            extraAbilityInfo.ability._locAbilityData = new StringPairData(extraAbilityInfo.ability._abilityName, "");
            extraAbilityInfo.ability._locID = "";
            extraAbilityInfo.rarity = LoadedDBsHandler.MiscDB.DefaultRarity;
            extraAbilityInfo.cost = (ManaColorSO[])Ability.cost.Clone();
            extraAbilityInfo.rarity.rarityValue = 5;
            extraAbilityInfo.ability.priority.priorityValue = 5;

            RemoveExtraAbilityFromEnemyEffect RemoveExtraAbility = ScriptableObject.CreateInstance<RemoveExtraAbilityFromEnemyEffect>();
            RemoveExtraAbility.ExtraAbility = extraAbilityInfo;

            List<EffectInfo> AddedEffect = extraAbilityInfo.ability.effects.ToList();
            AddedEffect.Add(new EffectInfo { effect = RemoveExtraAbility, entryVariable = 0, targets = Targeting.Slot_SelfSlot });
            extraAbilityInfo.ability.effects = AddedEffect.ToArray();

            extraAbilityInfo.ability._description = Regex.Replace(extraAbilityInfo.ability._description, "Opposing", "Left and Right");
            extraAbilityInfo.ability._description = Regex.Replace(extraAbilityInfo.ability._description, "opposing", "left and right");

            extraAbilityInfo.ability._locAbilityData.description = extraAbilityInfo.ability._description;

            for (int i = 0; i < extraAbilityInfo.ability.effects.Length; i++)
                if (extraAbilityInfo.ability.effects[i].targets != null)
                    extraAbilityInfo.ability.effects[i].targets = Targetting_GilbertFlip.Generate(extraAbilityInfo.ability.effects[i].targets);

            for (int i = 0; i < extraAbilityInfo.ability.intents.Count; i++)
                extraAbilityInfo.ability.intents[i].targets = Targetting_GilbertFlip.Generate(extraAbilityInfo.ability.intents[i].targets);

            extraAbilityInfo.ability.animationTarget = Targetting_GilbertFlip.Generate(extraAbilityInfo.ability.animationTarget);

            TranslatedAttacks.Add(Ability.ability.name, extraAbilityInfo);

            return extraAbilityInfo;
        }

        public static void AddGilbertActionsToTimeline(ITurn[] units, int[] abilitIDs)
        {
            Timeline_Base Timeline = CombatManager._instance._stats.timeline;
            List<TurnInfo> NewSortTurnInfo = new List<TurnInfo>();
            List<TurnInfo> OldSortTunrInfo = new List<TurnInfo>();

            NewSortTurnInfo.Add(CombatManager._instance._stats.timeline.Round[0]);

            if (CombatManager._instance._stats.timeline.Round.Count > 1)
                for (int i = 1; i < CombatManager._instance._stats.timeline.Round.Count; i++)
                {
                    if (EnemyGilberID.Contains(Timeline.Round[i].turnUnit.ID))
                        NewSortTurnInfo.Add(Timeline.Round[i]);
                    else
                        OldSortTunrInfo.Add(Timeline.Round[i]);
                }

            List<TurnUIInfo> NewSortUIInfo = new List<TurnUIInfo>();
           
            for (int i = 0; i < units.Length; i++)
            {
                TurnInfo NewTurnInfo = new TurnInfo(units[i], abilitIDs[i], false);
                NewSortTurnInfo.Add(NewTurnInfo);
                NewSortUIInfo.Add(NewTurnInfo.GenerateTurnUIInfo(NewSortTurnInfo.Count - 1, false));
                units[i].TurnsInTimeline++;
            }

            NewSortTurnInfo.AddRange(OldSortTunrInfo);
            Timeline.Round = NewSortTurnInfo;

            CombatManager._instance.AddUIAction(new GilbertUpdateTimelineVisualsAction(NewSortUIInfo.ToArray()));
            CombatManager.Instance.AddUIAction(new UpdateTimelinePointerUIAction(Timeline.CurrentTurn));

            AddGilbertActionsToTimeLineAction.ClearPending();
        }

        //TimelineLayoutHandler

        public static IEnumerator GiblertAddTimelineSlots(TurnUIInfo[] newTurns)
        {
            TimelineLayoutHandler_TurnBased TimelineHandler = CombatManager._instance._combatUI._TimelineHandler as TimelineLayoutHandler_TurnBased;

            TimelineInfo[] NewInfo = new TimelineInfo[newTurns.Length];

            List<TimelineInfo> NewSortTimelineinfo = new List<TimelineInfo>();
            List<TimelineInfo> OldSortTimelineinfo = new List<TimelineInfo>();

            NewSortTimelineinfo.Add(TimelineHandler.TimelineSlotInfo[0]);

            for (int i = 1; i < TimelineHandler.TimelineSlotInfo.Count; i++) 
            {
                if (EnemyGilberID.Contains(TimelineHandler.TimelineSlotInfo[i].enemyID))
                {
                    NewSortTimelineinfo.Add(TimelineHandler.TimelineSlotInfo[i]);
                }
                else
                    OldSortTimelineinfo.Add(TimelineHandler.TimelineSlotInfo[i]);
            }

            for (int i = 0; i < newTurns.Length; i++)
            {
                TurnUIInfo turnUIInfo = newTurns[i];
                EnemyCombatUIInfo enemyCombatUIInfo = TimelineHandler.EnemiesInCombat[turnUIInfo.enemyID];
                enemyCombatUIInfo.AddTimelineTurn(turnUIInfo);
                Sprite icon = (turnUIInfo.isSecret ? null : enemyCombatUIInfo.Portrait);
                AbilitySO ability = enemyCombatUIInfo.Abilities[turnUIInfo.abilitySlotID].ability;
                NewInfo[i] = new TimelineInfo(turnUIInfo, icon, ability);
                NewSortTimelineinfo.Add(NewInfo[i]);
            }

            //AbilityTimelineSlots
            /*
            List<int> CheckedEnemies = new List<int>();
            for (int i = 0; i < OldSortTimelineinfo.Count; i++)
            {
                if (CheckedEnemies.Contains(OldSortTimelineinfo[i].enemyID))
                    continue;
                if (TimelineHandler.EnemiesInCombat.TryGetValue(OldSortTimelineinfo[i].enemyID, out EnemyCombatUIInfo Enemy))
                    for (int a = 0; a < Enemy.AbilityTimelineSlots.Count; a++)
                        for (int j = 0; j < Enemy.AbilityTimelineSlots[a].Count; j++)
                            Enemy.AbilityTimelineSlots[a][j] += newTurns.Length;
                CheckedEnemies.Add(OldSortTimelineinfo[i].enemyID);
            }
            */

            NewSortTimelineinfo.AddRange(OldSortTimelineinfo);
            TimelineHandler.TimelineSlotInfo = NewSortTimelineinfo;

            yield return GilbertUpdateTimelineVisuals(NewInfo, newTurns);
        }

        public static IEnumerator GilbertUpdateTimelineVisuals(TimelineInfo[] Info, TurnUIInfo[] UIInfos)
        {
            TimelineZoneLayout TimelineLayout = CombatManager._instance._combatUI._timeline;
            List<TimelineSlotGroup> NewSlots = new List<TimelineSlotGroup>();

            //SetTimelinePositionsBasedOffID(TimelineLayout._slotsInUse);

            TimelineLayout._slotsInUse = SortTimelineSlotList(TimelineLayout._slotsInUse);

            for (int i = 0; i < Info.Length; i++)
            {

                TimelineInfo timelineInfo = Info[i];
                Sprite enemy;
                Sprite[] intents;
                Color[] spriteColors;

                if (timelineInfo.timelineIcon == null)
                {
                    enemy = TimelineLayout._blindTimelineIcon;
                    intents = null;
                    spriteColors = null;
                }
                else
                {
                    enemy = timelineInfo.timelineIcon;
                    intents = TimelineLayout.IntentHandler.GenerateSpritesFromAbility(timelineInfo.ability, casterIsCharacter: false, out spriteColors);
                }

                TimelineSlotGroup timelineSlotGroup = TimelineLayout.PrepareUnusedSlot(enemy, intents, spriteColors);
                timelineSlotGroup.SetSlotScale(false);
                timelineSlotGroup.SetActivation(false);

                for (int s = 0; s < TimelineLayout._slotsInUse.Count; s++)
                    if (UIInfos[i].timeSlotID == TimelineLayout._slotsInUse[s].slot.TimelineSlotID)
                        ShiftTimelineSlotsDown(TimelineLayout._slotsInUse, s);

                timelineSlotGroup.UpdateSlotID(UIInfos[i].timeSlotID);
                timelineSlotGroup.SetSiblingIndex(UIInfos[i].timeSlotID + 1);
                NewSlots.Add(timelineSlotGroup);

                TimelineLayout._slotsInUse = SortTimelineSlotList(TimelineLayout._slotsInUse);
            }

            for (int j = 0; j < NewSlots.Count; j++)
            {
                NewSlots[j].GenerateTweenScale(true, TimelineLayout._timelineMoveTime);
                NewSlots[j].SetActivation(true);
            }

            TimelineLayout.UpdateTimelineContentSize(TimelineLayout._slotsInUse.Count + 1);
            yield return TimelineLayout.UpdateTimelineBackgroundSize(TimelineLayout._slotsInUse.Count + 1);
        }

        public static void ShiftTimelineSlotsDown(List<TimelineSlotGroup> Slots, int StartIndex)
        {
            for (int i = Slots.Count - 2; i > StartIndex - 1; i--) 
            {
                Slots[i].UpdateSlotID(Slots[i].slot.TimelineSlotID + 1);
                Slots[i].SetSiblingIndex(Slots[i].slot.transform.GetSiblingIndex() + 1);
            }
        }

        /*
        public static void SetTimelinePositionsBasedOffID(List<TimelineSlotGroup> Slots)
        {
            for (int i = 0; i > Slots.Count - 1; i++)
                Slots[i].SetSiblingIndex(Slots[i].slot.TimelineSlotID + 2);
        }
        */


        public static List<TimelineSlotGroup> SortTimelineSlotList(List<TimelineSlotGroup> Slots)
        {
            TimelineSlotGroup[] SortedSlots = new TimelineSlotGroup[Slots.Count];
            List<int> UsedSlots = new List<int>();
            for (int i = 0; i < Slots.Count; i++)
            {
                if (!UsedSlots.Contains(Slots[i].slot.TimelineSlotID))
                { UsedSlots.Add(Slots[i].slot.TimelineSlotID); continue; }
                while (true)
                {
                    Slots[i].slot.TimelineSlotID++;
                    if (!UsedSlots.Contains(Slots[i].slot.TimelineSlotID))
                    { UsedSlots.Add(Slots[i].slot.TimelineSlotID); break; }
                }
            }
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i] == null)
                    continue;
                SortedSlots[Slots[i].slot.TimelineSlotID - 1] = Slots[i];
            }
            return SortedSlots.ToList();
        }

        /* //Adds another player timeline
         public static void AddSpecificEnemyAbilityToTimeLine(ITurn unit, int abilitID)
         {
            Timeline_Base Timeline = CombatManager._instance._stats.timeline;
            List<TurnInfo> NewSortTurnInfo = new List<TurnInfo>();
            List<TurnInfo> OldSortTunrInfo = new List<TurnInfo>();

            for (int i = 0; i < CombatManager._instance._stats.timeline.Round.Count; i++)
            {
                if (EnemyGilberID.Contains(Timeline.Round[i].turnUnit.ID))
                    NewSortTurnInfo.Add(Timeline.Round[i]);
                else
                    OldSortTunrInfo.Add(Timeline.Round[i]);
            }

            NewSortTurnInfo.Add(new TurnInfo(unit, abilitID, false));
            NewSortTurnInfo.AddRange(OldSortTunrInfo);
            unit.TurnsInTimeline++;

            Timeline.Round = NewSortTurnInfo;

            TurnUIInfo[] roundTurnUIInfo = Timeline.RoundTurnUIInfo;
            CombatManager.Instance.AddUIAction(new PopulateTimelineUIAction(roundTurnUIInfo));
            CombatManager.Instance.AddUIAction(new UpdateTimelinePointerUIAction(Timeline.CurrentTurn));
          } 
         */

        #endregion GilbertAbility

        #region Hooks

        public static void UseAbility(Action<CharacterCombat, int, FilledManaCost[]> orig, CharacterCombat self, int abilityID, FilledManaCost[] filledCost)
        {
            UseAbilityInfo Args = new UseAbilityInfo(self, self.CombatAbilities[abilityID], abilityID);
            CombatManager._instance.PostNotification("OnAbilityUsedInfo", self, Args);
            orig(self, abilityID, filledCost);
        }

        public static void InitializeCombat(Action<CombatManager> orig, CombatManager self)
        {
            Gilberts.Clear();
            EnemyGilberID.Clear();
            orig(self);
        }

        //Func<EnemyCombat, AbilitySO, bool> orig,
        public static bool TryPerformRandomAbility(EnemyCombat self, AbilitySO selectedAbility)
        {
            if (self.UnitTypes.Contains("GilbertType"))
                selectedAbility = GilbertAbilityTranslate(new CombatAbility(selectedAbility, new ManaColorSO[0])).ability;

            CombatManager.Instance.AddSubAction(new ShowAttackInformationUIAction(self.ID, self.IsUnitCharacter, selectedAbility.GetAbilityLocData().text));
            CombatManager.Instance.AddSubAction(new PlayAbilityAnimationAction(selectedAbility.visuals, selectedAbility.animationTarget, self));
            CombatManager.Instance.AddSubAction(new EffectAction(selectedAbility.effects, self));
            return true;
        }

        /*

             case "ScuttleFace":
             self.CharacterSprite = MainClass.ScuttleFuckYippee.GetRandom();
              break;
        */

        public static void TryUpdateLookAnimation(Action<CharacterInFieldLayout> orig, CharacterInFieldLayout self)
        {
            CombatStats Stats = CombatManager._instance._stats;
            if (!Stats.CharactersOnField.ContainsKey(self.CharacterID)) { orig(self); return; }
            switch (Stats.TryGetCharacterOnField(self.CharacterID)._currentName)
            {
                case "Lovebug":
                    self.CharacterSprite = MainClass.LoveBugYippee.GetRandom();
                    break;
            }
            orig(self);
        }

        #endregion Hooks

        #region Empty Parasitism/Mutualism 

        public static string EmptyP_ID => "EmptyParasite_ID"; public static string EmptyP_PA => "EmptyParasite_PA";
        public static string EmptyM_ID => "EmptyMutualism_ID"; public static string EmptyM_PA => "EmptyMutualism_PA";

        public static void GenerateEmptyParasitismAndMutualism()
        {
            /// Parasite

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(EmptyP_PA, "Parasite health: {0}", Color.magenta);

            RandomDamageBetweenPreviousAndEntryEffect ParasiteDamage = ScriptableObject.CreateInstance<RandomDamageBetweenPreviousAndEntryEffect>();
            ParasiteDamage._indirect = true;
            ParasiteDamage._ignoreShield = true;

            EmptyParasitePassiveAbility LoveBugEmptyParasite = ScriptableObject.CreateInstance<EmptyParasitePassiveAbility>();
            LoveBugEmptyParasite.m_PassiveID = EmptyP_ID;
            LoveBugEmptyParasite._passiveName = Passives.ParasiteParasitism._passiveName;
            LoveBugEmptyParasite.passiveIcon = Passives.ParasiteParasitism.passiveIcon;
            LoveBugEmptyParasite._enemyDescription = "Increases the damage received by 5% per point of Parasite. Parasite will decrease by the original unmutliplied damage amount. Parasite will remove 1-5 health from this enemies at the end of every turn and convert it into more Parasite.";
            LoveBugEmptyParasite.parasiteHealth_USD = EmptyP_PA;
            LoveBugEmptyParasite._isFriendly = false;
            LoveBugEmptyParasite.effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<DudEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ParasiteDamage, entryVariable = 5, targets = Targeting.Slot_SelfSlot },
                new EffectInfo() { effect = ScriptableObject.CreateInstance<ParasitismIncreaseValueEffect>(), entryVariable = 1, targets = Targeting.Slot_SelfSlot },
            };
            LoveBugEmptyParasite.specialStoredData = UnitStoreData.GetCustom_UnitStoreData(EmptyP_PA);

            LoadedDBsHandler.PassiveDB.AddNewPassive(LoveBugEmptyParasite.m_PassiveID, LoveBugEmptyParasite);

            /// Mutualism

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(EmptyM_PA, "Mutualism health: {0}", Color.magenta);

            EmptyParasitePassiveAbility MeatshotEmptyMutualism = ScriptableObject.CreateInstance<EmptyParasitePassiveAbility>();
            MeatshotEmptyMutualism.m_PassiveID = EmptyM_ID;
            MeatshotEmptyMutualism._passiveName = Passives.ParasiteMutualism._passiveName;
            MeatshotEmptyMutualism.passiveIcon = Passives.ParasiteMutualism.passiveIcon;
            MeatshotEmptyMutualism._enemyDescription = Passives.ParasiteMutualism._enemyDescription;
            MeatshotEmptyMutualism._characterDescription = Passives.ParasiteMutualism._characterDescription;
            MeatshotEmptyMutualism.parasiteHealth_USD = EmptyM_PA;
            MeatshotEmptyMutualism._isFriendly = true;
            MeatshotEmptyMutualism.effects = new EffectInfo[]
            {
                new EffectInfo() { effect = ScriptableObject.CreateInstance<HealEffect>(), entryVariable = 2, targets = Targeting.Slot_SelfSlot, condition = ConditUtils.RandomRangeCondit(1, 2) },
            };
            MeatshotEmptyMutualism.specialStoredData = UnitStoreData.GetCustom_UnitStoreData(EmptyM_PA);

            LoadedDBsHandler.PassiveDB.AddNewPassive(MeatshotEmptyMutualism.m_PassiveID, MeatshotEmptyMutualism);
        }

        public static int ApplyEmptyParasiteToUnit(this IUnit Unit, int Amount)
        {
            if (!Unit.ContainsPassiveAbility(EmptyP_ID))
                Unit.AddPassiveAbility(LoadedAssetsHandler.GetPassive(EmptyP_ID));

            Unit.TryGetStoredData(EmptyP_PA, out UnitStoreDataHolder holder, true);

            if (holder == null) return 0;

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Ruptured_ID.ToString(), out var effect);
            CombatManager._instance.AddSubAction(new BasicStatusEffectPopUp(Amount, Unit, effect.EffectInfo.AppliedSoundEvent, Passives.ParasiteParasitism.passiveIcon, StatusFieldEffectPopUpType.Number));

            holder.m_MainData += Amount;
            return Amount;
        }

        public static bool TryRemoveMutualismAmount(this IUnit Unit, int Amount)
        {
            if (!SortMutualism(Unit, out UnitStoreDataHolder MHolder)) return false;
            MHolder.m_MainData -= Amount;

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Ruptured_ID.ToString(), out var effect);
            CombatManager._instance.AddSubAction(new CustomeTextStatusEffectPopUp($"-{Amount}", Unit, effect.EffectInfo.RemovedSoundEvent, Passives.ParasiteMutualism.passiveIcon));

            return true;
        }

        public static bool ApplyEmptyMutualism(this IUnit Unit, int Amount)
        {
            if (!SortMutualism(Unit, out UnitStoreDataHolder MHolder))
            { Unit.AddPassiveAbility(LoadedAssetsHandler.GetPassive(EmptyM_ID)); Unit.TryGetStoredData("EmptyMutualismCurrentHealthPA", out MHolder, true); }

            MHolder.m_MainData += Amount;

            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Ruptured_ID.ToString(), out var effect);
            CombatManager._instance.AddSubAction(new BasicStatusEffectPopUp(Amount, Unit, effect.EffectInfo.AppliedSoundEvent, Passives.ParasiteMutualism.passiveIcon, StatusFieldEffectPopUpType.Number));

            return true;
        }

        public static bool SortMutualism(IUnit Unit, out UnitStoreDataHolder MHolder)
        {
            MHolder = null;

            if (Unit.TryGetStoredData("ParasiteCurrentHealthPA", out MHolder, false))
            {
                if (Unit.ContainsPassiveAbility(EmptyM_ID))
                    if (Unit.TryGetStoredData(EmptyP_PA, out UnitStoreDataHolder EmptyPHolder, false))
                        MHolder.m_MainData += EmptyPHolder.m_MainData;
            }
            else if (Unit.TryGetStoredData(EmptyP_PA, out UnitStoreDataHolder EmptHolder, false))
                MHolder = EmptHolder;

            return MHolder != null;
        }

        public static bool ConvertUnitToMutualism(this IUnit Unit, IUnit Target, ParasitePassiveAbility custMutulism = null)
        {
            if (Target == null || !Target.IsAlive) return false;

            ParasitePassiveAbility MPassive = null;
            if (custMutulism != null) MPassive = custMutulism;
            else MPassive = Passives.ParasiteMutualism as ParasitePassiveAbility;

            int EmptyMutualismAmount = 0;
            if (Unit.ContainsPassiveAbility(EmptyM_ID))
                if (Unit.TryGetStoredData("EmptyMutualismCurrentHealthPA", out UnitStoreDataHolder EmptyPHolder, true))
                {
                    EmptyMutualismAmount = EmptyPHolder.m_MainData;
                    Unit.TryRemovePassiveAbility(EmptyM_ID);
                }

            if (Target.ContainsPassiveAbility(MPassive.m_PassiveID))
                Unit.TryRemovePassiveAbility(MPassive.m_PassiveID);

            Unit.AddPassiveAbility(MPassive);
            Unit.TryGetStoredData("ParasiteCurrentHealthPA", out UnitStoreDataHolder MHPHolder, true);
            Unit.TryGetStoredData("ParasiteIDPA", out UnitStoreDataHolder MIDHolder, true);


            if (MHPHolder == null || MIDHolder == null || !TryBoxUnit(Unit, Target))
            { Unit.TryRemovePassiveAbility(MPassive.m_PassiveID); return false; }

            int MutualismHP = Mathf.Max(1, Target.CurrentHealth);
            int MutualismID = Target.IsUnitCharacter ? 1 + Target.ID : -(1 + Target.ID);

            MHPHolder.m_MainData = MutualismHP + EmptyMutualismAmount;
            MIDHolder.m_MainData = MutualismID;

            return true;
        }

        public static void ShowCaseEmptyParasiteOrMutualismRemoved(IUnit Unit, bool IsMutualism)
        {
            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(TempStatusEffectID.Ruptured_ID.ToString(), out var effect);
            CombatManager._instance.AddSubAction(new BasicStatusEffectPopUp(0, Unit, effect.EffectInfo.RemovedSoundEvent, IsMutualism ? Passives.ParasiteMutualism.passiveIcon : Passives.ParasiteParasitism.passiveIcon, StatusFieldEffectPopUpType.Removed));
        }

        #endregion Empty Parasitism/Mutualism 

        #region Effect

        public static EffectSO AutoSetFieldEffectEffects(this EffectSO effect, string fieldEffectID)
        {
            LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect(fieldEffectID, out FieldEffect_SO fieldEffect);
            foreach (FieldInfo field in effect.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == typeof(FieldEffect_SO))
                    field.SetValue(effect, fieldEffect);
            return effect;
        }

        public static EffectSO AutoSetStatusEffectEffects(this EffectSO effect, string statusEffectID)
        {
            LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(statusEffectID, out StatusEffect_SO statusEffect);
            foreach (FieldInfo field in effect.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == typeof(StatusEffect_SO))
                    field.SetValue(effect, statusEffect);
            return effect;
        }

        public static EffectSO AutoSetEffects<T>(this EffectSO effect, T obj)
        {
            foreach (FieldInfo field in effect.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == typeof(T))
                    field.SetValue(effect, obj);
            return effect;
        }

        public static T InitSetEffectOfType<T>(object obj) where T : EffectSO
        {
            return ScriptableObject.CreateInstance<T>().SetObjFieldsOfType<T>(obj);
        }

        public static T InitSetEffect<T>(object[] obj) where T : EffectSO
        {
            return ScriptableObject.CreateInstance<T>().SetObjFields<T>(obj);
        }

        public static T SetObjFields<T>(this T orgin, object[] obj)
        {
            FieldInfo[] fields = orgin.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            if (obj.Length > fields.Length) return orgin;
            for (int i = 0; i < obj.Length; i++)
                if (fields[i].FieldType.IsAssignableFrom(obj[i].GetType()))
                    fields[i].SetValue(orgin, obj[i]);
            return orgin;
        }

        public static T SetObjFieldsOfType<T>(this T orgin, object obj)
        {
            foreach (FieldInfo field in orgin.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
                if (field.FieldType == obj.GetType())
                    field.SetValue(orgin, obj);
            return orgin;
        }

        #endregion Effect

        #region Misc

        public static T TryCopySimilarFields<T>(this T orgin, object copy)
        {
            FieldInfo[] orginFields = orgin.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] copyFields = copy.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int o = 0; o < orginFields.Length; o++)
                for (int c = 0; c < copyFields.Length; c++)
                    if (orginFields[o].Name == copyFields[c].Name && orginFields[o].FieldType.IsAssignableFrom(copyFields[c].FieldType))
                        orginFields[o].SetValue(orgin, copyFields[c].GetValue(copy));

            return orgin;
        }

        public static EffectSO RandomRangeEffectGen(EffectSO effect, Vector2 range)
        {
            UseEffectRandomRangeEffect UseRange = ScriptableObject.CreateInstance<UseEffectRandomRangeEffect>();
            UseRange.Effect = effect;
            UseRange.Range = range;
            return UseRange;
        }

        public static bool TryBoxUnit(this IUnit Unit, IUnit Target)
        {
            if (!Unit.IsAlive || !Target.IsAlive) return false;

            CombatStats stats = CombatManager._instance._stats;
            UnboxOnNoParasite_UUH UnBoxHandler = ScriptableObject.CreateInstance<UnboxOnNoParasite_UUH>();

            CombatManager.Instance.AddUIAction(new PlaySpriteJumpUIAction(Target.UnitTurnSprite, stats.GenerateUnitJumpInformation(Target.ID, Target.IsUnitCharacter), Unit.ID, Unit.IsUnitCharacter));
            return Target.IsUnitCharacter ? stats.TryBoxCharacter(Target.ID, UnBoxHandler, "Boxing") : stats.TryBoxEnemy(Target.ID, UnBoxHandler, "Boxing");
        }

        public static IEnumerator StartStatusFieldShowcase(this PopUpHandler3D Handler, bool isFieldText, Vector3 position, string text, Sprite sprite)
        {
            List<CombatSpriteText> idleList;
            CombatSpriteText currentText = Handler.GetIdleSpriteText(isFieldText, out idleList);
            currentText.transform.position = position;
            Sequence t = Handler._statusTextOptions.PrepareSpriteTextOptions(currentText, text, sprite);
            t.OnComplete(delegate
            {
                Handler.FinalizeSpriteTextShowcase(currentText, idleList);
            });
            currentText.gameObject.SetActive(value: true);
            yield return t.WaitForPosition(Handler._StatusWaitTime);
        }

        #endregion Misc

        public static void SetUp()
        {
            new Hook(typeof(CharacterCombat).GetMethod("UseAbility", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("UseAbility", (BindingFlags)(-1)));
            new Hook(typeof(CombatManager).GetMethod("InitializeCombat", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("InitializeCombat", (BindingFlags)(-1)));
            new Hook(typeof(CharacterInFieldLayout).GetMethod("TryUpdateLookAnimation", (BindingFlags)(-1)), typeof(ExtraUtils).GetMethod("TryUpdateLookAnimation", (BindingFlags)(-1)));
            new Hook(typeof(EnemyCombat).GetMethod("TryPerformRandomAbility", (BindingFlags)(-1), null, new Type[] { typeof(AbilitySO) }, null), typeof(ExtraUtils).GetMethod("TryPerformRandomAbility", (BindingFlags)(-1)));

            GenerateEmptyParasitismAndMutualism();
        }
    }
}
