﻿using Harmony;
using FairONI;

namespace WallPumps
{

    public static class WallPumps
    {
        public static readonly Tag WallMachineMaterial = TagManager.Create("WallMachineMaterial");
    }

    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
    {
        public static void Prefix()
        {
            AddTags.AddStrings(WallPumps.WallMachineMaterial, "Wall Machine Material");

            GasWallPump.Setup();
            LiquidWallPump.Setup();
        }
    }

    [HarmonyPatch(typeof(ElementLoader))]
    [HarmonyPatch("FinaliseElementsTable")]
    public static class ElementLoader_FinaliseElementsTable_Patch
    {
        public static void Postfix()
        {
            // Add new material it to all refined metals + insulation
            foreach (Element e in ElementLoader.elementTable.Values)
            {
                if (e.HasTag(GameTags.RefinedMetal))
                {
                    ElementUtils.AddOreTag(e, WallPumps.WallMachineMaterial);
                }
            }
            ElementUtils.AddOreTag(ElementLoader.FindElementByHash(SimHashes.SuperInsulator), WallPumps.WallMachineMaterial);
        }
    }

    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    public static class Game_OnPrefabInit_Patch
    {
        public static void Postfix(Game __instance)
        {
            Debug.Log(" === WallPumps v. 1.6 OnPrefabInit === ");
        }
    }
}
