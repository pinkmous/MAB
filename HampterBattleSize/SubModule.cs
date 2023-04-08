using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace HampterBattleSize
{
    // Token: 0x02000006 RID: 6
    public class SubModule : MBSubModuleBase
    {
        // Token: 0x0600000C RID: 12 RVA: 0x00002278 File Offset: 0x00000478
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Harmony harmony = new Harmony("com.HampterBattleSize");
            harmony.PatchAll();

            Type typeFromHandle = typeof(MissionAgentSpawnLogic);
            Type nestedType = typeFromHandle.GetNestedType("MissionSide", AccessTools.all);
            MethodInfo method = nestedType.GetMethod("SpawnTroops", AccessTools.all);
            MethodInfo method2 = typeof(PatchMissionAgentSpawnLogic).GetMethod("SpawnTroopsPrefix");
            harmony.Patch(method, new HarmonyMethod(method2), null, null, null);

            int[] value = new int[]
                {
                    1000,
                    1200,
                    1400,
                    1600,
                    1800,
                    2000,
                    2040
                };
                typeof(BannerlordConfig).GetField("_battleSizes", AccessTools.all).SetValue(null, value);
                typeof(BannerlordConfig).GetField("_siegeBattleSizes", AccessTools.all).SetValue(null, value);
                HampterBattleSizeConfig.ReadConfig();
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002351 File Offset: 0x00000551
        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            base.OnBeforeMissionBehaviorInitialize(mission);
            ExtensionMissionAgentSpawnLogic._hampterBattleSize = BannerlordConfig.GetRealBattleSize();
            ExtensionMissionAgentSpawnLogic._initialSpawnOver = false;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x0000236C File Offset: 0x0000056C
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            InformationManager.DisplayMessage(new InformationMessage(string.Format("HampterBattleSize-> ReinforcementInterval:{0} , RetreatProportion:{1}", HampterBattleSizeConfig.ReinforcementInterval, HampterBattleSizeConfig.RetreatProportion)));
        }
    }
}
