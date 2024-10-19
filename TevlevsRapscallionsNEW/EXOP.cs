using BrutalAPI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TevlevsRapscallionsNEW
{
    public class EXOP
    {
        #region EnemyInfoSetters
        public static Enemy EnemyInfoSetter(string Name, int Health, ManaColorSO HealthColor, string DamageSound, string DeathSound, int Size = 1)
        {
            Enemy enemy = new Enemy(Name, ReplaceWhitespace(Name) + "_EN");
            enemy.Health = Health;
            enemy.HealthColor = HealthColor;
            enemy.DamageSound = DamageSound;
            enemy.DeathSound = DeathSound;
            enemy.Size = Size;
            return enemy;
        }

        public static Enemy EnemyInfoSetter(string Name, int Health, ManaColorSO HealthColor, EnemySO SoundRef, int Size = 1)
        {
            Enemy enemy = new Enemy(Name, ReplaceWhitespace(Name) + "_EN");
            enemy.Health = Health;
            enemy.HealthColor = HealthColor;
            enemy.DamageSound = SoundRef.damageSound;
            enemy.DeathSound = SoundRef.deathSound;
            enemy.Size = Size;
            return enemy;
        }

        public static Enemy EnemyInfoSetter(string Name, int Health, ManaColorSO HealthColor, CharacterSO SoundRef, int Size = 1)
        {
            Enemy enemy = new Enemy(Name, ReplaceWhitespace(Name) + "_EN");
            enemy.Health = Health;
            enemy.HealthColor = HealthColor;
            enemy.DamageSound = SoundRef.damageSound;
            enemy.DeathSound = SoundRef.deathSound;
            enemy.Size = Size;
            return enemy;
        }

        #endregion EnemyInfoSetters

        public static Character CharacterInfoSetter(string Name, ManaColorSO Health, string DamageSound, string DeathSound, string DialougeSound = "")
        {
            Character character = new Character(Name, ReplaceWhitespace(Name) + "_CH");
            character.HealthColor = Health;
            character.DamageSound = DamageSound;
            character.DeathSound = DeathSound;
            character.DialogueSound = DialougeSound == "" ? _nowak.dxSound : DialougeSound;
            return character;
        }

        public static Character CharacterSpriteSetterAddon(Character character, string Sprites)
        {
            character.FrontSprite = ResourceLoader.LoadSprite(Sprites + "Front");
            character.BackSprite = ResourceLoader.LoadSprite(Sprites + "Back");
            character.OverworldSprite = ResourceLoader.LoadSprite(Sprites + "OverWorld", new Vector2?(new Vector2(0.5f, 0f)));
            character.GenerateMenuCharacter(ResourceLoader.LoadSprite(Sprites + "Menu"), ResourceLoader.LoadSprite(Sprites + "Menu"));
            return character;
        }

        public static bool IsCharacterCheck(string Name)
        {
            char[] Chars = Name.ToCharArray();
            return (Chars[Chars.Length - 1] == 'H' && Chars[Chars.Length - 2] == 'C');
        }

        private static readonly Regex Whitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement = "")
        {
            return Whitespace.Replace(input, replacement);
        }

        public static Character CharacterSpriteSetterAddon(Character character, string Sprites, string Menu)
        {
            character.FrontSprite = ResourceLoader.LoadSprite(Sprites + "Front");
            character.BackSprite = ResourceLoader.LoadSprite(Sprites + "Back");
            character.OverworldSprite = ResourceLoader.LoadSprite(Sprites + "OverWorld", new Vector2?(new Vector2(0.5f, 0f)));
            character.GenerateMenuCharacter(ResourceLoader.LoadSprite(Menu), ResourceLoader.LoadSprite(Menu));
            return character;
        }

        public static string GetDamageIntent(int entryvalue)
        {
            if (entryvalue < 3) return "Damage_1_2";
            if (entryvalue < 7 && entryvalue > 2) return "Damage_3_6";
            if (entryvalue < 11 && entryvalue > 6) return "Damage_7_10";
            if (entryvalue < 16 && entryvalue > 10) return "Damage_11_15";
            if (entryvalue < 21 && entryvalue > 15) return "Damage_16_20";
            return "Damage_21";
        }

        public static string GetHealIntent(int entryvalue)
        {
            if (entryvalue < 4) return "Heal_1_4";
            if (entryvalue < 11 && entryvalue > 4) return "Heal_5_10";
            if (entryvalue < 21 && entryvalue > 10) return "Heal_11_20";
            return "Heal_21";
        }

        #region Refs

        [Header("BaseGameEnemies")]
        public static readonly EnemySO _mungEN = LoadedAssetsHandler.GetEnemy("Mung_EN");
        public static readonly EnemySO _musicMan = LoadedAssetsHandler.GetEnemy("MusicMan_EN");
        public static readonly EnemySO _Scrungie = LoadedAssetsHandler.GetEnemy("Scrungie_EN");
        public static readonly EnemySO _revola = LoadedAssetsHandler.GetEnemy("Revola_EN");
        public static readonly EnemySO _mudLung = LoadedAssetsHandler.GetEnemy("MudLung_EN");
        public static readonly EnemySO _flaMinGoa = LoadedAssetsHandler.GetEnemy("FlaMinGoa_EN");
        public static readonly EnemySO _keko = LoadedAssetsHandler.GetEnemy("Keko_EN");
        public static readonly EnemySO _wringle = LoadedAssetsHandler.GetEnemy("Wringle_EN");
        public static readonly EnemySO _vaboola = LoadedAssetsHandler.GetEnemy("Voboola_EN");
        public static readonly EnemySO _kekastle = LoadedAssetsHandler.GetEnemy("Kekastle_EN");
        public static readonly EnemySO _munglingMudLung = LoadedAssetsHandler.GetEnemy("MunglingMudLung_EN");
        public static readonly EnemySO _flarb = LoadedAssetsHandler.GetEnemy("Flarb_EN");
        public static readonly EnemySO _silverSuckle = LoadedAssetsHandler.GetEnemy("SilverSuckle_EN");
        public static readonly EnemySO _jumbleGutsHollowing = LoadedAssetsHandler.GetEnemy("JumbleGuts_Hollowing_EN");
        public static readonly EnemySO _jumbleGutsClotted = LoadedAssetsHandler.GetEnemy("JumbleGuts_Clotted_EN");
        public static readonly EnemySO _jumbleGutsWaning = LoadedAssetsHandler.GetEnemy("JumbleGuts_Waning_EN");
        public static readonly EnemySO _jumbleGutsFlummoxing = LoadedAssetsHandler.GetEnemy("JumbleGuts_Flummoxing_EN");
        public static readonly EnemySO _spoggleResonant = LoadedAssetsHandler.GetEnemy("Spoggle_Resonant_EN");
        public static readonly EnemySO _spoggleSpitFire = LoadedAssetsHandler.GetEnemy("Spoggle_Spitfire_EN");
        public static readonly EnemySO _spoggleWrithing = LoadedAssetsHandler.GetEnemy("Spoggle_Writhing _EN");
        public static readonly EnemySO _spoggleRuminating = LoadedAssetsHandler.GetEnemy("Spoggle_Ruminating_EN");
        public static readonly EnemySO _conductor = LoadedAssetsHandler.GetEnemy("Conductor_EN");
        public static readonly EnemySO _singingStone = LoadedAssetsHandler.GetEnemy("SingingStone_EN");
        public static readonly EnemySO _maniskin = LoadedAssetsHandler.GetEnemy("ManicMan_EN");
        public static readonly EnemySO _mungie = LoadedAssetsHandler.GetEnemy("Mungie_EN");
        public static readonly EnemySO _wrigglingSacrifice = LoadedAssetsHandler.GetEnemy("WrigglingSacrifice_EN");

        [Header("BaseGameMiniBosses")]
        public static readonly EnemySO _xiphactinus = LoadedAssetsHandler.GetEnemy("Xiphactinus_EN");
        public static readonly EnemySO _unfinishedHeir = LoadedAssetsHandler.GetEnemy("UnfinishedHeir_BOSS");

        [Header("BaseGameBosses")]
        public static readonly EnemySO _Charcarrion = LoadedAssetsHandler.GetEnemy("Charcarrion_ALIVE_BOSS");
        public static readonly EnemySO _Roids = LoadedAssetsHandler.GetEnemy("Roids_Boss");
        public static readonly EnemySO _OsmanSinnoks = LoadedAssetsHandler.GetEnemy("OsmanSinnoks_BOSS");
        public static readonly EnemySO _Mobius_BOSS = LoadedAssetsHandler.GetEnemy("Mobius_BOSS");

        [Header("BaseGameCharacters")]
        public static readonly CharacterSO _nowak = LoadedAssetsHandler.GetCharacter("Nowak_CH");
        public static readonly CharacterSO _anton = LoadedAssetsHandler.GetCharacter("Anton_CH");
        public static readonly CharacterSO _clive = LoadedAssetsHandler.GetCharacter("Clive_CH");
        public static readonly CharacterSO _boyle = LoadedAssetsHandler.GetCharacter("Boyle_CH");
        public static readonly CharacterSO _burnout = LoadedAssetsHandler.GetCharacter("Burnout_CH");
        public static readonly CharacterSO _longLiver = LoadedAssetsHandler.GetCharacter("LongLiver_CH");
        public static readonly CharacterSO _bimini = LoadedAssetsHandler.GetCharacter("Bimini_CH");
        public static readonly CharacterSO _kleiver = LoadedAssetsHandler.GetCharacter("Kleiver_CH");
        public static readonly CharacterSO _rags = LoadedAssetsHandler.GetCharacter("Rags_CH");
        public static readonly CharacterSO _leviat = LoadedAssetsHandler.GetCharacter("Leviat_CH");
        public static readonly CharacterSO _smokeStacks = LoadedAssetsHandler.GetCharacter("SmokeStacks_CH");
        public static readonly CharacterSO _agon = LoadedAssetsHandler.GetCharacter("Agon_CH");
        public static readonly CharacterSO _cranes = LoadedAssetsHandler.GetCharacter("Cranes_CH");
        public static readonly CharacterSO _griffin = LoadedAssetsHandler.GetCharacter("Griffin_CH");
        public static readonly CharacterSO _pearl = LoadedAssetsHandler.GetCharacter("Pearl_CH");
        public static readonly CharacterSO _splig = LoadedAssetsHandler.GetCharacter("Splig_CH");
        public static readonly CharacterSO _fennec = LoadedAssetsHandler.GetCharacter("Fennec_CH");
        public static readonly CharacterSO _hans = LoadedAssetsHandler.GetCharacter("Hans_CH");
        public static readonly CharacterSO _thype = LoadedAssetsHandler.GetCharacter("Thype_CH");
        public static readonly CharacterSO _arnold = LoadedAssetsHandler.GetCharacter("Arnold_CH");
        public static readonly CharacterSO _dimitri = LoadedAssetsHandler.GetCharacter("Dimitri_CH");
        public static readonly CharacterSO _gospel = LoadedAssetsHandler.GetCharacter("Gospel_CH");
        public static readonly CharacterSO _mordrake = LoadedAssetsHandler.GetCharacter("Mordrake_CH");
        public static readonly CharacterSO _mungCH = LoadedAssetsHandler.GetCharacter("Mung_CH");

        #endregion Refs
    }
}
