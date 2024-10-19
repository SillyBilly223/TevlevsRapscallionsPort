using JetBrains.Annotations;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TevlevsRapscallionsNEW.Conditions
{
    public class RandomChanceCondition : EffectConditionSO
    {
        public bool wasSuccessful = true;

        public float Percent;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return Random.Range(0, 101) <= Percent;
        }

        public static EffectConditionSO Chance(float percent)
        {
            RandomChanceCondition randomChanceCondition = ScriptableObject.CreateInstance<RandomChanceCondition>();
            randomChanceCondition.Percent = percent;
            return randomChanceCondition;
        }
    }
}
