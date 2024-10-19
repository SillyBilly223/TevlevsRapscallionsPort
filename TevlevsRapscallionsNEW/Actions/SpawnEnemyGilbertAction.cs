using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Actions
{
    public class SpawnEnemyGilbertAction : CombatAction
    {
        public int _preferredSlot;

        public EnemySO _enemy;

        public bool _givesExperience;

        public bool _trySpawnAnyways;

        public string _spawnSoundID;

        public int _maxHealth;

        public int _orginGilberID;

        public SpawnEnemyGilbertAction(EnemySO enemy, int orginGilberID, int maxHealth,  int preferredSlot, bool givesExperience, bool trySpawnAnyways, string spawnSoundID)
        {
            _preferredSlot = preferredSlot;
            _givesExperience = givesExperience;
            _enemy = enemy;
            _trySpawnAnyways = trySpawnAnyways;
            _spawnSoundID = spawnSoundID;
            _maxHealth = maxHealth;
            _orginGilberID = orginGilberID;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            int num;
            if (_preferredSlot >= 0)
            {
                num = stats.combatSlots.GetEnemyFitSlot(_preferredSlot, _enemy.size);
                if (num == -1 && _trySpawnAnyways)
                {
                    num = stats.GetRandomEnemySlot(_enemy.size);
                }
            }
            else
            {
                num = stats.GetRandomEnemySlot(_enemy.size);
            }

            if (num != -1)
            {
                SpawnEnemy(stats, _enemy, _orginGilberID, _maxHealth, num, _givesExperience, _spawnSoundID);
            }

            yield return null;
        }

        public void SpawnEnemy(CombatStats stats, EnemySO enemy, int orginGilbert, int maxHealth, int slot, bool givesExperience, string spawnSoundID)
        {
            int firstEmptyEnemyFieldID = stats.GetFirstEmptyEnemyFieldID();
            if (firstEmptyEnemyFieldID == -1)
            {
                return;
            }

            int count = stats.Enemies.Count;
            EnemyCombat enemyCombat = new EnemyCombat(count, firstEmptyEnemyFieldID, enemy, givesExperience);

            enemyCombat.MaximumHealth = maxHealth;
            enemyCombat.CurrentHealth = maxHealth;

            stats.Enemies.Add(count, enemyCombat);
            stats.AddEnemyToField(count, firstEmptyEnemyFieldID);
            stats.combatSlots.AddEnemyToSlot(enemyCombat, slot);
            stats.timeline.AddEnemyToTimeline(enemyCombat);

            CombatManager.Instance.AddUIAction(new EnemySpawnUIAction(enemyCombat.ID, spawnSoundID));

            enemyCombat.ConnectPassives();
            enemyCombat.InitializationEnd();

            ExtraUtils.AddEnemyGilber(orginGilbert, enemyCombat.ID);
        }
    }
}
