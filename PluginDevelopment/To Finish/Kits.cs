using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Kits", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 15)]
    [Description("Kits for players in Hurtworld.")]

    class Kits : CovalencePlugin
    {
        #region Languages

        void LoadMessages()
        {
            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["start"] = "Start",
                ["help"] = "Help",
                ["home"] = "Home",
                ["kitgive"] = "You have been given kit {kitname}!",
                ["nokit"] = "This kit doesn't exist!"
            }, this);

            // Polish
            lang.RegisterMessages(new Dictionary<string, string>
            {

            }, this, "pl");

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

        #region Structs
        struct KitData
        {
            public string KitName;
            public float KitTimer;
            public IList<int> ItemList;

            public KitData(string NewKitName, float KitTimer, IList<int> NewItemList)
            {
                KitName = NewKitName;
                KitTimer = NewKitTimer;
                ItemList = NewItemList;
            }
        }
        struct PlayerKitData
        {
            public string SteamID;
            public string KitName;
            public int AmountRedeemed;
            public Timer Timer;

            public PlayerData(string NewSteamID, string NewKitName, int NewAmountRedeemed, Timer NewTimer)
            {
                SteamID = NewSteamID;
                KitName = NewKitName;
                AmountRedeemed = NewAmountRedeemed;
                Timer = NewTimer;
            }
        }
        #endregion

        #region LoadConfig
        void Loaded()
        {
            LoadDefaultConfig();
        }

        void LoadDefaultConfig()
        {
            
        }
        #endregion

        #region Commands
        [Command("kit")]
        private void kitCommand(IPlayer Sender, string command, string[] args)
        {
            if (!(args[0] == null))
            {
                
            }
        }
        #endregion
    }
}
