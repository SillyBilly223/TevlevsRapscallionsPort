using BepInEx;
using BrutalAPI;
using System.Collections.Generic;
using UnityEngine;

namespace TevlevsRapscallionsNEW
{
    public class ScaledAbility
    {

        #region Base

        private Ability RefrenceAbility;

        public Ability[] abilities;
        public bool DebugMode;

        public int ScaleAmount;
        public int ReturnAbilityCounter = 0;

        #endregion Base

        #region ScaleVariables

        //ScaleName
        public string[] AddonName = new string[0];
        public string SetName = " Ability";
        public bool LeftName = false;

        //ScaleDescription
        public string[] Description = new string[0];
        public string Space = " ";

        //ScaleCost
        public ManaColorSO[][] CostScale = new ManaColorSO[0][];

        //ScaleEffect
        public int[][] EntryValueScale = new int[0][];
        public EffectSO[][] EffectScale = new EffectSO[0][];

        //ScaleIntent
        public IntentTypeScalePointer[][] intentTypeScale = new IntentTypeScalePointer[0][];

        #endregion ScaleVariables

        public ScaledAbility(Ability refAbility, int scale, bool isLeftName = false, bool ABNaemHasSpace = true)
        {
            ScaleAmount = scale;
            RefrenceAbility = refAbility;
            abilities = new Ability[ScaleAmount];

            //Name
            AddonName = new string[ScaleAmount];
            LeftName = isLeftName;

            //Description
            Description = new string[ScaleAmount];
            Space = (ABNaemHasSpace) ? " " : "";

            //Cost
            CostScale = new ManaColorSO[ScaleAmount][];

            //Effect
            EntryValueScale = new int[RefrenceAbility.Effects.Length][];
            EffectScale = new EffectSO[RefrenceAbility.Effects.Length][];

            //Intent
            intentTypeScale = new IntentTypeScalePointer[RefrenceAbility.EffectIntents.Count][];

            //ManaScale
            for (int i = 0; i < CostScale.Length; i++)
                CostScale[i] = (ManaColorSO[])refAbility.Cost.Clone();

            //EffectScale/EntryValueScale
            for (int i = 0; i < RefrenceAbility.Effects.Length; i++) 
            {
                EntryValueScale[i] = new int[ScaleAmount];
                EffectScale[i] = new EffectSO[ScaleAmount];
                for (int j = 0; j < ScaleAmount; j++)
                {
                    EffectScale[i][j] = RefrenceAbility.Effects[i].effect;
                }
            }

            //IntentTypeScale
            for (int i = 0; i < intentTypeScale.Length; i++)
            {
                intentTypeScale[i] = new IntentTypeScalePointer[RefrenceAbility.EffectIntents[i].intents.Length];
                for (int j = 0; j < intentTypeScale[i].Length; j++)
                {
                    intentTypeScale[i][j] = new IntentTypeScalePointer(-1, IntentTypeScale.None);
                }
            }
        }

        #region AbilityConstruct
        public ScaledAbility Scale()
        {
            List<Ability> abilities = new List<Ability>();
            for (int i = 0; i < ScaleAmount; i++)
            {
                SendDebug($"Scale {i}");
                string AbilityName = GetAbilityName(i);
                Ability ability = new Ability(RefrenceAbility.ability, EXOP.ReplaceWhitespace(AbilityName) + "_AB", RefrenceAbility.Cost);
                ability.Name = AbilityName;
                ability.Description = GetAbilityDescription(i);
                SendDebug($"Scale {i}: Cost {i} is null:{CostScale[i] == null}");
                ability.Cost = CostScale[i] != null ? CostScale[i] : (ManaColorSO[])RefrenceAbility.Cost.Clone();
                for (int a = 0; a < ability.Effects.Length; a++)
                {
                    SendDebug($"Scale {i}: Construct Effect {a}");
                    if (EffectScale[a] != null)
                    {
                        ability.Effects[a].effect = EffectScale[a][i];
                    }
                    if (EntryValueScale[a] != null && EntryValueScale[a][i] > 0)
                    {
                        ability.Effects[a].entryVariable = EntryValueScale[a][i];
                    }
                    CheckIfIntentScale(a, ability.Effects[a].entryVariable, ability);
                }
                abilities.Add(ability);
            }
            SendDebug("Ability Constructed");
            this.abilities = abilities.ToArray();
            return this;
        }


        private void CheckIfIntentScale(int abilityPointer, int amount ,Ability ability)
        {
            for (int i = 0; i < intentTypeScale.Length; i++)
            {
                for (int a = 0; a < intentTypeScale[i].Length; a++)
                {
                    if (intentTypeScale[i][a].AbilityPointer == abilityPointer && intentTypeScale[i][a].ScaleType != IntentTypeScale.None)
                    {
                        ability.EffectIntents[i].intents[a] = GetIntent(intentTypeScale[i][a].ScaleType, amount);
                    }
                }
            }
        }

        private string GetAbilityName(int ScaleAmount)
        {
            SendDebug($"Is LeftName? {LeftName}: {(LeftName ? AddonName[ScaleAmount] + Space + SetName : SetName + Space + AddonName[ScaleAmount])}");
            return LeftName? AddonName[ScaleAmount] + Space + SetName : SetName + Space + AddonName[ScaleAmount];
        }

        private string GetAbilityDescription(int ScaleAmount)
        {
            return Description[ScaleAmount];
        }

        private string GetIntent(IntentTypeScale intentTypeScale, int amount)
        {
            switch (intentTypeScale)
            {
                case IntentTypeScale.Damage:
                    return EXOP.GetDamageIntent(amount);
                case IntentTypeScale.Health:
                    return EXOP.GetHealIntent(amount);
                default:
                    return EXOP.GetDamageIntent(0);
            }
        }

        private void SendDebug(object text)
        {
            if (!DebugMode) return;
            Debug.Log(text.ToString());
        }

        public struct IntentTypeScalePointer
        {
            public int AbilityPointer;

            public IntentTypeScale ScaleType;

            public IntentTypeScalePointer(int ABPointer, IntentTypeScale IntentType)
            {
                AbilityPointer = ABPointer;
                ScaleType = IntentType;
            }
        }
        #endregion AbilityConstruct

        #region Addons
        public void SetCostScaleFromIndex(int index, ManaColorSO[] Cost)
        {
            if (index < 0 || index >= CostScale.Length)
            { Debug.LogWarning("SetCostScaleFromIndex index out of range"); return; }
            for (int i = index; i < CostScale.Length; i++)
                CostScale[i] = Cost;
        }

        public void SetEffectScaleFromIndex(int index, int startRange, EffectSO Effect)
        {
            if (index < 0 || index >= EffectScale.Length)
            { Debug.LogWarning("SetCostScaleFromIndex index out of range"); return; }
            for (int i = startRange; i < EffectScale[index].Length; i++)
                EffectScale[index][i] = Effect;
        }

        public void SetFormatDescription(string refrence, object[] Formats)
        {
            if (Formats.Length != Description.Length)
            { Debug.LogWarning("SetFormatDescription does not meet array length requirment"); return; }
            for (int i = 0; i < Formats.Length; i++)
                Description[i] = string.Format(refrence, Formats[i]);
        }

        public void SetFormatDescription(string refrence, object[][] Formats)
        {
            if (Formats.Length != Description.Length)
            { Debug.LogWarning("SetFormatDescription does not meet array length requirment"); return; }
            for (int i = 0; i < Formats.Length; i++)
                Description[i] = string.Format(refrence, Formats[i]);
        }

        public Ability GetScaleAbility()
        {
            if (abilities == null || abilities.Length == 0)
            { Debug.LogError("Cannot get scaled ability, either empty or null."); return null; }

            if (ReturnAbilityCounter >= abilities.Length) ReturnAbilityCounter = 0;
            Ability ReturnAbility = abilities[ReturnAbilityCounter];
            if (ReturnAbilityCounter + 1 >= abilities.Length) ReturnAbilityCounter = 0;
            else ReturnAbilityCounter++;
            return ReturnAbility;
        }

        public void ResetInternalScaleCounter()
        {
            ReturnAbilityCounter = 0;
        }

        public static Ability[] GetscaleAbilities(ScaledAbility[] ScaledAbilities)
        {
            Ability[] Abilities = new Ability[ScaledAbilities.Length];
            for (int i = 0; i < Abilities.Length; i++)
                Abilities[i] = ScaledAbilities[i].GetScaleAbility();
            return Abilities;
        }

        #endregion Addons
    }

    #region IntentTypeScale

    public enum IntentTypeScale
    {
        None,
        Damage,
        Health  
    }

    #endregion IntentTypeScale
}
