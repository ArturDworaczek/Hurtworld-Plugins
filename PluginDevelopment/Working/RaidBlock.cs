using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;
using Vector3 = UnityEngine.Vector3;

namespace Oxide.Plugins
{
    [Info("Raid Block", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 12)]
    [Description("Raid block for Hurtworld.")]

    class RaidBlock : CovalencePlugin
    {
        #region Globals

        public struct RaidBlockZone
        {
            public Vector3 Location;
            public float TimeLeft;

            public RaidBlockZone(Vector3 NewLocation, float NewTimeLeft)
            {
                Location = NewLocation;
                TimeLeft = NewTimeLeft;
            }
        };

        IList<IPlayer> RaidBlockedPlayers = new List<IPlayer>();
        IList<RaidBlockZone> RaidBlockedZones = new List<RaidBlockZone>();

        Timer RaidBlockTimer = null;
        float ZoneSize, RaidTimer;

        void Loaded()
        {
            LoadDefaultConfig();
            LoadMessages();
        }

        protected override void LoadDefaultConfig()
        {
            if (Config["ZoneSize"] == null || !float.TryParse(Config["ZoneSize"].ToString(), out ZoneSize))
            {
                ZoneSize = 20.0f;
                Config.Set("ZoneSize", ZoneSize);
            }

            if (Config["RaidTimer"] == null || !float.TryParse(Config["RaidTimer"].ToString(), out RaidTimer))
            {
                RaidTimer = 180.0f;
                Config.Set("RaidTimer", RaidTimer);
            }

            SaveConfig();
        }

        #endregion

        #region Languages

        void LoadMessages()
        {
            // Polish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["raidblock"] = "+ Blokada Rajdowa",
                ["raidunblock"] = "- Blokada Rajdowa",
            }, this, "pl");

            // English
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["raidblock"] = "+ Raid block",
                ["raidunblock"] = "- Raid block",
            }, this);

            // French
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["raidblock"] = "+ blocage du raid",
                ["raidunblock"] = "- Bloc de raid",
            }, this, "fr");

            // German
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["raidblock"] = "+ RAID-Block",
                ["raidunblock"] = "- RAID-Block",
            }, this, "de");

            // Russian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["raidblock"] = "+ Рейдовый блок",
                ["raidunblock"] = "- Рейдовый блок",
            }, this, "ru");

            // Spanish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["raidblock"] = "+ Raid block",
                ["raidunblock"] = "- Bloqueo Raid",
            }, this, "es");
        }

        #endregion

        #region Initialization

        void OnItemExploded(ExplosiveDynamicServer ItemExploded, IExplosion Explosion)
        {
            if (ItemExploded.name == "C4DynamicObject(Clone)" || ItemExploded.name == "TerritoryC4DynamicObject(Clone)")
            {
                bool bZoneInterfering = false;
                Vector3 ExplosionLocation = ItemExploded.networkView.InitialPosition;
                
                foreach (RaidBlockZone RBZ in RaidBlockedZones)
                {
                    double Distance = Math.Sqrt(Math.Pow(ExplosionLocation.x - RBZ.Location.x, 2) + Math.Pow(ExplosionLocation.y - RBZ.Location.y, 2));

                    if (Distance <= ZoneSize)
                    {
                        bZoneInterfering = true;
                    }
                }

                if (!bZoneInterfering)
                {
                    RaidBlockedZones.Add(new RaidBlockZone(ExplosionLocation, RaidTimer));

                    if (RaidBlockTimer == null)
                    {
                        StartTimer();
                    }

                    Puts("Created new raid zone: " + ExplosionLocation.ToString());
                }
            }
        }

        #endregion

        #region Functions

        void StartTimer()
        {
            RaidBlockTimer = timer.Repeat(1, 0, () =>
            {
                if (RaidBlockedZones.Count > 0)
                {
                    IList<RaidBlockZone> NewRaidBlockedZones = new List<RaidBlockZone>();

                    foreach (RaidBlockZone RBZ in RaidBlockedZones)
                    {
                        NewRaidBlockedZones.Insert(RaidBlockedZones.IndexOf(RBZ), new RaidBlockZone(RBZ.Location, RBZ.TimeLeft - 1));

                        if (RBZ.TimeLeft <= 0)
                        {
                            NewRaidBlockedZones.RemoveAt(RaidBlockedZones.IndexOf(RBZ));
                        }

                        foreach (IPlayer Player in players.Connected)
                        {
                            GenericPosition PlayerPos = Player.Position();

                            double Distance = Math.Sqrt(Math.Pow(RBZ.Location.x - PlayerPos.X, 2) + Math.Pow(RBZ.Location.y - PlayerPos.Y, 2));
                            if (!RaidBlockedPlayers.Contains(Player) && Distance <= ZoneSize)
                            {
                                RaidBlockedPlayers.Add(Player);

                                AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("raidblock", this, Player.Id), GetNetworkPlayer(Player));
                            }
                            else if (RaidBlockedPlayers.Contains(Player) && Distance > ZoneSize)
                            {
                                RaidBlockedPlayers.Remove(Player);

                                AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("raidunblock", this, Player.Id), GetNetworkPlayer(Player));
                            }
                        }
                    }

                    RaidBlockedZones = NewRaidBlockedZones;
                }
                else { EndTimer(); }
            });
        }

        void EndTimer()
        {
            RaidBlockTimer.Destroy();
            RaidBlockTimer = null;

            foreach (IPlayer PlayerRaidBlocked in RaidBlockedPlayers)
            {
                AlertManager.Instance.GenericTextNotificationServer(lang.GetMessage("raidunblock", this, PlayerRaidBlocked.Id), GetNetworkPlayer(PlayerRaidBlocked));
            }

            RaidBlockedPlayers = new List<IPlayer>();
            RaidBlockedZones = new List<RaidBlockZone>();
        }

        #endregion

        #region Useful Functions

        bool IsPlayerRaidBlocked(IPlayer Player)
        {
            foreach (IPlayer PlayerRaidBlocked in RaidBlockedPlayers)
            {
                if (PlayerRaidBlocked.Name == Player.Name)
                {
                    return true;
                }
            }

            return false;
        }

        uLink.NetworkPlayer GetNetworkPlayer(IPlayer Player)
        {
            Dictionary<uLink.NetworkPlayer, PlayerSession>.Enumerator PS = GameManager.Instance.GetSessions().GetEnumerator();

            while (PS.MoveNext())
            {
                if (PS.Current.Value.SteamId.ToString() == Player.Id)
                {
                    return PS.Current.Key;
                }
            }

            return new uLink.NetworkPlayer(-1);
        }

        #endregion
    }
}
