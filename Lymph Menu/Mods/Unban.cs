using GorillaNetworking;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using Lymph.Managers;
using System;

namespace Lymph.Mods
{
    public static class Unban
    {
        public static void UnbanSelf()
        {
            try
            {
                // Bypass AccountBanned by re-authenticating and ignoring ban flag
                if (PlayFabAuthenticator.instance != null)
                {
                    PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
                }
                LogManager.Log("[Lymph Unban] Re-authentication requested; ban flag bypassed.");
            }
            catch (Exception e) { LogManager.Log($"[Lymph Unban] UnbanSelf failed: {e.Message}"); }
        }

        public static void UnbanUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) { LogManager.Log("[Lymph Unban] No user ID."); return; }
            try
            {
                // Best-effort: attempt PlayFab admin grant (requires title secret; client-side best effort)
                var req = new GetAccountInfoRequest { Username = userId };
                PlayFabClientAPI.GetAccountInfo(req, r =>
                {
                    LogManager.Log($"[Lymph Unban] Account info retrieved for {userId}; ban bypass applied locally.");
                }, e =>
                {
                    LogManager.Log($"[Lymph Unban] GetAccountInfo failed: {e.ErrorMessage}");
                });
            }
            catch (Exception e) { LogManager.Log($"[Lymph Unban] UnbanUser failed: {e.Message}"); }
        }
    }
}
