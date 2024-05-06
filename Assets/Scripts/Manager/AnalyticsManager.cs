using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H2910.Utils;
using DG.Tweening;
using H2910.Defines;
using UnityEngine.SceneManagement;
using H2910.Common.Singleton;

namespace H2910.Analytics
{
    public class AnalyticsManager : ManualSingletonMono<AnalyticsManager>
    {
        private Dictionary<AnalyticLevelParam, int> levelParam = new Dictionary<AnalyticLevelParam, int>();
        private long timeStartLevel;
        private int goldBeforLevel;
        private int gemBeforLevel;

        private void Start()
        {
            var array = Enum.GetValues(typeof(AnalyticLevelParam));
            foreach (var param in array)
            {
                if (!levelParam.ContainsKey((AnalyticLevelParam)param))
                    levelParam.Add((AnalyticLevelParam)param, 0);
            }
            timeStartLevel = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void BeginPlayLevel(MapInfo mapInfo)
        {
            timeStartLevel = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            PlayerData.Instance.PlayerMapInfo.CheckReplay(mapInfo);
            goldBeforLevel = PlayerData.Instance.PlayerProp.Gold;
            gemBeforLevel = PlayerData.Instance.PlayerProp.Gem;
        }

        public void LogEvent(string eventID, params Param[] lstParam)
        {
            try
            {
                Parameter[] lstParameters = new Parameter[lstParam.Length];
                string log = "";
                for (int i = 0; i < lstParam.Length; i++)
                {
                    var p = lstParam[i];
                    if (string.IsNullOrEmpty(p.ValueString))
                    {
                        lstParameters[i] = new Parameter(eventID + "_" + p.ParamName, p.ValueInt);
                        log += p.ParamName + "_" + p.ValueInt + " ;";
                    }
                    else
                    {
                        lstParameters[i] = new Parameter(eventID + "_" + p.ParamName, p.ValueString);
                        log += p.ParamName + "_" + p.ValueString + " ;";
                    }
                }
                //Debug.Log("LogEvent " + eventID + ": " + log);
#if !UNITY_EDITOR
                if (FirebaseManager.IsInstanceValid() && FirebaseManager.IsAvailable)
                    FirebaseAnalytics.LogEvent(eventID, lstParameters);
                else
                {
                    Debug.Log("LogEvent fail " + eventID + " / " + FirebaseManager.IsInstanceValid() + " / " + FirebaseManager.IsAvailable);
                }    
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"LogEvent Error: {e.Message} {e.StackTrace}");
            }
        }

        public void Count(AnalyticLevelParam param, int quantity = 1)
        {
            if (!levelParam.ContainsKey(param))
            {
                levelParam.Add(param, quantity);
            }
            else
                levelParam[param] += quantity;
        }
        public void Set(AnalyticLevelParam param, int quantity = 1)
        {
            if (!levelParam.ContainsKey(param))
            {
                levelParam.Add(param, quantity);
            }
            else
                levelParam[param] = quantity;
        }

        public void LogEvent(AnalyticEventID eventID, params Param[] lstParam)
        {
            LogEvent(eventID.ToString(), lstParam);
        }

        public void LogEventLevel(int levelControl, int victoryAction, int victoryAdsReward)
        {
            Vector2 playerPos = Player.Player.Instance.transform.position;
            levelParam[AnalyticLevelParam.level_highest] = PlayerData.Instance.PlayerMapInfo.GetHighestLevel();
            levelParam[AnalyticLevelParam.play_time] = (int)((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timeStartLevel) / 10);
            var currentMap = PlayerData.Instance.PlayerMapInfo.GetCurrentMap();
            levelParam[AnalyticLevelParam.level] = H2910Utils.GetMapId(currentMap);
            var mapCacheData = PlayerData.Instance.PlayerMapInfo.GetMapCacheData(currentMap);
            int flameCount = MainStats.Instance.ListStarIndex.Count;

            LogEvent(AnalyticEventID.level_up,
                    new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                    new Param(AnalyticLevelParam.value, levelControl),
                    new Param(AnalyticLevelParam.flame, flameCount),
                    new Param(AnalyticLevelParam.r2_objects, levelParam[AnalyticLevelParam.r2_objects]),
                    //new Param(AnalyticLevelParam.r3_objects, levelParam[AnalyticLevelParam.r3_objects]),
                    new Param(AnalyticLevelParam.level_highest, levelParam[AnalyticLevelParam.level_highest]),
                    new Param(AnalyticLevelParam.dies, levelParam[AnalyticLevelParam.dies]),
                    new Param(AnalyticLevelParam.play_time, levelParam[AnalyticLevelParam.play_time]),
                    new Param(AnalyticLevelParam.farm_replay, mapCacheData.TotalFarmReplay),
                    new Param(AnalyticLevelParam.comlete_replay, mapCacheData.TotalCompleteReplay),
                    new Param(AnalyticLevelParam.gold_before_level, goldBeforLevel / 500),
                    new Param(AnalyticLevelParam.golds, PlayerData.Instance.PlayerProp.AllCoin / 500),
                    new Param(AnalyticLevelParam.gem_before_level, gemBeforLevel / 10),
                    /*MultiCharacter  new Param(AnalyticParam.invisible_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(SkillUpgradeType.Skill1)),
                      new Param(AnalyticParam.slash_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(SkillUpgradeType.Skill2)),
                      new Param(AnalyticParam.critical_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(SkillUpgradeType.Skill3)),
                      new Param(AnalyticParam.evade_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(SkillUpgradeType.Skill4)),
                      new Param(AnalyticParam.hp_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(SkillUpgradeType.Skill5)),*/
                    new Param(AnalyticParam.hp_remain, (int)MainStats.Instance.CurrentHealth),
                    new Param(AnalyticLevelParam.kills, levelParam[AnalyticLevelParam.kills]),
                    new Param(AnalyticLevelParam.victory_action, victoryAction),
                    new Param(AnalyticLevelParam.victory_ads_reward, victoryAdsReward),
                    new Param(AnalyticParam.location_x, (int)(playerPos.x) / 5),
                    new Param(AnalyticParam.location_y, (int)(playerPos.y) / 5),
                    new Param(AnalyticLevelParam.miniboss_kill, levelParam[AnalyticLevelParam.miniboss_kill]),
                    new Param(AnalyticLevelParam.miniboss_meet, levelParam[AnalyticLevelParam.miniboss_meet]));

            LogEvent(AnalyticEventID.level_collect,
                    new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                    new Param(AnalyticLevelParam.level_highest, levelParam[AnalyticLevelParam.level_highest]),
                    new Param(AnalyticLevelParam.ore_bronze, levelParam[AnalyticLevelParam.ore_bronze]),
                    new Param(AnalyticLevelParam.ore_iron, levelParam[AnalyticLevelParam.ore_iron]),
                    new Param(AnalyticLevelParam.ore_mithril, levelParam[AnalyticLevelParam.ore_mithril]),
                    new Param(AnalyticLevelParam.ore_titan, levelParam[AnalyticLevelParam.ore_titan]),
                    new Param(AnalyticLevelParam.ore_silver, levelParam[AnalyticLevelParam.ore_silver]),
                    new Param(AnalyticLevelParam.ore_adamantium, levelParam[AnalyticLevelParam.ore_adamantium]),
                    new Param(AnalyticLevelParam.ore_orihalcon, levelParam[AnalyticLevelParam.ore_orihalcon]),
                    new Param(AnalyticLevelParam.herb_mushroom, levelParam[AnalyticLevelParam.herb_mushroom]),
                    new Param(AnalyticLevelParam.herb_fire, levelParam[AnalyticLevelParam.herb_fire]),
                    new Param(AnalyticLevelParam.herb_aloe, levelParam[AnalyticLevelParam.herb_aloe]),
                    new Param(AnalyticLevelParam.herb_ice, levelParam[AnalyticLevelParam.herb_ice]),
                    new Param(AnalyticLevelParam.herb_lightning, levelParam[AnalyticLevelParam.herb_lightning]),
                    new Param(AnalyticLevelParam.herb_rotten, levelParam[AnalyticLevelParam.herb_rotten]),
                    new Param(AnalyticLevelParam.herb_lily, levelParam[AnalyticLevelParam.herb_lily]),
                    new Param(AnalyticLevelParam.herb_sun, levelParam[AnalyticLevelParam.herb_sun]),
                    new Param(AnalyticLevelParam.herb_hyssop, levelParam[AnalyticLevelParam.herb_hyssop]),
                    new Param(AnalyticLevelParam.herb_herbal, levelParam[AnalyticLevelParam.herb_herbal]),
                    new Param(AnalyticLevelParam.herb_reed, levelParam[AnalyticLevelParam.herb_reed]),
                    new Param(AnalyticLevelParam.herb_resin, levelParam[AnalyticLevelParam.herb_resin]),
                    new Param(AnalyticLevelParam.herb_crystal, levelParam[AnalyticLevelParam.herb_crystal]));

            LogEvent(AnalyticEventID.level_use,
                    new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                    new Param(AnalyticLevelParam.level_highest, levelParam[AnalyticLevelParam.level_highest]),
                    new Param(AnalyticLevelParam.shuriken_bronze, levelParam[AnalyticLevelParam.shuriken_bronze]),
                    new Param(AnalyticLevelParam.shuriken_steel, levelParam[AnalyticLevelParam.shuriken_steel]),
                    new Param(AnalyticLevelParam.shuriken_mithril, levelParam[AnalyticLevelParam.shuriken_mithril]),
                    new Param(AnalyticLevelParam.shuriken_titan, levelParam[AnalyticLevelParam.shuriken_titan]),
                    new Param(AnalyticLevelParam.shuriken_sliver, levelParam[AnalyticLevelParam.shuriken_sliver]),
                    new Param(AnalyticLevelParam.shuriken_adamantium, levelParam[AnalyticLevelParam.shuriken_adamantium]),
                    new Param(AnalyticLevelParam.shuriken_orihalcon, levelParam[AnalyticLevelParam.shuriken_orihalcon]),
                    new Param(AnalyticLevelParam.grease_poison, levelParam[AnalyticLevelParam.grease_poison]),
                    new Param(AnalyticLevelParam.grease_fire, levelParam[AnalyticLevelParam.grease_fire]),
                    new Param(AnalyticLevelParam.grease_water, levelParam[AnalyticLevelParam.grease_water]),
                    new Param(AnalyticLevelParam.grease_ice, levelParam[AnalyticLevelParam.grease_ice]),
                    new Param(AnalyticLevelParam.grease_lightning, levelParam[AnalyticLevelParam.grease_lightning]),
                    new Param(AnalyticLevelParam.grease_bug, levelParam[AnalyticLevelParam.grease_bug]),
                    new Param(AnalyticLevelParam.grease_magic, levelParam[AnalyticLevelParam.grease_magic]),
                    new Param(AnalyticLevelParam.grease_holy, levelParam[AnalyticLevelParam.grease_holy]),
                    new Param(AnalyticLevelParam.grease_dark, levelParam[AnalyticLevelParam.grease_dark]),
                    new Param(AnalyticLevelParam.grease_kills, levelParam[AnalyticLevelParam.grease_kills]),
                    new Param(AnalyticLevelParam.slashes, levelParam[AnalyticLevelParam.slashes]),
                    new Param(AnalyticLevelParam.invisible, levelParam[AnalyticLevelParam.invisible]));

            if (victoryAction != 3)
            {
                LogEvent(AnalyticEventID.victory_ads,
                    new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                    new Param(AnalyticLevelParam.level_highest, levelParam[AnalyticLevelParam.level_highest]),
                    new Param(AnalyticLevelParam.victory_ads_reward, victoryAdsReward),
                    new Param(AnalyticLevelParam.dies, levelParam[AnalyticLevelParam.dies]),
                    new Param(AnalyticLevelParam.play_time, levelParam[AnalyticLevelParam.play_time]),
                    new Param(AnalyticLevelParam.farm_replay, mapCacheData.TotalFarmReplay),
                    new Param(AnalyticLevelParam.comlete_replay, mapCacheData.TotalCompleteReplay),
                    new Param(AnalyticLevelParam.gold_before_level, goldBeforLevel / 500),
                    new Param(AnalyticLevelParam.golds, PlayerData.Instance.PlayerProp.AllCoin / 500),
                    new Param(AnalyticLevelParam.gem_before_level, gemBeforLevel / 10),
                    new Param(AnalyticLevelParam.gem, PlayerData.Instance.PlayerProp.AllGem / 10),
                    /*MultiCharacternew Param(AnalyticParam.total_skill_level, PlayerData.Instance.PlayerProp.GetTotalSkill()),*/
                    new Param(AnalyticLevelParam.kills, levelParam[AnalyticLevelParam.kills]),
                    new Param(AnalyticLevelParam.miniboss_kill, levelParam[AnalyticLevelParam.miniboss_kill]),
                    new Param(AnalyticLevelParam.use_grease, levelParam[AnalyticLevelParam.use_grease]),
                    new Param(AnalyticLevelParam.use_shuriken, levelParam[AnalyticLevelParam.use_shuriken]),
                    new Param(AnalyticParam.ads_today, PlayerData.Instance.PlayerAdventureShop.TodayAds),
                    new Param(AnalyticParam.ads_total, PlayerData.Instance.PlayerAdventureShop.TotalAds));
            }

            var array = Enum.GetValues(typeof(AnalyticLevelParam));
            foreach (var param in array)
            {
                if (levelParam.ContainsKey((AnalyticLevelParam)param))
                    levelParam[(AnalyticLevelParam)param] = 0;
            }
        }

        public void LogEventCraft(AnalyticEventID eventId, string name, int quantity)
        {
            LogEvent(eventId, new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                    new Param(AnalyticParam.name, name),
                    new Param(AnalyticParam.quantity, quantity / 2));
        }

        public void LogEventPlayerDie(Vector2 posision, int timeDie)
        {
            var currentMap = PlayerData.Instance.PlayerMapInfo.GetCurrentMap();
            levelParam[AnalyticLevelParam.level] = H2910Utils.GetMapId(currentMap);
            Count(AnalyticLevelParam.dies);
            LogEvent(AnalyticEventID.level_die,
                    new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                    new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                    new Param(AnalyticParam.location_x, (int)posision.x / 5),
                    new Param(AnalyticParam.location_y, (int)posision.y / 5),
                    /*MultiCharacter new Param(AnalyticParam.invisible_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill1)),
                    new Param(AnalyticParam.slash_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill2)),
                    new Param(AnalyticParam.critical_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill3)),
                    new Param(AnalyticParam.evade_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill4)),
                    new Param(AnalyticParam.hp_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill5)),*/
                    new Param(AnalyticParam.die_time, timeDie));
        }

        public void SetUserProperty(string userType, string value)
        {
            if (FirebaseManager.IsInstanceValid() && FirebaseManager.IsAvailable)
                FirebaseAnalytics.SetUserProperty(userType, value);
            else
            {
                Debug.Log("LogEvent fail " + userType + " / " + FirebaseManager.IsInstanceValid() + " / " + FirebaseManager.IsAvailable);
            }
        }

        public void LogEventLogin()
        {
            var mapInfo = PlayerData.Instance.PlayerMapInfo.GetCurrentMap();
            if (SceneManager.GetActiveScene().name == "Splash" && !PlayerData.Instance.PlayerProp.IsUnlockFeature(UnlockFeature.CompleteMainTutorial))
            {
                SetUserProperty("current_level", 1.ToString());
                SetUserProperty("highest_level", 1.ToString());
            }
            else
            {
                SetUserProperty("current_level", PlayerData.Instance.PlayerMapInfo.GetCurrentMapId().ToString());
                SetUserProperty("highest_level", PlayerData.Instance.PlayerMapInfo.GetHighestLevel().ToString());
            }

            SetUserProperty("current_scene", H2910Utils.GetMapName(mapInfo));
            DateTime lastLogin = PlayerData.Instance.PlayerAdventureShop.GetLastLogin();
            if (lastLogin == DateTime.MinValue || lastLogin > DateTime.UtcNow)
                return;
            int current_hour = DateTime.UtcNow.Hour;
            int last_login_hour = lastLogin.Hour;
            TimeSpan delta = DateTime.UtcNow - lastLogin;
            if (current_hour < last_login_hour || delta.TotalDays >= 1)
                last_login_hour *= -1;
            LogEvent(AnalyticEventID.time_open_game_ev,
                    new Param(AnalyticParam.current_in_hour, current_hour),
                    new Param(AnalyticParam.last_in_hour, last_login_hour),
                    new Param(AnalyticParam.delta, (int)(delta.TotalMinutes / 5)));
        }

        public void LogEventMenuClick(string btnName)
        {
            var currentMap = PlayerData.Instance.PlayerMapInfo.GetCurrentMap();
            levelParam[AnalyticLevelParam.level] = H2910Utils.GetMapId(currentMap);
            LogEvent(AnalyticEventID.menu_btn_ev,
                    new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                    new Param(AnalyticParam.btn_name, btnName),
                    new Param(AnalyticParam.total_gems, PlayerData.Instance.PlayerProp.Gem / 10),
                    new Param(AnalyticParam.total_golds, PlayerData.Instance.PlayerProp.Gold / 500));
        }

        public void LogEventBuyItem(AnalyticEventID eventId, string name, int action)
        {
            LogEvent(eventId,
                    new Param(AnalyticParam.name, name),
                    new Param(AnalyticParam.action, action),
                    new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()));
        }

        public void LogEventCheckPoint(Vector2 position, int type, int quantityUsed, int remainInInventory)
        {
            var currentMap = PlayerData.Instance.PlayerMapInfo.GetCurrentMap();
            levelParam[AnalyticLevelParam.level] = H2910Utils.GetMapId(currentMap);
            LogEvent(AnalyticEventID.checkpoint,
                new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                new Param(AnalyticParam.location_x, (int)position.x / 5),
                new Param(AnalyticParam.location_y, (int)position.y / 5),
                new Param(AnalyticParam.type, type),
                new Param(AnalyticParam.number, quantityUsed),
                new Param(AnalyticParam.remain, remainInInventory)
                );
        }

        public void LogEventWatchAdsLuckWheel(int currentTime, int allTime)
        {
            LogEvent(AnalyticEventID.lucky_wheel_ev,
                    new Param(AnalyticParam.click_count, currentTime),
                    new Param(AnalyticParam.click_count_all_time, allTime),
                    new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()));
        }
        public void LogEventLuckyWheelStats(string itemName, int quantity)
        {
            LogEvent(AnalyticEventID.lucky_wheel_stats,
                  new Param(AnalyticParam.name, itemName),
                  new Param(AnalyticParam.quantity, quantity),
                  new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel())
                );
        }
        public void LogEventUseGold(string use_taget, int goldUse)
        {
            LogEvent(AnalyticEventID.gold_use,
                new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                new Param(AnalyticParam.total_gold, AchievementManager.Instance.Total_gold()),
                new Param(AnalyticParam.current_gold, (int)PlayerData.Instance.PlayerProp.Gold / 500),
                new Param(AnalyticParam.total_gold_used, AchievementManager.Instance.Used_gold()),
                new Param(AnalyticParam.use_taget, use_taget),
                new Param(AnalyticParam.gold_use, (int)goldUse / 500)
                );
        }
        public void LogEventUseGem(string use_taget, int gemUse)
        {
            LogEvent(AnalyticEventID.gem_use,
                new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                new Param(AnalyticParam.total_gold, AchievementManager.Instance.Total_gem()),
                new Param(AnalyticParam.current_gold, (int)PlayerData.Instance.PlayerProp.Gem / 50),
                new Param(AnalyticParam.total_gold_used, AchievementManager.Instance.Used_gem()),
                new Param(AnalyticParam.use_taget, use_taget),
                new Param(AnalyticParam.gold_use, gemUse)
                );
        }
        public void LogEventGardenStats(string plantName, int quantity, int typeSeed, int condition, int watchAds, int slotHasUnlocked)
        {
            LogEvent(AnalyticEventID.garden_stats,
                   new Param(AnalyticParam.name, plantName),
                   new Param(AnalyticParam.quantity, quantity),
                   new Param(AnalyticParam.seed, typeSeed),
                   new Param(AnalyticParam.condition, condition),
                   new Param(AnalyticParam.ads, watchAds),
                   new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                   new Param(AnalyticParam.slot, slotHasUnlocked)
                 );
        }
        public void LogEventUpgradeSkill(string name, int targetLevel, int type, int adsRemain, int adsUse)
        {
            LogEvent(AnalyticEventID.skill_up,
                    new Param(AnalyticParam.name_level_up, name),
                    new Param(AnalyticParam.target_level, targetLevel),
                    /*MultiCharacter new Param(AnalyticParam.invisible_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill1)),
                     new Param(AnalyticParam.slash_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill2)),
                     new Param(AnalyticParam.critical_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill3)),
                     new Param(AnalyticParam.evade_skill_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill4)),
                     new Param(AnalyticParam.hp_level, PlayerData.Instance.PlayerProp.GetCurrentSkill(Defines.SkillUpgradeType.Skill5)),*/
                    new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                    new Param(AnalyticParam.type, type),
                    new Param(AnalyticParam.ads_remain, adsRemain),
                    new Param(AnalyticParam.ad_use, adsUse)
                    );
        }

        public void LogEventBuyItem(string name)
        {
            LogEvent(AnalyticEventID.adventure_change, new Param(AnalyticParam.name, name));
        }
        public void LogEventQuestStats(string questId, string duration, string idQuestLongestTimesFinish, string durationLongest, int countMainQuestDone, int countSideQuestDone, int watchAds, int countInProgressQuestNotDone
            , string idQuestInProgressLongestNotDone, string timeQuestInProgressLongestNotDone, string idQuestInProgressNearest, int countQuestActiveButNotAccept, string idQuestDoneNearest)
        {
            LogEvent(AnalyticEventID.quest_stats,
                new Param(AnalyticParam.id, questId),
                new Param(AnalyticParam.duration, duration),
                new Param(AnalyticParam.id_longest, idQuestLongestTimesFinish),
                new Param(AnalyticParam.duration_longest, durationLongest),
                new Param(AnalyticParam.complete_main, countMainQuestDone),
                new Param(AnalyticParam.complete_side, countSideQuestDone),
                new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                new Param(AnalyticParam.ads, watchAds),
                new Param(AnalyticParam.inprogress, countInProgressQuestNotDone),
                new Param(AnalyticParam.id_inpro_longest, idQuestInProgressLongestNotDone),
                new Param(AnalyticParam.dura_inpro_longest, timeQuestInProgressLongestNotDone),
                new Param(AnalyticParam.id_inpro_nearest, idQuestInProgressNearest),
                new Param(AnalyticParam.avaiable, countQuestActiveButNotAccept),
                new Param(AnalyticParam.id_comp_nearest, idQuestDoneNearest)
                );
        }
        public void LogEventLevelQuest(int countQuestActiveButNotAccept, int countQuestComplete, int countQuestInProgress, int finishM3, int finishM4, int finishM5, int finishM6, int finishM7, int finishM8,
            int finishM9, int finishM10, int finishM11, int finishM12, int finishM13, int acceptM5ButNotDoneYet)
        {
            var currentMap = PlayerData.Instance.PlayerMapInfo.GetCurrentMap();
            levelParam[AnalyticLevelParam.level] = H2910Utils.GetMapId(currentMap);
            LogEvent(AnalyticEventID.level_quest,
                new Param(AnalyticLevelParam.level, levelParam[AnalyticLevelParam.level]),
                new Param(AnalyticLevelParam.level_highest, PlayerData.Instance.PlayerMapInfo.GetHighestLevel()),
                new Param(AnalyticParam.avaiable, countQuestActiveButNotAccept),
                new Param(AnalyticParam.complete, countQuestComplete),
                new Param(AnalyticParam.inprogress, countQuestInProgress),
                new Param(AnalyticParam.finish_m3, finishM3),
                new Param(AnalyticParam.finish_m4, finishM4),
                new Param(AnalyticParam.finish_m5, finishM5),
                new Param(AnalyticParam.finish_m6, finishM6),
                new Param(AnalyticParam.finish_m7, finishM7),
                new Param(AnalyticParam.finish_m8, finishM8),
                new Param(AnalyticParam.finish_m9, finishM9),
                new Param(AnalyticParam.finish_m10, finishM10),
                new Param(AnalyticParam.finish_m11, finishM11),
                new Param(AnalyticParam.finish_m12, finishM12),
                new Param(AnalyticParam.finish_m13, finishM13),
                new Param(AnalyticParam.accept_m5, acceptM5ButNotDoneYet)
                );
        }
        public void LogEventBeginStory(int[] dura_pics, int numberOfSkips, int totalTime)
        {
            LogEvent(AnalyticEventID.begin_story,
                new Param(AnalyticParam.dura_pic1, dura_pics[0]),
                new Param(AnalyticParam.dura_pic2, dura_pics[1]),
                new Param(AnalyticParam.dura_pic3, dura_pics[2]),
                new Param(AnalyticParam.dura_pic4, dura_pics[3]),
                new Param(AnalyticParam.dura_pic5, dura_pics[4]),
                new Param(AnalyticParam.dura_pic6, dura_pics[5]),
                new Param(AnalyticParam.dura_pic7, dura_pics[6]),
                new Param(AnalyticParam.dura_pic8, dura_pics[7]),
                new Param(AnalyticParam.dura_pic9, dura_pics[8]),
                new Param(AnalyticParam.dura_pic10, dura_pics[9]),
                new Param(AnalyticParam.skips, numberOfSkips),
                new Param(AnalyticParam.total_time, totalTime),
                new Param(AnalyticParam.device_language, Application.systemLanguage.ToString())
            );
        }
        public class Param
        {
            public string ParamName;
            public int ValueInt;
            public string ValueString;

            public Param(AnalyticParam paramName, int valueInt)
            {
                this.ParamName = paramName.ToString();
                this.ValueInt = valueInt;
            }

            public Param(AnalyticParam paramName, string valueString)
            {
                this.ParamName = paramName.ToString();
                this.ValueString = valueString;
            }

            public Param(AnalyticLevelParam paramName, int valueInt)
            {
                this.ParamName = paramName.ToString();
                this.ValueInt = valueInt;
            }

            public Param(AnalyticLevelParam paramName, string valueString)
            {
                this.ParamName = paramName.ToString();
                this.ValueString = valueString;
            }

            public Param(string paramName, int valueInt)
            {
                this.ParamName = paramName;
                this.ValueInt = valueInt;
            }

            public Param(string paramName, string valueString)
            {
                this.ParamName = paramName;
                this.ValueString = valueString;
            }
        }
    }
    public enum AnalyticEventID
    {
        level_up,
        level_collect,
        level_use,
        craft_grease,
        craft_shuriken,
        checkpoint,
        level_die,
        time_open_game_ev,
        menu_btn_ev,
        buy_pack_ev,
        buy_pack_by_gem_ev,
        skill_up,
        lucky_wheel_ev,
        victory_ads,
        adventure_change,
        lucky_wheel_stats,
        garden_stats,
        quest_stats,
        level_quest,
        begin_story,
        gold_use,
        gem_use,
        trial_remove_ads
    }
    public enum AnalyticLevelParam
    {
        value,
        level,
        flame,
        r2_objects,
        //r3_objects,
        level_highest,
        dies,
        play_time,
        farm_replay,
        comlete_replay,
        gold_before_level,
        golds,
        gem_before_level,
        gem,
        kills,
        victory_action,
        victory_ads_reward,
        miniboss_kill,
        miniboss_meet,
        ore_bronze,
        ore_iron,
        ore_mithril,
        ore_titan,
        ore_silver,
        ore_adamantium,
        ore_orihalcon,
        herb_mushroom,
        herb_fire,
        herb_aloe,
        herb_ice,
        herb_lightning,
        herb_rotten,
        herb_lily,
        herb_sun,
        herb_hyssop,
        herb_herbal,
        herb_reed,
        herb_resin,
        herb_crystal,
        shuriken_bronze,
        shuriken_steel,
        shuriken_mithril,
        shuriken_titan,
        shuriken_sliver,
        shuriken_adamantium,
        shuriken_orihalcon,
        grease_poison,
        grease_fire,
        grease_water,
        grease_ice,
        grease_lightning,
        grease_bug,
        grease_magic,
        grease_holy,
        grease_dark,
        grease_kills,
        slashes,
        invisible,
        use_grease,
        use_shuriken,

    }

    public enum AnalyticParam
    {
        quantity,
        location_x,
        location_y,
        current_in_hour,
        last_in_hour,
        delta,
        btn_name,
        total_golds,
        total_gems,
        name,
        action,
        name_level_up,
        target_level,
        type,
        ads_remain,
        click_count,
        click_count_all_time,
        invisible_skill_level,
        slash_skill_level,
        critical_skill_level,
        evade_skill_level,
        hp_level,
        hp_remain,
        ad_use,
        ads_total,
        ads_today,
        total_skill_level,
        die_time,
        number,
        remain,
        seed,
        condition,
        ads,
        slot,
        id,
        duration,
        duration_longest,
        complete_main,
        complete_side,
        inprogress,
        id_inpro_longest,
        dura_inpro_longest,
        id_inpro_nearest,
        avaiable,
        id_comp_nearest,
        complete,
        finish_m3,
        finish_m4,
        finish_m5,
        finish_m6,
        finish_m7,
        finish_m8,
        finish_m9,
        finish_m10,
        finish_m11,
        finish_m12,
        finish_m13,
        dura_pic1,
        dura_pic2,
        dura_pic3,
        dura_pic4,
        dura_pic5,
        dura_pic6,
        dura_pic7,
        dura_pic8,
        dura_pic9,
        dura_pic10,
        total_time,
        skips,
        device_language,
        accept_m5,
        total_gold,
        current_gold,
        total_gold_used,
        use_taget,
        gold_use,
        total_gem,
        current_gem,
        total_gem_used,
        gem_use,
        id_longest,
    }
}