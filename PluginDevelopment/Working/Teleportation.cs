// Requires: IDSystem

using System;
using System.Collections.Generic;
using System.Linq;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Teleportation", "Artur 'NijoMeisteR' Dworaczek", "0.0.1", ResourceId = 6)]
    [Description("Teleportation For Hurtworld Administrators.")]

    class Teleportation : CovalencePlugin
    {
        #region Initialization
        IDSystem IDSystem;

        private void Loaded()
        {
            IDSystem = (IDSystem)Manager.GetPlugin("IDSystem");
        }

        #endregion

        #region Commands

        [Command("tp")]
        private void tpCommand(IPlayer Sender, string command, string[] args)
        {
            if (permission.UserHasGroup(Sender.Id, "admin") && args.Length > 0)
            {
                IPlayer Player = GetPlayer(args[0]);

                if (Player != null)
                {
                    if (Player.Id != Sender.Id)
                    {
                        GenericPosition PlayerPosition = Player.Position();

                        Sender.Teleport(PlayerPosition);
                        Sender.Reply("<color=#FF0000>[TP]</color> Teleporting to " + Player.Name + "!");
                    }
                    else
                    {
                        Sender.Reply("<color=#FF0000>[TP]</color> You can't tp to yourself!");
                    }
                }
                else
                {
                    Sender.Reply("<color=#FF0000>[TP]</color> Player not found!");
                }
            }
        }

        [Command("tph")]
        private void tpmeCommand(IPlayer Sender, string command, string[] args)
        {
            if (permission.UserHasGroup(Sender.Id, "admin") && args.Length > 0)
            {
                IPlayer Player = GetPlayer(args[0]);

                if (Player != null)
                {
                    if (Player.Id != Sender.Id)
                    {
                        GenericPosition PlayerPosition = Sender.Position();

                        Player.Teleport(PlayerPosition);
                        Sender.Reply("<color=#FF0000>[TP]</color> Teleporting " + Player.Name + " to you!");
                    }
                    else
                    {
                        Sender.Reply("<color=#FF0000>[TP]</color> You can't teleport yourself to yourself!");
                    }
                }
                else
                {
                    Sender.Reply("<color=#FF0000>[TP]</color> Player not found!");
                }
            }
        }

        struct PlayerTeleportRequest { public IPlayer RequestingPlayer; public IPlayer PlayerRequested; public Timer DelayTimer; };

        List<PlayerTeleportRequest> PlayersTP = new List<PlayerTeleportRequest>();
        float TeleportDelay = 5.0f;
        
        [Command("tpr")]
        private void tprCommand(IPlayer Sender, string command, string[] args)
        {
            if (permission.UserHasGroup(Sender.Id, "admin") && args.Length > 0)
            {
                foreach (PlayerTeleportRequest Request in PlayersTP)
                {
                    Request.DelayTimer.Destroy();
                    PlayersTP.Remove(Request);
                }

                int AmountOfPlayers = 0;

                if (Int32.TryParse(args[0], out AmountOfPlayers))
                {
                    int[] OccupiedIDs = IDSystem.GetTakenIDs(Sender);

                    if (OccupiedIDs.Length >= AmountOfPlayers)
                    {
                        for (int Index = 0; Index < AmountOfPlayers; Index++)
                        {
                            bool bNewPlayer = false;

                            while (!bNewPlayer) { bNewPlayer = RequestRandomTP(Sender, OccupiedIDs); }
                        }
                    }
                    else
                    {
                        Sender.Reply("<color=#FF0000>[TP]</color> There are currently not enough players on the server!");
                    }
                }
            }
        }

        [Command("tpa")]
        private void tpaCommand(IPlayer Sender, string command, string[] args)
        {
            PlayerTeleportRequest SlotToRemove = new PlayerTeleportRequest();

            foreach (PlayerTeleportRequest Request in PlayersTP)
            {
                if (Sender.Id == Request.PlayerRequested.Id)
                {
                    Sender.Reply("<color=#FF0000>[TP]</color> You have accepted the teleport request!");

                    Request.DelayTimer.Destroy();
                    Sender.Teleport(Request.RequestingPlayer.Position());

                    SlotToRemove = Request;
                }
            }

            if (SlotToRemove.PlayerRequested != null)
            {
                PlayersTP.Remove(SlotToRemove);
            }
        }

        #endregion

        #region UsefulCommands

        bool RequestRandomTP(IPlayer RequestingPlayer, int[] IDs)
        {
            IPlayer Player = PickPlayerAtRandom(IDs);

            foreach (PlayerTeleportRequest Request in PlayersTP)
            {
                if (Player.Id == Request.PlayerRequested.Id)
                {
                    return false;
                }
            }

            PlayerTeleportRequest NewPlayer = new PlayerTeleportRequest();
            NewPlayer.RequestingPlayer = RequestingPlayer;
            NewPlayer.PlayerRequested = Player;
            NewPlayer.DelayTimer = timer.Once(TeleportDelay, () =>
            {
                RequestANewPlayer(RequestingPlayer, IDs, NewPlayer);
            });

            PlayersTP.Add(NewPlayer);

            Player.Reply("<color=#FF0000>[TP]</color> You have been selected at random to teleport to an Admin!");
            Player.Reply("<color=#FF0000>[TP]</color> Use the command '/tpa' in " + TeleportDelay.ToString() + " seconds to accept the teleport request!");

            return true;
        }

        void RequestANewPlayer(IPlayer RequestingPlayer, int[] IDs, PlayerTeleportRequest NewPlayer)
        {
            NewPlayer.PlayerRequested.Reply("<color=#FF0000>[TP]</color> You failed to accept the teleport request in time!");

            RequestRandomTP(RequestingPlayer, IDs);
            PlayersTP.Remove(NewPlayer);
        }

        IPlayer PickPlayerAtRandom(int[] IDArray)
        {
            Random rnd = new Random();
            int PlayerID = rnd.Next(0, IDArray.Length);

            IPlayer Player = GetPlayer(IDArray[PlayerID].ToString());

            return Player;
        }

        IPlayer GetPlayer(string IdentificationNumber)
        {
            int Pid = 0;

            if (Int32.TryParse(IdentificationNumber, out Pid))
            {
                string SteamID = IDSystem.GetPlayerSteamID(Pid);

                foreach (IPlayer Player in players.Connected)
                {
                    if (Player.Id == SteamID)
                    {
                        return Player;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
