using UnityEngine;
using UnityEditor;
using PlayFab;
using PlayFab.PlayFabPlus;

namespace Assets.PlayFabPlus.Editor
{
    public class PlayFabPlusEditor : EditorWindow
    {
        private string titleID;
        private string virtualCurrencyID;
        private const string DefineSymbol = "RISKY_PLAYFAB_FUCTIONS";

        [MenuItem("Tools/PlayFab++ Settings")]
        public static void ShowWindow()
        {
            GetWindow<PlayFabPlusEditor>("PlayFab++ Settings");
        }

        private void OnEnable()
        {
            LoadSettings();
        }

        private void OnGUI()
        {
            GUILayout.Label("PlayFab++ Settings", EditorStyles.boldLabel);

            titleID = EditorGUILayout.TextField("Title ID", titleID);
            virtualCurrencyID = EditorGUILayout.TextField("Virtual Currency ID", virtualCurrencyID);

            GUILayout.Space(10);

            // Toggle for enabling/disabling the directive
            bool isEnabled = IsSymbolDefined(DefineSymbol);
            bool newIsEnabled = EditorGUILayout.Toggle("Enable PlayFab++ Features", isEnabled);

            if (newIsEnabled != isEnabled)
            {
                if (newIsEnabled)
                    AddDefineSymbol(DefineSymbol);
                else
                    RemoveDefineSymbol(DefineSymbol);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Save Settings"))
            {
                SaveSettings();
            }
        }

        private void LoadSettings()
        {
            titleID = PlayerPrefs.GetString("PlayFab_TitleID", "");
            virtualCurrencyID = PlayerPrefs.GetString("PlayFab_VirtualCurrencyID", "");
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetString("PlayFab_TitleID", titleID);
            PlayerPrefs.SetString("PlayFab_VirtualCurrencyID", virtualCurrencyID);
            PlayFabSettings.TitleId = titleID;
            PlayFabPlusCore.CurrenyCode = virtualCurrencyID;
            PlayerPrefs.Save();
            Debug.Log("PlayFab++ Settings Saved!");
        }

        // Check if the define symbol is present
        private bool IsSymbolDefined(string symbol)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            return defines.Contains(symbol);
        }

        // Add the define symbol
        private void AddDefineSymbol(string symbol)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!defines.Contains(symbol))
            {
                defines = string.IsNullOrEmpty(defines) ? symbol : defines + ";" + symbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
                Debug.Log($"Added Define Symbol: {symbol}");
            }
        }

        // Remove the define symbol
        private void RemoveDefineSymbol(string symbol)
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (defines.Contains(symbol))
            {
                defines = defines.Replace(symbol, "").Replace(";;", ";").Trim(';');
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
                Debug.Log($"Removed Define Symbol: {symbol}");
            }
        }
    }
}
