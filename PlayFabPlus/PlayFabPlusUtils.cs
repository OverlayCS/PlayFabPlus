using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab;
using System;
using PlayFab.PlayFabPlus;
using System.Collections.Generic;

namespace PlayFab.PlayFabPlus
{
    public class CoinService
    {
        public int GetVirtualCurreny(Action<int> OnGetVirtualCurrenySuccess, Action<int> OnGetVirtualCurrenyFailed)
        {
            int coins = 0;

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess =>
            {
                coins = OnGetUserInventorySuccess.VirtualCurrency[PlayFabPlusUtils.GetCurrencyCode()];
                OnGetVirtualCurrenySuccess.Invoke(coins);
            }, OnError =>
            {
                OnGetVirtualCurrenyFailed.Invoke(0);
            });

            return 0;
        }

        public void AddVirtualCurreny(int amount, Action<ExecuteCloudScriptResult> OnAddVirtualCurrenySuccess, Action<PlayFabError> OnAddVirtualCurrenyFailed)
        {
            if (amount == 0) { return; }
            var Request = new ExecuteCloudScriptRequest
            {
                FunctionName = "GivePlayerCurrency",
                FunctionParameter = new { PlayerToGiveID = PlayFabPlusCore.GetPlayFabID(), CCode = PlayFabPlusUtils.GetCurrencyCode(), AmountToGive = amount }
            };
            PlayFabClientAPI.ExecuteCloudScript(Request, OnAddVirtualCurrenySuccess, OnAddVirtualCurrenyFailed);
        }
        public void RemoveVirtualCurreny(int amount, Action<ExecuteCloudScriptResult> OnAddVirtualCurrenySuccess, Action<PlayFabError> OnAddVirtualCurrenyFailed)
        {
            if(amount == 0) { return; }

            var Request = new ExecuteCloudScriptRequest
            {
                FunctionName = "RemovePlayerCurrency",
                FunctionParameter = new { PlayFabId = PlayFabPlusCore.GetPlayFabID(), CCode = PlayFabPlusUtils.GetCurrencyCode(), AmountToRemove = amount }
            };
            PlayFabClientAPI.ExecuteCloudScript(Request, OnAddVirtualCurrenySuccess, OnAddVirtualCurrenyFailed);
        }

    }

    public class InventoryService
    {
        public List<ItemInstance> OwnedItems { get { return GetOwnedUserItems(); } }

        private List<ItemInstance> GetOwnedUserItems()
        {
            if (!PlayFabPlusCore.Spamming() && PlayFabPlusCore.IsLoggedIn())
            {
                List<ItemInstance> itemInstances = new List<ItemInstance>();
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest { }, success => { itemInstances = success.Inventory; }, error => { Debug.LogError("Found error while getting items:  " + error.ErrorMessage); });
                return itemInstances;
            }
            return null;
        }

        public void GrantItemToPlayer(string ItemID, string CatlogName, string ID = "")
        {
            if (!PlayFabPlusCore.Spamming() && PlayFabPlusCore.IsLoggedIn() && !string.IsNullOrEmpty(ItemID) && !string.IsNullOrEmpty(CatlogName))
            {
                string TargetID = "";
                if (ID == string.Empty)
                    TargetID = PlayFabPlusCore.GetPlayFabID();
                else
                    TargetID = ID;

                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest { FunctionName = "GrantItemToPlayer", FunctionParameter = new { GrantPlayerID = TargetID, Ver = CatlogName, GrantItemID = ItemID } }, r => { }, ErrorHandle =>
                {
                    Debug.LogError("Found error while granting item:  " + ErrorHandle.ErrorMessage);
                });
            }
        }
    }

    public class AchievementService
    {
        //Coming in later updates
    }

    public class PlayerDataService
    {
        //Coming in later updates
    }

    public class LeaderboardService
    {
        //Coming in later updates
    }

    public class TitleDataService
    {
        //Coming in later updates
    }

    public class OculusService
    {
        //Coming in later updates
    }

    public class FriendService
    {
        //Coming in later updates
    }

    public static class PlayFabPlusUtils
    {
        public static string GetCurrencyCode()
        {
            return PlayFabPlusCore.CurrenyCode;
        }
#if RISKY_PLAYFAB_FUCTIONS
        public static void BanPlayer(string ID, string reason, int time, Action<ExecuteCloudScriptResult> OnBanPlayerSuccess, Action<PlayFabError> OnBanPlayerFailed)
        {
            var Request = new ExecuteCloudScriptRequest
            {
                FunctionName = "BanPlayer",
                FunctionParameter = new { PlayFabId = ID, Reason = reason, Duration = time }
            };
            PlayFabClientAPI.ExecuteCloudScript(Request, OnBanPlayerSuccess, OnBanPlayerFailed);
        }
#endif
    }
}