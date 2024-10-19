

namespace TevlevsRapscallionsNEW.Effects
{
    public class HealBasedOnStatusEffect : EffectSO
    {
        public StatusEffect_SO _Status;

        public bool usePreviousExitValue;
        public bool _directHeal = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (usePreviousExitValue) entryVariable *= PreviousExitValue;

            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].HasUnit || !targets[i].Unit.ContainsStatusEffect(_Status._StatusID) || targets[i].Unit.CurrentHealth == 0) continue;
                int Amount = entryVariable * targets[i].Unit.GetStatus(_Status._StatusID);
                exitAmount += targets[i].Unit.Heal(Amount, caster, _directHeal);
            }
            return exitAmount > 0;
        }

        /*
        public override bool PerformEffect(
          CombatStats stats,
          IUnit caster,
          TargetSlotInfo[] targets,
          bool areTargetSlots,
          int entryVariable,
          out int exitAmount)
        {
            if (usePreviousExitValue)
                entryVariable *= PreviousExitValue;
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && (!_onlyIfHasHealthOver0 || target.Unit.CurrentHealth > 0))
                {
                    int num = entryVariable * target.Unit.GetStatus(_status);
                    if (entryAsPercentage)
                        num = target.Unit.CalculatePercentualAmount(num);
                    exitAmount += target.Unit.Heal(num, (HealType)1, _directHeal);
                }
            }
            return exitAmount > 0;
        }
        */
    }
}
