using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Daily Tasks", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 8)]
    [Description("A Daily Task System for Hurtworld.")]

    class DailyTasks : CovalencePlugin
    {
        #region Structs

        public struct TaskInformation
        {
            public string Task;
            public string EntityName;
            public int AmountToComplete;

            public TaskInformation(string NewTask, string NewEntityName, int NewAmountToComplete)
            {
                Task = NewTask;
                EntityName = NewEntityName;
                AmountToComplete = NewAmountToComplete;
            }
        };

        #endregion

        #region Languages

        void Loaded()
        {
            LoadMessages();
        }

        void LoadMessages()
        {
            // Polish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["newtask"] = "Nowe zadanie dostępne!",
                ["reward"] = "Otrzymałeś nagrode za ukońcenie zadania!",
                ["1"] = "Zadaj {CompletedAmount}/{TotalAmount} obrażeń stworą!",
                ["2"] = "Zadaj {CompletedAmount}/{TotalAmount} obrażeń graczom!",
                ["3"] = "Zabij {CompletedAmount}/{TotalAmount} graczy!",
                ["4"] = "Zabij {CompletedAmount}/{TotalAmount} Bor!",
                ["5"] = "Zabij {CompletedAmount}/{TotalAmount} Napromieńiowanego Bor!",
                ["6"] = "Zabij {CompletedAmount}/{TotalAmount} Shigi!",
                ["7"] = "Zabij {CompletedAmount}/{TotalAmount} Leśnego Shigi!",
                ["8"] = "Zabij {CompletedAmount}/{TotalAmount} Arktycznego Shigi!",
                ["9"] = "Zabij {CompletedAmount}/{TotalAmount} Tokar!",
                ["10"] = "Zabij {CompletedAmount}/{TotalAmount} Papuge Tokar!",
                ["11"] = "Zabij {CompletedAmount}/{TotalAmount} Niebieskiego Tokar!",
                ["12"] = "Zabij {CompletedAmount}/{TotalAmount} Leśnego Yeti!",
                ["13"] = "Zabij {CompletedAmount}/{TotalAmount} Yeti!",
            }, this, "pl");

            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["newtask"] = "New task available!",
                ["reward"] = "You received a reward for completing the task!",
                ["1"] = "Deal {CompletedAmount}/{TotalAmount} damage to creatures!",
                ["2"] = "Deal {CompletedAmount}/{TotalAmount} damage to players!",
                ["3"] = "Kill {CompletedAmount}/{TotalAmount} players!",
                ["4"] = "Kill {CompletedAmount}/{TotalAmount} Bor!",
                ["5"] = "Kill {CompletedAmount}/{TotalAmount} Radiated Bor!",
                ["6"] = "Kill {CompletedAmount}/{TotalAmount} Shigi!",
                ["7"] = "Kill {CompletedAmount}/{TotalAmount} Forest Shigi!",
                ["8"] = "Kill {CompletedAmount}/{TotalAmount} Arctic Shiga!",
                ["9"] = "Kill {CompletedAmount}/{TotalAmount} Tokar!",
                ["10"] = "Kill {CompletedAmount}/{TotalAmount} Papuge Tokar!",
                ["11"] = "Kill {CompletedAmount}/{TotalAmount} Blue Tokar!",
                ["12"] = "Kill {CompletedAmount}/{TotalAmount} Forest Yeti!",
                ["13"] = "Kill {CompletedAmount}/{TotalAmount} Yeti!",
            }, this);

            // French
            lang.RegisterMessages(new Dictionary<string, string>
            {

            }, this, "fr");

            // German
            lang.RegisterMessages(new Dictionary<string, string>
            {

            }, this, "de");

            // Russian
            lang.RegisterMessages(new Dictionary<string, string>
            {

            }, this, "ru");

            // Spanish
            lang.RegisterMessages(new Dictionary<string, string>
            {

            }, this, "es");
        }

        #endregion

        #region GlobalVariables

        TaskInformation[] Tasks = new TaskInformation[39]
        {
            new TaskInformation("1", "CreatureDamage", 500),
            new TaskInformation("1", "CreatureDamage", 1250),
            new TaskInformation("1", "CreatureDamage", 1750),

            new TaskInformation("2", "PlayerDamage", 250),
            new TaskInformation("2", "PlayerDamage", 500),
            new TaskInformation("2", "PlayerDamage", 750),

            new TaskInformation("3", "Player", 2),
            new TaskInformation("3", "Player", 5),
            new TaskInformation("3", "Player", 10),

            new TaskInformation("4", "aiborserver(clone)", 5),
            new TaskInformation("4", "aiborserver(clone)", 10),
            new TaskInformation("4", "aiborserver(clone)", 15),

            new TaskInformation("5", "aiborradserver(clone)", 5),
            new TaskInformation("5", "aiborradserver(clone)", 10),
            new TaskInformation("5", "aiborradserver(clone)", 15),

            new TaskInformation("6", "aishigiserver(clone)", 5),
            new TaskInformation("6", "aishigiserver(clone)", 10),
            new TaskInformation("6", "aishigiserver(clone)", 15),

            new TaskInformation("7", "aishigiforestserver(clone)", 5),
            new TaskInformation("7", "aishigiforestserver(clone)", 10),
            new TaskInformation("7", "aishigiforestserver(clone)", 15),

            new TaskInformation("8", "aishigiarcticserver(clone)", 5),
            new TaskInformation("8", "aishigiarcticserver(clone)", 10),
            new TaskInformation("8", "aishigiarcticserver(clone)", 15),

            new TaskInformation("9", "aitokarserver(clone)", 5),
            new TaskInformation("9", "aitokarserver(clone)", 10),
            new TaskInformation("9", "aitokarserver(clone)", 15),

            new TaskInformation("10", "aitokarparrotserver(clone)", 5),
            new TaskInformation("10", "aitokarparrotserver(clone)", 10),
            new TaskInformation("10", "aitokarparrotserver(clone)", 15),

            new TaskInformation("11", "aitokarblueserver(clone)", 5),
            new TaskInformation("11", "aitokarblueserver(clone)", 10),
            new TaskInformation("11", "aitokarblueserver(clone)", 15),

            new TaskInformation("12", "aiyetiforestserver(clone)", 1),
            new TaskInformation("12", "aiyetiforestserver(clone)", 2),
            new TaskInformation("12", "aiyetiforestserver(clone)", 3),

            new TaskInformation("13", "aiyetiserver(clone)", 1),
            new TaskInformation("13", "aiyetiserver(clone)", 2),
            new TaskInformation("13", "aiyetiserver(clone)", 3),
        };
        TaskInformation CurrentTask = new TaskInformation("None", "None", 0);
        int TaskInt = -1;
        float TimeToNewTask = 0.0f;

        #endregion

        #region Initialization

        private void Init()
        {
            GenerateNewTask();
        }

        void GenerateNewTask()
        {
            DateTime CT = DateTime.Now;

            TimeToNewTask += (24 - CT.Hour) * 60 * 60;
            TimeToNewTask += (60 - CT.Minute) * 60;
            TimeToNewTask += 60 - CT.Second;

            GetANewTask();

            timer.Once(TimeToNewTask, () =>
            {
                GenerateNewTask();

                Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

                while (PlayerSession.MoveNext())
                {
                    AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("newtask", this, PlayerSession.Current.Value.SteamId.ToString()), PlayerSession.Current.Key);

                    break;
                }
            });
        }

        void GetANewTask()
        {
            System.Random Random = new System.Random();
            int NewTaskInt = Random.Next(0, Tasks.Length);

            if (TaskInt != NewTaskInt)
            {
                TaskInt = NewTaskInt;
                CurrentTask = Tasks[TaskInt];
            }
            else
            {
                while (TaskInt == NewTaskInt)
                {
                    NewTaskInt = Random.Next(0, Tasks.Length);

                    if (TaskInt != NewTaskInt)
                    {
                        TaskInt = NewTaskInt;
                        CurrentTask = Tasks[TaskInt];

                        Config.Clear();

                        break;
                    }
                }
            }
        }

        #endregion

        #region Tasks

        void OnPlayerDeath(PlayerSession session, EntityEffectSourceData source)
        {
            if (CurrentTask.EntityName == "Player")
            {
                string SDKey = !string.IsNullOrEmpty(source.SourceDescriptionKey) ? source.SourceDescriptionKey : Singleton<GameManager>.Instance.GetDescriptionKey(source.EntitySource);

                if (SDKey.EndsWith("(P)"))
                {
                    string KillerName = SDKey.Substring(0, SDKey.Length - 3);
                    Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

                    while (PlayerSession.MoveNext())
                    {
                        if (PlayerSession.Current.Value.IPlayer.Name == KillerName)
                        {
                            AddPlayerCompleteTaskAmount(PlayerSession.Current.Key, PlayerSession.Current.Value.SteamId.ToString(), 1);

                            break;
                        }
                    }
                }
            }
        }

        void OnPlayerTakeDamage(PlayerSession session, EntityEffectSourceData source)
        {
            if (CurrentTask.EntityName == "PlayerDamage")
            {
                string SDKey = !string.IsNullOrEmpty(source.SourceDescriptionKey) ? source.SourceDescriptionKey : Singleton<GameManager>.Instance.GetDescriptionKey(source.EntitySource);

                if (SDKey.EndsWith("(P)"))
                {
                    string KillerName = SDKey.Substring(0, SDKey.Length - 3);
                    Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

                    while (PlayerSession.MoveNext())
                    {
                        if (PlayerSession.Current.Value.IPlayer.Name == KillerName)
                        {
                            AddPlayerCompleteTaskAmount(PlayerSession.Current.Key, PlayerSession.Current.Value.SteamId.ToString(), source.Value);

                            break;
                        }
                    }
                }
            }
        }

        void OnEntityDeath(AnimalStatManager stats, EntityEffectSourceData source)
        {
            if (CurrentTask.EntityName == "aiborserver(clone)" || CurrentTask.EntityName == "aiborradserver(clone)" || CurrentTask.EntityName == "aishigiserver(clone)" || CurrentTask.EntityName == "aishigiforestserver(clone)" || CurrentTask.EntityName == "aishigiarcticserver(clone)" || CurrentTask.EntityName == "aitokarserver(clone)" || CurrentTask.EntityName == "aitokarparrotserver(clone)" || CurrentTask.EntityName == "aitokarblueserver(clone)" || CurrentTask.EntityName == "aiyetiforestserver(clone)" || CurrentTask.EntityName == "aiyetiserver(clone)")
            {
                if (CurrentTask.EntityName == stats.name)
                {
                    string SDKey = !string.IsNullOrEmpty(source.SourceDescriptionKey) ? source.SourceDescriptionKey : Singleton<GameManager>.Instance.GetDescriptionKey(source.EntitySource);

                    if (SDKey.EndsWith("(P)"))
                    {
                        string KillerName = SDKey.Substring(0, SDKey.Length - 3);
                        Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

                        while (PlayerSession.MoveNext())
                        {
                            if (PlayerSession.Current.Value.IPlayer.Name == KillerName)
                            {
                                AddPlayerCompleteTaskAmount(PlayerSession.Current.Key, PlayerSession.Current.Value.SteamId.ToString(), 1);

                                break;
                            }
                        }
                    }
                }
            }
        }

        void OnEntityTakeDamage(AIEntity entity, EntityEffectSourceData source)
        {
            if (CurrentTask.EntityName == "AnimalDamage") { }
        }

        #endregion

        #region Functions

        void AddPlayerCompleteTaskAmount(uLink.NetworkPlayer NewtworkPlayer, string Steam64, float TaskAmountToAdd)
        {
            if (Config[Steam64] == null)
            {
                Config.Set(Steam64, TaskAmountToAdd);
                SaveConfig();
            }
            else if (!(float.Parse(Config[Steam64].ToString()) >= CurrentTask.AmountToComplete))
            {
                Config.Set(Steam64, float.Parse(Config[Steam64].ToString()) + TaskAmountToAdd);
                SaveConfig();
            }
            else { return; }

            AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage(CurrentTask.Task, this, Steam64)
                .Replace("{CompletedAmount}", Config[Steam64].ToString())
                .Replace("{TotalAmount}", CurrentTask.AmountToComplete.ToString())
                , NewtworkPlayer);

            if (float.Parse(Config[Steam64].ToString()) >= CurrentTask.AmountToComplete)
            {
                GivePlayerTaskReward(NewtworkPlayer, Steam64);
            }
        }

        void GivePlayerTaskReward(uLink.NetworkPlayer NewtworkPlayer, string Steam64)
        {
            AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("reward", this, Steam64), NewtworkPlayer);
        }

        #endregion

        #region Commands

        [Command("task")]
        private void taskCommand(IPlayer Sender, string command, string[] args)
        {
            Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

            while (PlayerSession.MoveNext())
            {
                if (Sender.Name == PlayerSession.Current.Value.IPlayer.Name)
                {
                    if (Config[Sender.Id] == null)
                    {
                        AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage(CurrentTask.Task, this, Sender.Id)
                            .Replace("{CompletedAmount}", "0")
                            .Replace("{TotalAmount}", CurrentTask.AmountToComplete.ToString())
                            , PlayerSession.Current.Key);
                    }
                    else
                    {
                        AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage(CurrentTask.Task, this, Sender.Id)
                            .Replace("{CompletedAmount}", Config[Sender.Id].ToString())
                            .Replace("{TotalAmount}", CurrentTask.AmountToComplete.ToString())
                            , PlayerSession.Current.Key);
                    }
                }

                break;
            }
        }

        #endregion
    }
}
