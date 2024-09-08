using AbilityExample.abilities;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Wizards.People;
using Wizards.Perks;

namespace AbilityExample;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        harmony.PatchAll();
    }

    
    public static bool applied = false;
    // This seems to be a safe place to insert  
    [HarmonyPatch(typeof(PerkScene), nameof(PerkScene.Initialize))]
    class Patch
    {
        static void Prefix(CharacterNames _startCharacter)
        {
            if (_startCharacter == CharacterNames.WitchCop && !applied)
            {
                ClassData chara = Managers.Characters.GetClassData(_startCharacter);
                AbilitySO ability1 = ScriptableObject.CreateInstance<FPush>();
                ability1.damage = 5;
                ability1.knockback = 5;
                ability1.range = 10;
                ability1.displayName = "Force Push";
                ability1.isMainAbility = true;
                ability1.description = "This is my test ability";
                chara.abilityList.Add(ability1);
                
                AbilitySO ability2 = ScriptableObject.CreateInstance<Roundhouse>();
                ability2.damage = 2;
                ability2.knockback = 2;
                ability2.range = 1;
                ability2.displayName = "Roundhouse";
                ability2.isMainAbility = true;
                ability2.description = "This is my second test ability";
                chara.abilityList.Add(ability2);
                
                Managers.Characters.characterMap[_startCharacter][0] = chara;
                applied = true;
            }
        }
    }
    
}