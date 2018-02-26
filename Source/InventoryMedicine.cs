﻿using System.Reflection;
using Verse;
using UnityEngine;
using Harmony;

namespace InventoryMedicine
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            // initialize settings
            GetSettings<Settings>();
#if DEBUG
            HarmonyInstance.DEBUG = true;
#endif
            HarmonyInstance harmony = HarmonyInstance.Create("uuugggg.rimworld.inventorymedicine.main");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            harmony.Patch(AccessTools.Property(typeof(MapPawns), nameof(MapPawns.AllPawnsSpawned)).GetGetMethod(false),
                 null, new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.AllPawnsSpawnedPostfix)));
            harmony.Patch(AccessTools.Property(typeof(MapPawns), nameof(MapPawns.PrisonersOfColonySpawned)).GetGetMethod(false),
                 null, new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.PrisonersOfColonySpawnedPostfix)));
            harmony.Patch(AccessTools.Method(typeof(MapPawns), "SpawnedPawnsInFaction"),
                 null, new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.SpawnedPawnsInFactionPostfix)));


        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            GetSettings<Settings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Smart Medicine";
        }
    }
}