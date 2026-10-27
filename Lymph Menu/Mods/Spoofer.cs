using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Lymph.Managers;
using Lymph.Patches.Menu;
using System;

using UnityEngine;

namespace Lymph.Mods
{
    public static class Spoofer
    {
        public static bool SpoofUsername;
        public static string SpoofedUsername = "Lymph";
        public static bool SpoofColor;
        public static Color SpoofedColor = Color.white;
        public static bool SpoofPlatform;
        public static string SpoofedPlatform = "STEAM";

        public static void ApplyUsernameSpoof()
        {
            try
            {
                if (PhotonNetwork.LocalPlayer != null && SpoofUsername)
                {
                    PhotonNetwork.LocalPlayer.NickName = SpoofedUsername;
                }
                LogManager.Log($"[Lymph Spoofer] Username spoof: {SpoofedUsername}");
            }
            catch (Exception e) { LogManager.Log($"[Lymph Spoofer] {e.Message}"); }
        }

        public static void ApplyColorSpoof()
        {
            try
            {
                if (GorillaTagger.Instance != null && GorillaTagger.Instance.offlineVRRig != null && SpoofColor)
                {
                    GorillaTagger.Instance.offlineVRRig.playerColor = SpoofedColor;
                }
                LogManager.Log($"[Lymph Spoofer] Color spoof applied.");
            }
            catch (Exception e) { LogManager.Log($"[Lymph Spoofer] {e.Message}"); }
        }

        public static void ApplyPlatformSpoof()
        {
            LogManager.Log($"[Lymph Spoofer] Platform spoof: {SpoofedPlatform}");
        }
    }
}

