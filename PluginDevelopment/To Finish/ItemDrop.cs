using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Item Drop", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 3)]
    [Description("Stops players from throwing too many things too fast.")]

    class ItemDrop : CovalencePlugin
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
                ["ThrowNotification"] = "Nie możesz wyrzucić niczego przez 60s!",
            }, this, "pl");

            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ThrowNotification"] = "Can't Throw Anything Away For 60s!",
            }, this);

            // French
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ThrowNotification"] = "Ne peut rien jeter pendant 60s!",
            }, this, "fr");

            // German
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ThrowNotification"] = "Kann nichts für 60s werfen!",
            }, this, "de");

            // Russian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ThrowNotification"] = "Не могу ничего бросить за 60-е годы!",
            }, this, "ru");

            // Spanish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ThrowNotification"] = "No puedo tirar nada en los años 60",
            }, this, "es");
        }

        #endregion

        #region Globals

        public struct PlayerDropInformation { public Inventory Inv; public int ItemsDropped; public Timer RestartTimer; };
        PlayerDropInformation[] Inventories = new PlayerDropInformation[100];

        float WaitDelay = 60.0f;
        bool bFound = false;

        #endregion

        #region Initialization

        object OnItemDrop(Inventory inventory, int slot)
        {
            uLink.NetworkPlayer NetworkPlayer = inventory.GetComponent<uLinkNetworkView>().owner;
            
            if (!IsPlayerAdminNewtorkPlayer(NetworkPlayer))
            {
                if (!(slot == 14))
                {
                    bFound = false;

                    for (int Index = 1; Index < Inventories.Length; Index++)
                    {
                        if (Inventories[Index].Inv == inventory)
                        {
                            bFound = true;

                            if (Inventories[Index].ItemsDropped < 10)
                            {
                                Inventories[Index].RestartTimer.Destroy();

                                Inventories[Index].ItemsDropped++;
                                Inventories[Index].RestartTimer = timer.Once(WaitDelay, () => { Inventories[Index].Inv = null; Inventories[Index].ItemsDropped = 0; });

                                return null;
                            }
                            else
                            {
                                AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("ThrowNotification", this, NetworkPlayer.id.ToString()), NetworkPlayer);

                                return false;
                            }
                        }
                    }

                    if (!bFound)
                    {
                        for (int Index = 1; Index < Inventories.Length; Index++)
                        {
                            if (Inventories[Index].Inv == null)
                            {
                                Inventories[Index].Inv = inventory;
                                Inventories[Index].ItemsDropped = 1;

                                Inventories[Index].RestartTimer = timer.Once(WaitDelay, () => { Inventories[Index].Inv = null; Inventories[Index].ItemsDropped = 0; Inventories[Index].RestartTimer = null; });

                                return null;
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region UsefulFunction

        bool IsPlayerAdminNewtorkPlayer(uLink.NetworkPlayer NewtworkPlayer)
        {
            Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PlayerSession = GameManager.Instance.GetSessions().GetEnumerator();

            while (PlayerSession.MoveNext())
            {
                if (NewtworkPlayer.ipAddress == PlayerSession.Current.Key.ipAddress)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}

