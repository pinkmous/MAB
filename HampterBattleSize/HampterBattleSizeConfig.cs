using System;
using System.IO;
using System.Xml;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.MountAndBlade.MissionSpawnSettings;

namespace HampterBattleSize
{
    // Token: 0x02000002 RID: 2
    public class HampterBattleSizeConfig
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        /* 1.1.0 public MissionSpawnSettings(
         * InitialSpawnMethod initialTroopsSpawnMethod, 0
         * ReinforcementTimingMethod reinforcementTimingMethod, // 0?
         * ReinforcementSpawnMethod reinforcementTroopsSpawnMethod, 2
         * float globalReinforcementInterval = 0f, (float)HampterBattleSizeConfig.ReinforcementInterval
         * float reinforcementBatchPercentage = 0f, 0f
         * float desiredReinforcementPercentage = 0f, 0f
         * float reinforcementWavePercentage = 0f, 0.5f
         * int maximumReinforcementWaveCount = 0, reinforcementWaveCount
         * float defenderReinforcementBatchPercentage = 0f, 0f
         * float attackerReinforcementBatchPercentage = 0f, 0f
         * float defenderAdvantageFactor = 1f, 1f
         * float maximumBattleSizeRatio = 0.75f, 0.75f )
         */

        /* 1.0.3 public MissionSpawnSettings(
            float reinforcementInterval, (float)HampterBattleSizeConfig.ReinforcementInterval
            float reinforcementIntervalChange, 0f
            int reinforcementIntervalCount, 0
            InitialSpawnMethod initialTroopsSpawnMethod, 0
            ReinforcementSpawnMethod reinforcementTroopsSpawnMethod, 2
            float reinforcementBatchPercentage=0f, 0f
            float desiredReinforcementPercentage=0f, 0f
            float reinforcementWavePercentage=0f, 0.5f
            int maximumReinforcementWaveCount=0, reinforcementWaveCount
            float defenderReinforcementBatchPercentage=0f, 0f
            float attackerReinforcementBatchPercentage=0f, 0f
            float defenderAdvantageFactor=DefaultDefenderAdvantageFactor, 1f
            float maximumBattleSizeRatio=DefaultMaximumBattleSizeRatio 0.75f
            new MissionSpawnSettings((float)HampterBattleSizeConfig.ReinforcementInterval, 0f, 0, 0, 2, 0f, 0f, 0.5f, reinforcementWaveCount, 0f, 0f, 1f, 0.75f);
         */
        public static MissionSpawnSettings GetMissionSpawnSettings(int reinforcementWaveCount = 0)
        {
            return new MissionSpawnSettings(MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating, 
                MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer, 
                MissionSpawnSettings.ReinforcementSpawnMethod.Wave, 
                (float)HampterBattleSizeConfig.ReinforcementInterval, 
                0f, 
                0f, 
                0.5f, 
                reinforcementWaveCount, 
                0f, 
                0f, 
                1f, 
                0.75f);
        }

        // Token: 0x06000002 RID: 2 RVA: 0x0000209C File Offset: 0x0000029C
        public static void ReadConfig()
        {
            XmlDocument xmlDocument = new XmlDocument();
            StreamReader streamReader = new StreamReader(ModuleHelper.GetModuleFullPath("HampterBattleSize") + "ModuleData/HBS_Config.xml");
            string xml = streamReader.ReadToEnd();
            xmlDocument.LoadXml(xml);
            streamReader.Close();
            foreach (object obj in xmlDocument.GetElementsByTagName("Config")[0])
            {
                XmlNode xmlNode = (XmlNode)obj;
                bool flag = xmlNode.Name == "ReinforcementInterval";
                if (flag)
                {
                    HampterBattleSizeConfig.ReinforcementInterval = int.Parse(xmlNode.Attributes["value"].Value);
                }
                else
                {
                    bool flag2 = xmlNode.Name == "RetreatProportion";
                    if (flag2)
                    {
                        HampterBattleSizeConfig.RetreatProportion = int.Parse(xmlNode.Attributes["value"].Value);
                    }
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public static int ReinforcementInterval = 30;

        // Token: 0x04000002 RID: 2
        public static int RetreatProportion = 20;
    }
}
