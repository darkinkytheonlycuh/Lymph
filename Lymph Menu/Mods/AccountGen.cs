using GorillaNetworking;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using Lymph.Managers;
using System;

namespace Lymph.Mods
{
    public static class AccountGen
    {
        public static string GeneratedSessionId = "";
        public static string GeneratedPlayFabId = "";

        public static void GenerateSession()
        {
            GeneratedSessionId = Guid.NewGuid().ToString("N");
            GeneratedPlayFabId = "LYMPH_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            LogManager.Log($"[Lymph AccountGen] Generated SessionId: {GeneratedSessionId}");
            LogManager.Log($"[Lymph AccountGen] Generated PlayFabId: {GeneratedPlayFabId}");
        }

        public static void EquipSession()
        {
            if (string.IsNullOrEmpty(GeneratedSessionId)) GenerateSession();
            try
            {
                if (PhotonNetwork.LocalPlayer != null)
                {
                    PhotonNetwork.LocalPlayer.UserId = GeneratedPlayFabId;
                    PhotonNetwork.LocalPlayer.NickName = "LymphUser";
                }
                LogManager.Log($"[Lymph AccountGen] Equipped session {GeneratedSessionId} mid-game.");
            }
            catch (Exception e) { LogManager.Log($"[Lymph AccountGen] Equip failed: {e.Message}"); }
        }
    }
}
