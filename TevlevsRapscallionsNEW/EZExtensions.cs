﻿
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TevlevsRapscallionsNEW
{
    public static class EZExtensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            return array.Length == 0 ? default : array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T GetRandom<T>(this List<T> list)
        {
            return list.Count <= 0 ? default : list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T[] SelfArray<T>(this T self)
        {
            return new T[1] { self };
        }

        public static int GetStatus(this IUnit self, string StatusID)
        {
            if (self is IStatusEffector istatusEffector)
            {
                foreach (IStatusEffect statusEffect in istatusEffector.StatusEffects)
                {
                    if (statusEffect.StatusID == StatusID)
                        return statusEffect.StatusContent;
                }
            }
            return 0;
        }

        /*
        public static void AddToDollPool(WearableStaticModifierSetterSO abil)
        {
            CasterAddRandomExtraAbilityEffect effect = (LoadedAssetsHandler.GetCharcater("Doll_CH").passiveAbilities[0] as Connection_PerformEffectPassiveAbility).connectionEffects[1].effect as CasterAddRandomExtraAbilityEffect;
            switch (abil)
            {
                case BasicAbilityChange_Wearable_SMS changeWearableSms:
                    effect._slapData = new List<BasicAbilityChange_Wearable_SMS>(effect._slapData)
          {
            changeWearableSms
          }.ToArray();
                    break;
                case ExtraAbility_Wearable_SMS abilityWearableSms:
                    effect._extraData = new List<ExtraAbility_Wearable_SMS>(effect._extraData)
          {
            abilityWearableSms
          }.ToArray();
                    break;
            }
        }
        */

        public static Type[] GetAllDerived(Type baze)
        {
            List<Type> typeList = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (baze.IsAssignableFrom(type) && !typeList.Contains(type) && type != baze)
                        typeList.Add(type);
                }
            }
            return typeList.ToArray();
        }

        public static bool PCall(Action orig, string name = null)
        {
            try
            {
                orig();
            }
            catch
            {
                Debug.LogError(name != null ? name + " failed" : (object)(orig.ToString() + " failed"));
                return false;
            }
            return true;
        }
    }
}