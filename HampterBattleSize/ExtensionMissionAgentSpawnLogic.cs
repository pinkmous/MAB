using System;
using TaleWorlds.MountAndBlade;

namespace HampterBattleSize
{
    // Token: 0x02000007 RID: 7
    public static class ExtensionMissionAgentSpawnLogic
    {
        // Token: 0x06000010 RID: 16 RVA: 0x000023AA File Offset: 0x000005AA
        public static int HampterBattleSize(this MissionAgentSpawnLogic logic)
        {
            return ExtensionMissionAgentSpawnLogic._hampterBattleSize;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000023B1 File Offset: 0x000005B1
        public static bool InitialSpawnOver(this MissionAgentSpawnLogic logic)
        {
            return ExtensionMissionAgentSpawnLogic._initialSpawnOver;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000023B8 File Offset: 0x000005B8
        public static int NumberOfAllAgents(this MissionAgentSpawnLogic logic)
        {
            return Mission.Current.AllAgents.Count;
        }

        // Token: 0x04000003 RID: 3
        public static int _hampterBattleSize;

        // Token: 0x04000004 RID: 4
        public static bool _initialSpawnOver;

        // Token: 0x04000005 RID: 5
        public static int _numberOfAllAgents;
    }
}