using System;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace RealBattleSize
{
    // Token: 0x02000003 RID: 3
    [HarmonyPatch(typeof(Agent))]
    public class PatchAgent
    {
        // Token: 0x06000005 RID: 5 RVA: 0x000021C5 File Offset: 0x000003C5
        [HarmonyPostfix]
        [HarmonyPatch("Die")]
        private static void Postfix(Agent __instance, Blow b, Agent.KillInfo overrideKillInfo = Agent.KillInfo.Invalid)
        {
            PatchAgent.RetreatMount(__instance);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021D0 File Offset: 0x000003D0
        private static void RetreatMount(Agent __instance)
        {
            bool hasMount = __instance.HasMount;
            if (hasMount)
            {
                bool flag = MBRandom.RandomFloat < 0.8f;
                if (flag)
                {
                    __instance.MountAgent.Retreat(__instance.Mission.GetClosestFleePositionForAgent(__instance));
                }
            }
        }
    }
}
