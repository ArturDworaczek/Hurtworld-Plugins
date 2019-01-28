// Requires: KillCounter

using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Better Chat", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 2)]
    [Description("Better chat for Hurtworld.")]

    class BetterChat : CovalencePlugin
    {
        double MaximumDistance = 20.0f;
        string AdminColour, SupportColour, VipColour;

        #region Loading

        KillCounter KillCounter;

        private void Loaded()
        {
            KillCounter = (KillCounter)Manager.GetPlugin("KillCounter");

            LoadPluginConfig();
        }

        void LoadPluginConfig()
        {
            if (Config["AdminColour"].ToString().Length == 7 && !(Config["AdminColour"] == null)) { AdminColour = Config["AdminColour"].ToString(); }
            else { AdminColour = "#FF0000"; Config.Set("AdminColour", AdminColour); }

            if (Config["SupportColour"].ToString().Length == 7 && !(Config["SupportColour"] == null)) { SupportColour = Config["SupportColour"].ToString(); }
            else { SupportColour = "#00FF00"; Config.Set("SupportColour", SupportColour); }

            if (Config["VipColour"].ToString().Length == 7 && !(Config["VipColour"] == null)) { VipColour = Config["VipColour"].ToString(); }
            else { VipColour = "#FFFF00"; Config.Set("VipColour", VipColour); }

            SaveConfig();
        }

        #endregion

        #region Initialization

        object OnPlayerChat(PlayerSession session, string message)
        {
            DateTime CT = DateTime.Now;

            string HurtworldMessage = "";
            if (permission.UserHasGroup(session.SteamId.ToString(), "admin"))
            {
                HurtworldMessage = "<color=" + AdminColour + ">[A]</color>";
            }
            else if (permission.UserHasGroup(session.SteamId.ToString(), "support"))
            {
                HurtworldMessage = "<color=" + SupportColour + ">[S]</color>";
            }
            else if(permission.UserHasGroup(session.SteamId.ToString(), "vip"))
            {
                HurtworldMessage = "<color=" + VipColour + ">[V]</color>";
            }
            HurtworldMessage += "<color=#FFFF00>[" + CT.Hour.ToString() + ":" + CT.Minute.ToString() + ":" + CT.Second.ToString() + "]</color> " + session.IPlayer.Name;
            HurtworldMessage += "<color=#FF0000> [" + KillCounter.GetPlayerKills(session.SteamId.ToString()).ToString() + "]</color>";

            string ServerMessage = "[" + CT.ToString() + "] (" + session.SteamId.ToString() + ") "+ session.IPlayer.Name + ": " + message;
            GenericPosition SenderPosition = session.IPlayer.Position();

            Puts(ServerMessage);

            foreach (IPlayer Player in players.Connected)
            {
                if (session.IPlayer != Player)
                {
                    GenericPosition ReceivingPosition = Player.Position();

                    double Distance = Math.Sqrt(Math.Pow(SenderPosition.X - ReceivingPosition.X, 2) + Math.Pow(SenderPosition.Y - ReceivingPosition.Y, 2));
                    if (Distance <= MaximumDistance)
                    {
                        Player.Reply(HurtworldMessage + ": " + message);
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
