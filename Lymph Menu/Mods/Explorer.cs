using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using Lymph.Managers;
using System;
using System.Linq;

namespace Lymph.Mods
{
    public static class Explorer
    {
        public static void ListRooms()
        {
            if (!PhotonNetwork.IsConnected) { LogManager.Log("[Lymph Explorer] Not connected."); return; }
            LogManager.Log("[Lymph Explorer] Current room: " + (PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : "(none)"));
            LogManager.Log($"[Lymph Explorer] Region: {PhotonNetwork.CloudRegion}");
            LogManager.Log($"[Lymph Explorer] Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
            foreach (var p in PhotonNetwork.PlayerList)
            {
                LogManager.Log($"   - {p.NickName} (UID={p.UserId}) Actor={p.ActorNumber} Master={p.IsMasterClient}");
            }
        }

        public static void ListCosmetics()
        {
            if (CosmeticsController.instance == null) { LogManager.Log("[Lymph Explorer] CosmeticsController not available."); return; }
            LogManager.Log($"[Lymph Explorer] Total cosmetics: {CosmeticsController.instance.allCosmetics.Count}");
            LogManager.Log($"[Lymph Explorer] Unlocked: {CosmeticsController.instance.unlockedCosmetics.Count}");
            foreach (var c in CosmeticsController.instance.allCosmetics.Take(50))
            {
                LogManager.Log($"   {c.displayName} ({c.itemName}) price={c.cost} tryOn={c.canTryOn}");
            }
        }

        public static void ListPlayers()
        {
            if (!PhotonNetwork.IsConnected) { LogManager.Log("[Lymph Explorer] Not connected."); return; }
            foreach (var p in PhotonNetwork.PlayerList)
            {
                LogManager.Log($"[Lymph Explorer] {p.NickName} | UID={p.UserId} | Actor={p.ActorNumber} | Master={p.IsMasterClient}");
            }
        }
    }
}

