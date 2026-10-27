using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Lymph.Managers;
using Lymph.Menu;
using System;
using UnityEngine;

namespace Lymph.Mods
{
    public static class LymphExtras
    {
        public static bool flyEnabled;
        public static float flySpeed = 5f;

        public static void EnableFly()
        {
            flyEnabled = true;
            if (GorillaTagger.Instance != null)
                GorillaTagger.Instance.rigidbody.useGravity = false;
            LogManager.Log("[Lymph Extras] Fly: ON");
        }

        public static void DisableFly()
        {
            flyEnabled = false;
            if (GorillaTagger.Instance != null)
                GorillaTagger.Instance.rigidbody.useGravity = true;
            LogManager.Log("[Lymph Extras] Fly: OFF");
        }

        public static void SpeedBoost()
        {
            if (GorillaTagger.Instance != null)
            {
                if (GorillaTagger.Instance != null) GorillaTagger.Instance.transform.localScale *= 1.1f;
            }
            LogManager.Log("[Lymph Extras] Speed boost applied.");
        }

        public static void ResetSpeed()
        {
            if (GorillaTagger.Instance != null)
            {
                if (GorillaTagger.Instance != null) GorillaTagger.Instance.transform.localScale /= 1.1f;
            }
            LogManager.Log("[Lymph Extras] Speed reset.");
        }

        public static void HealSelf()
        {
            LogManager.Log("[Lymph Extras] Heal (no-op on this version).");
            LogManager.Log("[Lymph Extras] Healed.");
        }

        public static void GiveAllCosmetics()
        {
            if (CosmeticsController.instance == null) { LogManager.Log("CosmeticsController not available."); return; }
            foreach (var c in CosmeticsController.instance.allCosmetics)
            {
                if (!CosmeticsController.instance.unlockedCosmetics.Contains(c))
                {
                    CosmeticsController.instance.unlockedCosmetics.Add(c);
                    if (!CosmeticsController.instance.concatStringCosmeticsAllowed.Contains(c.itemName))
                        CosmeticsController.instance.concatStringCosmeticsAllowed += c.itemName;
                }
            }
            CosmeticsController.instance.UpdateShoppingCart();
            LogManager.Log($"[Lymph Extras] Unlocked all {CosmeticsController.instance.allCosmetics.Count} cosmetics.");
        }
    }
}

