using System;
using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.MissionSpawnHandlers;

namespace HampterBattleSize
{
    // Token: 0x02000004 RID: 4
    [HarmonyPatch(typeof(CustomMissionSpawnHandler))]
    public class PatchCustomMissionSpawnHandler
    {
        // Token: 0x06000008 RID: 8 RVA: 0x00002220 File Offset: 0x00000420
        [HarmonyPrefix]
        [HarmonyPatch("CreateCustomBattleWaveSpawnSettings")]
        public static bool CreateCustomBattleWaveSpawnSettingsPrefix(CustomMissionSpawnHandler __instance, ref MissionSpawnSettings __result)
        {
            __result = HampterBattleSizeConfig.GetMissionSpawnSettings(0);
            return false;
        }
    }
}
