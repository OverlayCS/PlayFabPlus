using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab;
using System;
using PlayFab.PlayFabPlus;

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
		public void AddVritualCurrency(int amount)
		}
		      var request = new PlayFabClientAPI.ExecuteCloudScriptRequest({
			 FunctionName = "GivePlayerCurrency",
			 FunctionParemeters = new {
				 PlayerToGiveID = 
			 }
			      
		      });
	        {
    }

	public static class PlayFabPlusUtils
	{
		public static string GetCurrencyCode()
		{
            return PlayFabPlusCore.CurrenyCode;
        }
	}
}
