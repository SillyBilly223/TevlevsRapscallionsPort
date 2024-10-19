using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace TevlevsRapscallionsNEW.CustomePassives.EmptyParasite
{
    public class EmptyParasitePassiveAbility : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => true;

        [UnitStoreValueNamesIDsEnumRef]
        public string parasiteHealth_USD = "EmptyParasiteCurrentHealthPA";

        public bool _isFriendly = true;

        public int _damagePercentage = 5;

        [EventRef]
        public string _parasiteShield = "";

        [Header("Passive Effects")]
        public EffectInfo[] effects = new EffectInfo[0];

        [Header("Passive Effects")]
        public bool connectionImmediateEffect = true;

        public bool disconnectionImmediateEffect = true;

        public EffectInfo[] connectionEffects = new EffectInfo[0];

        public EffectInfo[] disconnectionEffects = new EffectInfo[0];

        public override void TriggerPassive(object sender, object args)
        {
            IUnit caster = sender as IUnit;
            if (_isFriendly)
            {
                CombatManager.Instance.AddSubAction(new EffectAction(effects, caster));
            }
            else
            {
                CombatManager.Instance.AddSubAction(new EffectAction(effects, caster));
            }
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            if (connectionImmediateEffect)
            {
                CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(connectionEffects, unit), addToPreInit: true);
            }
            else
            {
                CombatManager.Instance.AddSubAction(new EffectAction(connectionEffects, unit));
            }
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            if (disconnectionImmediateEffect)
            {
                CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(disconnectionEffects, unit));
            }
            else
            {
                CombatManager.Instance.AddSubAction(new EffectAction(disconnectionEffects, unit));
            }
        }

        public override void CustomOnTriggerAttached(IPassiveEffector caller)
        {
            CombatManager.Instance.AddObserver(TriggerPassive, caller.IsUnitCharacter? TriggerCalls.OnTurnFinished.ToString() : TriggerCalls.OnRoundFinished.ToString(), caller);
            CombatManager.Instance.AddObserver(SecondCustomTryTriggerPassive, TriggerCalls.OnBeingDamaged.ToString(), caller);
        }

        public override void CustomOnTriggerDettached(IPassiveEffector caller)
        {
            CombatManager.Instance.RemoveObserver(TriggerPassive, caller.IsUnitCharacter ? TriggerCalls.OnTurnFinished.ToString() : TriggerCalls.OnRoundFinished.ToString(), caller);
            CombatManager.Instance.RemoveObserver(SecondCustomTryTriggerPassive, TriggerCalls.OnBeingDamaged.ToString(), caller);
        }

        public void SecondCustomTryTriggerPassive(object sender, object args)
        {
            IUnit unit = sender as IUnit;
            if (args is DamageReceivedValueChangeException ex && !ex.Equals(null) && ex.directDamage)
            {
                if (_isFriendly)
                {
                    ex.AddModifier(new EmptyFriendlyParasiteIntValueModifier(this, unit));
                }
                else
                {
                    ex.AddModifier(new EmptyHarmfulParasitePercentageValueModifier(this, unit));
                }
            }
        }

        public int CalculateParasiteBlockModifier(IUnit unit, int amount)
        {
            unit.TryGetStoredData(parasiteHealth_USD, out var holder);
            int mainData = holder.m_MainData;
            if (mainData <= 0 || amount <= 0)
            {
                return 0;
            }

            int num = Mathf.Min(mainData, amount);
            mainData -= num;
            CombatManager.Instance.AddUIAction(new PlayParasiteEffectUIAction(_parasiteShield, unit.FieldID, unit.IsUnitCharacter, num));
            holder.m_MainData = mainData;
            if (mainData <= 0)
                CombatManager._instance.AddRootAction(new RemoveEmptyParasiteAction(unit, this));

            return num;
        }

        public int CalculateParasiteMultiplierModifier(IUnit unit, int amount)
        {
            unit.TryGetStoredData(parasiteHealth_USD, out var holder);
            int mainData = holder.m_MainData;
            if (mainData <= 0 || amount <= 0)
            {
                return 0;
            }

            int result = mainData * _damagePercentage;
            int num = Mathf.Min(mainData, amount);
            mainData -= num;
            CombatManager.Instance.AddUIAction(new PlayParasiteEffectUIAction(_parasiteShield, unit.FieldID, unit.IsUnitCharacter, num));
            holder.m_MainData = mainData;
            if (mainData <= 0)
                CombatManager._instance.AddRootAction(new RemoveEmptyParasiteAction(unit, this));

            return result;
        }
    }

    public class RemoveEmptyParasiteAction : CombatAction
    {
        public IUnit Unit;

        public EmptyParasitePassiveAbility Passive;

        public RemoveEmptyParasiteAction(IUnit unit, EmptyParasitePassiveAbility passive)
        {
            Unit = unit;
            Passive = passive;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            Unit.TryRemovePassiveAbility(Passive.m_PassiveID); 
            ExtraUtils.ShowCaseEmptyParasiteOrMutualismRemoved(Unit, Passive._isFriendly);
            yield break;
        }
    }
}
