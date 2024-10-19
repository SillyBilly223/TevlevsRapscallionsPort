using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TevlevsRapscallionsNEW.Actions;
using UnityEngine;
using Yarn;

namespace TevlevsRapscallionsNEW.Effects
{
    public class SpawnGilbertAnywhereEffect : EffectSO
    {
        [CombatIDsEnumRef]
        public string _spawnSoundID = "Basic";

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = entryVariable;
            EnemySO enemy = LoadedAssetsHandler.GetEnemy("Gilbert_EN" + TryGetGilberSpawnLevel(stats, caster).ToString());

            int CaculatedHalfAmount = Mathf.RoundToInt(caster.MaximumHealth / 2);
            caster.MaximizeHealth(Math.Max(CaculatedHalfAmount, 1));

            int OrginID = SetEnemyGilbertAsChildOfOrginGilbert(caster);

            CombatManager.Instance.AddSubAction(new SpawnEnemyGilbertAction(enemy, OrginID, CaculatedHalfAmount, caster.IsUnitCharacter? caster.SlotID : -1, false, true, _spawnSoundID));

            return true;
        }

        public int TryGetGilberSpawnLevel(CombatStats stats, IUnit Caster)
        {
            if (Caster.IsUnitCharacter)
            {
                CharacterCombat Char = stats.TryGetCharacter(Caster.ID);
                if (Char != null)
                    return Char.Rank;
            }
            else
            {
                EnemyCombat Enemy = stats.TryGetEnemyOnField(Caster.ID);
                if (Enemy != null)
                    switch (Enemy.Enemy.name)
                    {
                        case "Gilbert_EN0":
                            return 0;
                        case "Gilbert_EN1":
                            return 1;
                        case "Gilbert_EN2":
                            return 2;
                        case "Gilbert_EN3":
                            return 3;
                    }
            }
            return 0;
        }

        public int SetEnemyGilbertAsChildOfOrginGilbert(IUnit OrginGilbert)
        {
            if (!OrginGilbert.UnitTypes.Contains("GilbertType"))
                return -1;
            if (OrginGilbert.IsUnitCharacter)
            {
                if (ExtraUtils.Gilberts.ContainsKey(OrginGilbert.ID))
                    return OrginGilbert.ID;
                ExtraUtils.AddGilbert(OrginGilbert.ID);
                return OrginGilbert.ID;
            }
            else
            {
                int OrginID = ExtraUtils.ContainsEnemyGilbert(OrginGilbert.ID);
                if (OrginID != -1)
                    return OrginID;
                return -1;
            }
        }
    }
}
