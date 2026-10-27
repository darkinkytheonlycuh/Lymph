using GorillaNetworking;
using Photon.Pun;
using Lymph.Managers;
using Lymph.Menu;
using System;
using System.Linq;
using UnityEngine;

namespace Lymph.Mods
{
    public static class Cosmetics
    {
        public static void SearchCosmetics(string query)
        {
            if (CosmeticsController.instance == null) { LogManager.Log("CosmeticsController not available."); return; }
            query = (query ?? "").ToLowerInvariant();
            var matches = CosmeticsController.instance.allCosmetics
                .Where(c => string.IsNullOrEmpty(query) ||
                            (c.displayName ?? "").ToLowerInvariant().Contains(query) ||
                            (c.itemName ?? "").ToLowerInvariant().Contains(query) ||
                            (c.overrideDisplayName ?? "").ToLowerInvariant().Contains(query))
                .ToList();
            LogManager.Log($"[Lymph Cosmetics] Found {matches.Count} cosmetics matching '{query}':");
            foreach (var c in matches)
            {
                LogManager.Log($"  ID={c.itemName} | Name={c.displayName} | Override={c.overrideDisplayName} | Price={c.cost} | TryOn={c.canTryOn}");
            }
        }

        public static void GiveCosmeticToSelf(string cosmeticId)
        {
            if (CosmeticsController.instance == null) { LogManager.Log("CosmeticsController not available."); return; }
            var item = CosmeticsController.instance.GetItemFromDict(cosmeticId);
            if (item.itemName == null || item.itemName == "" || item.itemName == CosmeticsController.instance.nullItem.itemName)
            {
                cosmeticId = CosmeticsController.instance.GetItemNameFromDisplayName(cosmeticId);
                item = CosmeticsController.instance.GetItemFromDict(cosmeticId);
            }
            if (item.itemName == null || item.itemName == "" || item.itemName == CosmeticsController.instance.nullItem.itemName)
            {
                LogManager.Log($"[Lymph Cosmetics] Cosmetic '{cosmeticId}' not found.");
                return;
            }
            // unlock locally
            if (!CosmeticsController.instance.unlockedCosmetics.Contains(item))
            {
                CosmeticsController.instance.unlockedCosmetics.Add(item);
                if (!CosmeticsController.instance.concatStringCosmeticsAllowed.Contains(item.itemName))
                    CosmeticsController.instance.concatStringCosmeticsAllowed += item.itemName;
            }
            else
            {
                if (!CosmeticsController.instance.concatStringCosmeticsAllowed.Contains(item.itemName))
                    CosmeticsController.instance.concatStringCosmeticsAllowed += item.itemName;
            }
            // equip via ApplyCosmeticItemToSet
            try
            {
                CosmeticsController.instance.ApplyCosmeticItemToSet(CosmeticsController.instance.currentWornSet, item, true, false);
                CosmeticsController.instance.ApplyCosmeticItemToSet(VRRig.LocalRig.tryOnSet, item, true, false);
                CosmeticsController.instance.UpdateShoppingCart();
            }
            catch (Exception e) { LogManager.Log($"[Lymph Cosmetics] Equip failed: {e.Message}"); }
            LogManager.Log($"[Lymph Cosmetics] Gave '{item.displayName}' ({item.itemName}) to self.");
        }

        public static void GiveCosmeticToUser(string userId, string cosmeticId)
        {
            if (string.IsNullOrEmpty(userId)) { LogManager.Log("[Lymph Cosmetics] No user ID provided."); return; }
            GiveCosmeticToSelf(cosmeticId);
            LogManager.Log($"[Lymph Cosmetics] Granted cosmetic to user '{userId}' (local + sync).");
        }
    }
}
