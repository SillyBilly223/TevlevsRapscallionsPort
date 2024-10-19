using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TevlevsRapscallionsNEW.CustomePassives.CustomeParasite
{
    public class CustomeParasitePassiveAbility : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => true;

        public bool _isFriendly = true;

        public bool connectionImmediateEffect = true;

        public bool disconnectionImmediateEffect = true;

        public string parasiteID_USD = "ParasiteIDPA";

        public string parasiteHealth_USD = "ParasiteCurrentHealthPA";

        public string _parasiteShield = "";

        public int _damagePercentage = 5;

        public EffectInfo[] effects;

        public EffectInfo[] connectionEffects;

        public EffectInfo[] disconnectionEffects;

        public TriggerCalls _secondTriggerOn = TriggerCalls.OnBeingDamaged;

        public TriggerCalls _thirdTriggerOn = TriggerCalls.OnCombatEnd;

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

            if (!unit.HasFled)
            {
                CombatManager.Instance.AddSubAction(new ParasiteDisconnectedAction(unit));
                CombatManager.Instance.PostNotification(TriggerCalls.OnParasiteDisconnected.ToString(), unit, null);
            }
            else
            {
                CombatManager.Instance.AddSubAction(new ParasiteDisconnectedByFleetingAction(unit, parasiteID_USD, parasiteHealth_USD));
            }
        }

        public override void CustomOnTriggerAttached(IPassiveEffector caller)
        {
            if (_secondTriggerOn != TriggerCalls.Count)
            {
                CombatManager.Instance.AddObserver(SecondCustomTryTriggerPassive, _secondTriggerOn.ToString(), caller);
            }

            if (_thirdTriggerOn != TriggerCalls.Count)
            {
                CombatManager.Instance.AddObserver(ThirdCustomTryTriggerPassive, _thirdTriggerOn.ToString(), caller);
            }
        }

        public override void CustomOnTriggerDettached(IPassiveEffector caller)
        {
            if (_secondTriggerOn != TriggerCalls.Count)
            {
                CombatManager.Instance.RemoveObserver(SecondCustomTryTriggerPassive, _secondTriggerOn.ToString(), caller);
            }

            if (_thirdTriggerOn != TriggerCalls.Count)
            {
                CombatManager.Instance.RemoveObserver(ThirdCustomTryTriggerPassive, _thirdTriggerOn.ToString(), caller);
            }
        }

        public void SecondCustomTryTriggerPassive(object sender, object args)
        {
            IUnit unit = sender as IUnit;
            if (args is DamageReceivedValueChangeException ex && !ex.Equals(null) && ex.directDamage)
            {
                if (_isFriendly)
                    ex.AddModifier(new CustFriendlyParasiteIntValueModifier(this, unit));
                else
                    ex.AddModifier(new CustHarmfulParasitePercentageValueModifier(this, unit));
            }
        }

        public void ThirdCustomTryTriggerPassive(object sender, object args)
        {
            IUnit unit = sender as IUnit;
            CombatManager.Instance.AddSubAction(new ParasiteCombatEndAction(unit, parasiteID_USD, parasiteHealth_USD));
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
            {
                unit.TryRemovePassiveAbility(m_PassiveID);
            }

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
            {
                unit.TryRemovePassiveAbility(m_PassiveID);
            }

            return result;
        }
    }
}
