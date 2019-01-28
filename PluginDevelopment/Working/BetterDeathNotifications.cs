using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Better Death Notifications", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 9)]
    [Description("Better Death notifications for Hurtworld.")]

    class BetterDeathNotifications : CovalencePlugin
    {
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
                ["kill"] = "Zabiłeś {PlayerName}!",
                ["killedby"] = "Zabity przez {PlayerName}!",
            }, this, "pl");

            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["kill"] = "You killed {PlayerName}!",
                ["killedby"] = "Killed by {PlayerName}!",
            }, this);

            // French
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["kill"] = "Vous avez tué {PlayerName}!",
                ["killedby"] = "tué par {PlayerName}!",
            }, this, "fr");

            // German
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["kill"] = "Sie haben {PlayerName} getötet!",
                ["killedby"] = "Getötet von {PlayerName}!",
            }, this, "de");

            // Russian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["kill"] = "Вы убили {PlayerName}!",
                ["killedby"] = "Убит {PlayerName}!",
            }, this, "ru");

            // Spanish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["kill"] = "¡Mataste a {PlayerName}!",
                ["killedby"] = "¡Matado por {PlayerName}!",
            }, this, "es");
        }

        #endregion

        #region PlayerDeathNotifications

        void OnPlayerDeath(PlayerSession session, EntityEffectSourceData source)
        {
            string SDKey = !string.IsNullOrEmpty(source.SourceDescriptionKey) ? source.SourceDescriptionKey : Singleton<GameManager>.Instance.GetDescriptionKey(source.EntitySource);

            if (SDKey.EndsWith("(P)"))
            {
                int FoundPlayers = 0;
                string KillerName = SDKey.Substring(0, SDKey.Length - 3);
                Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

                while (PlayerSession.MoveNext())
                {
                    if (PlayerSession.Current.Value.IPlayer.Name == KillerName)
                    {
                        AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("kill", this, PlayerSession.Current.Value.SteamId.ToString()).Replace("{PlayerName}", session.IPlayer.Name), PlayerSession.Current.Key);
                        FoundPlayers++;
                    }
                    else if (PlayerSession.Current.Value.IPlayer.Name == session.IPlayer.Name)
                    {
                        AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("killedby", this, PlayerSession.Current.Value.SteamId.ToString()).Replace("{PlayerName}", KillerName), PlayerSession.Current.Key);
                        FoundPlayers++;
                    }

                    if (FoundPlayers == 2) { break; }
                }
            }
        }

        #endregion
    }
}