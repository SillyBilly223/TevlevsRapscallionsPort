using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FiendishFools.Targetting
{
    public class Targetting_BySlot_Index_Conditional : BaseCombatTargettingSO
    {
        public EffectConditionSO EffectCondition;

        public bool getAllies;

        public int[] slotPointerDirections;

        public bool allSelfSlots;

        public override bool AreTargetAllies => getAllies;

        public override bool AreTargetSlots => true;

        public bool CanGetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            TargetSlotInfo Caster = slots.GetCharacterTargetSlot(casterSlotID, 0);
            if (Caster == null || !Caster.HasUnit) return false;
            if (!EffectCondition.MeetCondition(Caster.Unit, null, 0)) return false;
            return true;
        }

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            if (!CanGetTargets(slots, casterSlotID, isCasterCharacter)) return new TargetSlotInfo[0];
            List<TargetSlotInfo> list = new List<TargetSlotInfo>();
            for (int i = 0; i < slotPointerDirections.Length; i++)
            {
                if (!getAllies && slotPointerDirections[i] == 0)
                {
                    list.AddRange(slots.GetFrontOpponentSlotTargets(casterSlotID, isCasterCharacter));
                }
                else if (allSelfSlots && getAllies && slotPointerDirections[i] == 0)
                {
                    list.AddRange(slots.GetAllSelfSlots(casterSlotID, isCasterCharacter));
                }
                else if (getAllies)
                {
                    TargetSlotInfo allySlotTarget = slots.GetAllySlotTarget(casterSlotID, slotPointerDirections[i], isCasterCharacter);
                    if (allySlotTarget != null)
                    {
                        list.Add(allySlotTarget);
                    }
                }
                else
                {
                    TargetSlotInfo allySlotTarget = slots.GetOpponentSlotTarget(casterSlotID, slotPointerDirections[i], isCasterCharacter);
                    if (allySlotTarget != null)
                    {
                        list.Add(allySlotTarget);
                    }
                }
            }

            return list.ToArray();
        }

        public static Targetting_BySlot_Index_Conditional Generate(int[] Index, bool TargetAllies, bool SelfSlots, EffectConditionSO Condition)
        {
            Targetting_BySlot_Index_Conditional Targetting = ScriptableObject.CreateInstance<Targetting_BySlot_Index_Conditional>();
            Targetting.EffectCondition = Condition;
            Targetting.slotPointerDirections = Index;
            Targetting.getAllies = TargetAllies;
            Targetting.allSelfSlots = SelfSlots;
            return Targetting;
        }
    }
}
