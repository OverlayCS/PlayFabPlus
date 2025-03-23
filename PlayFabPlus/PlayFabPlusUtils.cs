using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab;
using System;
using PlayFab.PlayFabPlus;

namespace Assets.PlayFabPlus
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
    }

	public static class PlayFabPlusUtils
	{
		public static string GetCurrencyCode()
		{
            return PlayFabPlusCore.CurrenyCode;
        }
	}
}