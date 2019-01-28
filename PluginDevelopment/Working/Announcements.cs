using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Announcements", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 4)]
    [Description("Custom announcements by Admins / Console.")]

    class Announcements : CovalencePlugin
    {
        #region Globals

        Timer PrefixedMsgTimer = null;
        int MessageIndex = 1;
        int MaxMessageIndex = 5;
        float SendDelay = 120.0f;
        string ServerName = "<color=#FFFF00>[NewHurtworldOrder]</color> ";

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
                ["1"] = "Witamy na serwerze <color=#FFFF00>NewHurtworldOrder</color>.",
                ["2"] = "Wszelakie pytania proszę kierować do administracji serwera.",
                ["3"] = "Możliwość kupna '<color=#FFFF00>premium</color>' jest dostępna na stronie 'NewHurtworldOrder.pl'.",
                ["4"] = "Cały zespół <color=#FFFF00>NewHurtworldOrder</color> życzy miłej rozgrywki na serwerze.",
                ["5"] = "Używanie cheatów / bugów może skutować bardzo surową karą na serwerze.",
            }, this, "pl");

            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["1"] = "Welcome to the server <color=#FFFF00>NewHurtworldOrder</color>.",
                ["2"] = "Please direct all questions to server administration.",
                ["3"] = "Opportunity to buy '<color=#FFFF00>premium</color>' is available on the 'NewHurtworldOrder.pl' website.",
                ["4"] = "The whole team <color=#FFFF00>NewHurtworldOrder</color> wishes you a nice time on the server.",
                ["5"] = "Using cheats / bugs can result in a very severe penalty on the server.",
            }, this);

            // French
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["1"] = "Bienvenue sur le serveur <color=#FFFF00>NewHurtworldOrder</color>.",
                ["2"] = "Veuillez adresser toutes vos questions à l'administration du serveur.",
                ["3"] = "Possibilité d'acheter '<color=#FFFF00>premium</color>' est disponible sur le site Web de 'NewHurtworldOrder.pl'.",
                ["4"] = "Toute l'équipe <color=#FFFF00>NewHurtworldOrder</color> vous souhaite un bon moment sur le serveur.",
                ["5"] = "L'utilisation de cheats / bugs peut entraîner une pénalité très sévère pour le serveur.",
            }, this, "fr");

            // German
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["1"] = "Willkommen auf dem Server <color=#FFFF00>NewHurtworldOrder</color>.",
                ["2"] = "Bitte richten Sie alle Fragen an die Serververwaltung.",
                ["3"] = "Die Option '<color=#FFFF00>premium</color>' ist auf der Website 'NewHurtworldOrder.pl' verfügbar.",
                ["4"] = "Das gesamte Team <color=#FFFF00>NewHurtworldOrder</color> wünscht Ihnen eine schöne Zeit auf dem Server.",
                ["5"] = "Die Verwendung von Cheats / Bugs kann auf dem Server zu erheblichen Strafen führen.",
            }, this, "de");

            // Russian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["1"] = "Добро пожаловать на сервер <color=#FFFF00>NewHurtworldOrder</color>.",
                ["2"] = "Пожалуйста, направляйте все вопросы в администрацию сервера.",
                ["3"] = "Возможность купить '<color=#FFFF00>premium</color>' доступна на веб-сайте 'NewHurtworldOrder.pl'",
                ["4"] = "Вся команда <color=#FFFF00>NewHurtworldOrder</color> желает вам приятно провести время на сервере.",
                ["5"] = "Использование читов / ошибок может привести к очень суровому наказанию на сервере.",
            }, this, "ru");

            // Spanish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["1"] = "Bienvenido al servidor <color=#FFFF00>NewHurtworldOrder</color>.",
                ["2"] = "Dirija todas las preguntas a la administración del servidor.",
                ["3"] = "La oportunidad de comprar '<color=#FFFF00>premium</color>' está disponible en el sitio web 'NewHurtworldOrder.pl'.",
                ["4"] = "Todo el equipo <color=#FFFF00>NewHurtworldOrder</color> te desea un buen momento en el servidor.",
                ["5"] = "El uso de trampas / errores puede resultar en una penalización muy severa para el servidor.",
            }, this, "es");
        }

        #endregion

        #region Initialization

        private void Init()
        {
            if (!permission.PermissionExists("Announcements.Use", this))
            {
                permission.RegisterPermission("Announcements.Use", this);
            }

            SendPrefixedMessage();
        }

        #endregion

        #region Functions

        void SendPrefixedMessage()
        {
            PrefixedMsgTimer = timer.Once(SendDelay, () =>
            {
                foreach (IPlayer Player in players.Connected)
                {
                    Player.Message(ServerName + lang.GetMessage(MessageIndex.ToString(), this, Player.Id));
                }
                if (MessageIndex == MaxMessageIndex) { MessageIndex = 1; } else { MessageIndex++; }

                SendPrefixedMessage();
            });
        }

        #endregion

        #region Commands

        [Command("am")]
        private void ACommand(IPlayer Sender, string command, string[] args)
        {
            if (permission.UserHasGroup(Sender.Id, "admin") || permission.UserHasGroup(Sender.Id, "support"))
            {
                if (args.Length > 0)
                {
                    DateTime CT = DateTime.Now;
                    string argsStr = string.Join(" ", args);
                    string Message = "<color=#FF0000>[Admin Message</color> <color=#FFFFFF>~ " + Sender.Name + "</color><color=#FF0000>]</color> " + argsStr;

                    Puts("[" + CT.ToString() + "] " + "[Admin Message ~ " + Sender.Name + "] " + argsStr);

                    foreach (IPlayer Player in players.Connected)
                    {
                        Player.Message(Message);
                    }
                }
                else
                {
                    Sender.Message("Syntax: /am [Message]");
                }
            }
        }

        #endregion
    }
}
