using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Kill Counter", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 13)]
    [Description("Kill counter for players in Hurtworld.")]

    class KillCounter : CovalencePlugin
    {
        #region Hooks

        void OnPlayerDeath(PlayerSession session, EntityEffectSourceData source)
        {
            string SDKey = !string.IsNullOrEmpty(source.SourceDescriptionKey) ? source.SourceDescriptionKey : Singleton<GameManager>.Instance.GetDescriptionKey(source.EntitySource);

            if (SDKey.EndsWith("(P)"))
            {
                string KillerName = SDKey.Substring(0, SDKey.Length - 3);

                foreach (IPlayer Player in players.Connected)
                {
                    if (Player.Name == KillerName)
                    {
                        DynamicConfigFile PlayerData;

                        if (Interface.Oxide.DataFileSystem.ExistsDatafile(Player.Id))
                        {
                            int KillAmount = 0;
                            PlayerData = Interface.Oxide.DataFileSystem.GetDatafile(Player.Id);

                            if (int.TryParse(PlayerData["Kills"].ToString(), out KillAmount))
                            {
                                PlayerData["Kills"] = KillAmount++;
                            }
                            else
                            {
                                PlayerData["Kills"] = 1;
                            }
                        }
                        else
                        {
                            PlayerData = Interface.Oxide.DataFileSystem.GetDatafile(Player.Id);
                            PlayerData["Kills"] = 1;
                        }

                        PlayerData.Save();

                        break;
                    }
                }
            }
        }

        #endregion

        #region Useful Functions

        public int GetPlayerKills(string Steam64)
        {
            if (Interface.Oxide.DataFileSystem.ExistsDatafile(Steam64))
            {
                int KillAmount = 0;
                DynamicConfigFile PlayerData = Interface.Oxide.DataFileSystem.GetDatafile(Steam64);

                if (int.TryParse(PlayerData["Kills"].ToString(), out KillAmount))
                {
                    return KillAmount;
                }
            }

            return 0;
        }

        #endregion
    }
}
