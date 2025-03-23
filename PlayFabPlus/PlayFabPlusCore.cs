using Oculus.Platform;
using Oculus.Platform.Models;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace PlayFab.PlayFabPlus
{
    public static class PlayFabPlusCore
    {
        private static string PlayFabID;
        private static string oculus_profileLink;
        private static string oculus_userId;
        private static string oculus_username;
        private static string oculus_displayname;

        public static string CurrenyCode;

        public static string GetPlayFabID()
        {
            if (PlayFabID == null)
            {
                Debug.Log("No PlayFab Id Found");
                return "No PlayFab ID";
            }
            return PlayFabID;
        }

        public static string GetOculusProfileLink()
        {
            if (oculus_profileLink == null)
            {
                Debug.Log("No Oculus Profile Link Found");
                return "No Oculus Profile Link";
            }
            return oculus_profileLink;
        }

        public static string GetOculusUserId()
        {
            if (oculus_userId == null)
            {
                Debug.Log("No Oculus User Id Found");
                return "No Oculus User Id";
            }
            return oculus_userId;
        }

        public static string GetOculusUsername()
        {
            if (oculus_username == null)
            {
                Debug.Log("No Oculus Username Found");
                return "No Oculus Username";
            }
            return oculus_username;
        }

        public static string GetOculusDisplayName()
        {
            if (oculus_displayname == null)
            {
                Debug.Log("No Oculus Display Name Found");
                return "No Oculus Display Name";
            }
            return oculus_displayname;
        }

        public static void LoginWithCustomID(string customID, Action OnLoginSuccessAction, Action OnLoginFailedAction, Dictionary<string, string> CustomTags = null,
            GetPlayerCombinedInfoRequestParams InfoParms = null)
        {
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
            {
                CustomId = customID,
                CustomTags = CustomTags,
                InfoRequestParameters = InfoParms,
                CreateAccount = true
            }, OnLoginSuccess =>
            {
                OnLoginSuccessAction.Invoke();
            }, OnLoginFailure =>
            {
                OnLoginFailedAction.Invoke();
            });
        }

        public static void LoginWithOculus(Action OnLoginSuccessAction, Action OnLoginFailedAction, Dictionary<string, string> CustomTags = null,
            GetPlayerCombinedInfoRequestParams InfoParms = null)
        {
            Core.AsyncInitialize().OnComplete(CoreOnComplete =>
            {
                if (CoreOnComplete.IsError)
                {
                    Debug.LogError("Oculus CoreOnComplete Initialization Failed: " + CoreOnComplete.GetError());
                    return;
                }

                Entitlements.IsUserEntitledToApplication().OnComplete(Entitlementscallback =>
                {
                    if (Entitlementscallback.IsError)
                    {
                        Debug.LogError("Meta Platform entitlement error: " + Entitlementscallback.GetError());
                        return;
                    }

                    Users.GetLoggedInUser().OnComplete(UserCallbacks =>
                    {
                        if (UserCallbacks.IsError || UserCallbacks.GetUser() == null)
                        {
                            Debug.LogError("Failed to get logged-in user: " + UserCallbacks.GetError());
                            return;
                        }

                        User user = UserCallbacks.GetUser();
                        oculus_userId = user.ID.ToString();
                        oculus_profileLink = user.ImageURL;
                        oculus_username = user.OculusID;
                        oculus_displayname = user.DisplayName;

                        Users.GetUserProof().OnComplete(proofCallbacks =>
                        {
                            if (proofCallbacks.IsError || proofCallbacks.GetUserProof() == null)
                            {
                                Debug.LogError("Failed to get user proof: " + (proofCallbacks.IsError ? proofCallbacks.GetError().Message : "Null proof received"));
                                return;
                            }
                            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                            {
                                CreateAccount = true,
                                CustomId = "META" + PlayFabSettings.DeviceUniqueIdentifier,
                                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                                {
                                    GetUserAccountInfo = true,
                                    GetUserInventory = true
                                },
                                CustomTags = CustomTags
                            }, RetrieveData =>
                            {
                                OnLoginSuccessAction.Invoke();
                            }, error =>
                            {
                                OnLoginFailedAction.Invoke();
                                Debug.LogError("Error with logging in: " + error.ErrorMessage);
                            });
                        });
                    });
                });
            });
        }
    }
}
