using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace TevlevsRapscallionsNEW.Actions
{
    public class AddGilbertActionsToTimeLineAction : CombatAction
    {
        private static List<ITurn> Units = new List<ITurn>();
        private static List<int> AbilityID = new List<int>();
        public static bool IsPending;

        public static void ClearPending()
        {
            Units.Clear();
            AbilityID.Clear();
            IsPending = false;
        }

        public static void AddToPending(ITurn unit, int abilityID)
        {
            Units.Add(unit);
            AbilityID.Add(abilityID);
        }

        public static void TryRemoveFromPending(ITurn unit)
        {
            for (int i = 0; i < Units.Count; i++)
                if (Units[i] == unit)
                { Units.RemoveAt(i); AbilityID.RemoveAt(i); }
        }

        public AddGilbertActionsToTimeLineAction(ITurn unit, int abilityID)
        {
            ClearPending();

            Units.Add(unit);
            AbilityID.Add(abilityID);
            IsPending = true;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return UnitsCheck(stats);
            if (Units.Count == 0) { ClearPending(); yield break; }

            ExtraUtils.AddGilbertActionsToTimeline(Units.ToArray(), AbilityID.ToArray());
            yield break;
        }

        public IEnumerator UnitsCheck(CombatStats stats)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                EnemyCombat Enemy = stats.TryGetEnemyOnField(Units[i].ID);
                if (Enemy == null || !Enemy.IsAlive || Enemy.CurrentHealth <= 0 || ExtraUtils.ContainsEnemyGilbert(Enemy.ID) == -1 )
                { Units.RemoveAt(i); AbilityID.RemoveAt(i); }
            }

            yield break;
        }
    }
}
