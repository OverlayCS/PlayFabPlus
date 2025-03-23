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
    }
}