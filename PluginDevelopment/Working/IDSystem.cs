using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info ("ID System", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 1) ]
    [Description ("Easier Player Identification For Hurtworld.") ]

    class IDSystem : CovalencePlugin
    {
        public struct PlayerInformation { public string SteamID; public string OriginalName; };
        PlayerInformation[] PlayerID = new PlayerInformation[100];

        #region Initialization

            void OnPlayerConnected(PlayerSession player)
            {
                for (int Pid = 1; Pid < PlayerID.Length; Pid++)
                {
                    if (PlayerID[Pid].SteamID == null)
                    {
                        PlayerID[Pid].SteamID = player.IPlayer.Id;
                        PlayerID[Pid].OriginalName = player.IPlayer.Name;
                        player.IPlayer.Name = "(" + Pid.ToString() + ") " + player.IPlayer.Name;

                        break;
                    }
                }
            }
            
            void OnPlayerDisconnected(PlayerSession player)
            {
                for (int Pid = 1; Pid < PlayerID.Length; Pid++)
                {
                    if (PlayerID[Pid].SteamID == player.IPlayer.Id)
                    {
                        player.IPlayer.Rename("(OFFLINE) " + PlayerID[Pid].OriginalName);
                        PlayerID[Pid].SteamID = null;

                        break;
                    }
                }
            }

        #endregion

        #region UsefulFuncitons

            public string GetPlayerSteamID(int ID)
            {
                return PlayerID[ID].SteamID;
            }

            public string GetPlayerOriginalName(int ID)
            {
                return PlayerID[ID].OriginalName;
            }

            public int FindIDPlayer(IPlayer Player)
            {
                for (int Pid = 1; Pid < PlayerID.Length; Pid++)
                {
                    if (PlayerID[Pid].SteamID == Player.Id)
                    {
                        return Pid;
                    }
                }

                return 0;
            }

            public int FindIDSession(PlayerSession session)
            {
                for (int Pid = 1; Pid < PlayerID.Length; Pid++)
                {
                    if (PlayerID[Pid].SteamID == session.SteamId.ToString())
                    {
                        return Pid;
                    }
                }

                return 0;
            }

            public int[] GetTakenIDs(IPlayer Player)
            {
                List<int> ConnectedPlayerID = new List<int>();

                for (int Pid = 1; Pid < PlayerID.Length; Pid++)
                {
                    if ((PlayerID[Pid].SteamID != null) && (Player.Id != PlayerID[Pid].SteamID))
                    {
                        ConnectedPlayerID.Add(Pid);
                    }
                }

                return ConnectedPlayerID.ToArray();
            }

        #endregion
    }
}
