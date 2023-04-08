using System;
using HarmonyLib;
using SandBox.Missions.MissionLogics;
using TaleWorlds.MountAndBlade;

namespace HampterBattleSize
{
    // Token: 0x02000005 RID: 5
    [HarmonyPatch(typeof(SandBoxMissionSpawnHandler))]
    public class PatchSandBoxMissionSpawnHandler
    {
        // Token: 0x0600000A RID: 10 RVA: 0x00002248 File Offset: 0x00000448
        [HarmonyPrefix]
        [HarmonyPatch("CreateSandBoxBattleWaveSpawnSettings")]
        public static bool CreateSandBoxBattleWaveSpawnSettingsPrefix(SandBoxMissionSpawnHandler __instance, ref MissionSpawnSettings __result)
        {
            int reinforcementWaveCount = BannerlordConfig.GetReinforcementWaveCount();
            __result = HampterBattleSizeConfig.GetMissionSpawnSettings(reinforcementWaveCount);
            return false;
        }
    }
}
