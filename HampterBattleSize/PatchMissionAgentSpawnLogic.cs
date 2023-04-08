using System;
using System.Linq;
using System.Security.AccessControl;
using HarmonyLib;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.CustomBattle;

namespace HampterBattleSize
{
    // Token: 0x02000008 RID: 8
    [HarmonyPatch(typeof(MissionAgentSpawnLogic))]
    public class PatchMissionAgentSpawnLogic
    {
        // Token: 0x06000013 RID: 19 RVA: 0x000023CC File Offset: 0x000005CC
        [HarmonyPrefix]
        [HarmonyPatch("MaxNumberOfTroopsForMission", (MethodType)1)]
        private static bool MaxNumberOfTroopsForMissionPrefix(MissionAgentSpawnLogic __instance, ref int __result)
        {
            InformationManager.DisplayMessage(new InformationMessage("MaxNumberOfTroopsForMission"));

            __result = 1000;
            return false;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000023E8 File Offset: 0x000005E8
        public static bool SpawnTroopsPrefix(MissionAgentSpawnLogic __instance, ref int __result)
        {
            InformationManager.DisplayMessage(new InformationMessage("SpawnTroopsPrefix"));

            bool flag = __instance.InitialSpawnOver();
            if (flag)
            {
                bool flag2 = __instance.NumberOfAllAgents() >= __instance.HampterBattleSize();
                if (flag2)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Too many Agents"));
                    __result = 0;
                    return false;
                }
            }
            return true;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002434 File Offset: 0x00000634
        [HarmonyPrefix]
        [HarmonyPatch("BattleSize", (MethodType)1)]
        private static bool BattleSizePrefix(MissionAgentSpawnLogic __instance, ref int __result)
        {
            InformationManager.DisplayMessage(new InformationMessage("BattleSize"));

            bool flag = !__instance.InitialSpawnOver();
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                int num = __instance.NumberOfAllAgents() - (__instance.NumberOfActiveDefenderTroops + __instance.NumberOfActiveAttackerTroops);
                __result = __instance.HampterBattleSize() - num;
                result = false;
            }
            return result;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002478 File Offset: 0x00000678
        [HarmonyPostfix]
        [HarmonyPatch("IsInitialSpawnOver", (MethodType)1)]
        private static void IsInitialSpawnOverPostfix(MissionAgentSpawnLogic __instance, bool __result)
        {
            InformationManager.DisplayMessage(new InformationMessage("IsInitialSpawnOver"));

            bool flag = __result && !__instance.InitialSpawnOver();
            if (flag)
            {
                ExtensionMissionAgentSpawnLogic._initialSpawnOver = true;
                bool flag2 = !(Game.Current.GameType is CustomGame);
                if (flag2)
                {
                    MapEvent mapEvent = MobileParty.MainParty.MapEvent;
                    bool flag3 = mapEvent.IsHideoutBattle || (mapEvent.IsSiegeAssault && PlayerSiege.BesiegedSettlement != null && PlayerSiege.BesiegedSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall);
                    if (flag3)
                    {
                        return;
                    }
                }
                PatchMissionAgentSpawnLogic.SetProportion(__instance);
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002500 File Offset: 0x00000700
        [HarmonyPrefix]
        [HarmonyPatch("TotalSpawnNumber", (MethodType)1)]
        public static bool TotalSpawnNumberPrefix(MissionAgentSpawnLogic __instance, ref int __result)
        {
            InformationManager.DisplayMessage(new InformationMessage("TotalSpawnNumberPrefix"));

            __result = Math.Max((__instance.HampterBattleSize() - __instance.NumberOfAgents) / 2 - 2, 0);
            return false;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x0000252C File Offset: 0x0000072C
        [HarmonyPrefix]
        [HarmonyPatch("CheckReinforcementBatch")]
        public static bool CheckReinforcementBatchPretfix(MissionAgentSpawnLogic __instance)
        {
            PatchMissionAgentSpawnLogic.TickRetreat(__instance);
            return true;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002548 File Offset: 0x00000748
        public unsafe static void SetProportion(MissionAgentSpawnLogic __instance)
        {
            InformationManager.DisplayMessage(new InformationMessage("Set proprotion"));

            float num = (float)__instance.GetAllTroopsForSide(BattleSideEnum.Attacker).ToArray<IAgentOriginBase>().Length;
            float num2 = (float)__instance.GetAllTroopsForSide(BattleSideEnum.Defender).ToArray<IAgentOriginBase>().Length;
            float num3 = num + num2;

            fixed (MissionSpawnSettings* reinforcementSpawnSettings = &__instance.ReinforcementSpawnSettings)
            {
                MissionSpawnSettings* ptr = reinforcementSpawnSettings;
                PatchMissionAgentSpawnLogic.SpawnSettings.AttackerReinforcementBatchPercentage = num / num3;
                PatchMissionAgentSpawnLogic.SpawnSettings.DefenderReinforcementBatchPercentage = num2 / num3;
                PatchMissionAgentSpawnLogic.SpawnSettings.GlobalReinforcementInterval = (float)HampterBattleSizeConfig.ReinforcementInterval;
                *ptr = PatchMissionAgentSpawnLogic.SpawnSettings;
            }
            PatchMissionAgentSpawnLogic.CanRetreat = false;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000025CC File Offset: 0x000007CC
        public unsafe static void TickRetreat(MissionAgentSpawnLogic __instance)
        {
            bool canRetreat = PatchMissionAgentSpawnLogic.CanRetreat;
            if (!canRetreat)
            {
                float num = (float)__instance.NumberOfActiveDefenderTroops / (float)__instance.NumberOfActiveAttackerTroops;
                bool flag = num > (float)HampterBattleSizeConfig.RetreatProportion;
                if (flag)
                {
                    fixed (MissionSpawnSettings* reinforcementSpawnSettings = &__instance.ReinforcementSpawnSettings)
                    {
                        MissionSpawnSettings* ptr = reinforcementSpawnSettings;
                        PatchMissionAgentSpawnLogic.SpawnSettings.AttackerReinforcementBatchPercentage = 0f;
                        *ptr = PatchMissionAgentSpawnLogic.SpawnSettings;
                    }
                    PatchMissionAgentSpawnLogic.CanRetreat = true;
                }
                else
                {
                    bool flag2 = num < 1f / (float)HampterBattleSizeConfig.RetreatProportion;
                    if (flag2)
                    {
                        fixed (MissionSpawnSettings* reinforcementSpawnSettings2 = &__instance.ReinforcementSpawnSettings)
                        {
                            MissionSpawnSettings* ptr2 = reinforcementSpawnSettings2;
                            PatchMissionAgentSpawnLogic.SpawnSettings.DefenderReinforcementBatchPercentage = 0f;
                            *ptr2 = PatchMissionAgentSpawnLogic.SpawnSettings;
                        }
                        PatchMissionAgentSpawnLogic.CanRetreat = true;
                    }
                }
            }
        }

        // Token: 0x04000006 RID: 6
        // public static MissionSpawnSettings SpawnSettings = new MissionSpawnSettings(30f, 0f, 0, 0, 2, 0f, 0f, 0.5f, 0, 0f, 0f, 1f, 0.75f);
        public static MissionSpawnSettings SpawnSettings = new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating,
                MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer,
                MissionSpawnSettings.ReinforcementSpawnMethod.Wave,
                30f,
                0f,
                0f,
                0.5f,
                0,
                0f,
                0f,
                1f,
                0.75f);

        // Token: 0x04000007 RID: 7
        public static bool CanRetreat;
    }
}
