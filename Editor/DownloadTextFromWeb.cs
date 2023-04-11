using System;
using System.Collections;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.AVT.FetchGoogleSheet
{
    public static class DownloadTextFromWeb
    {
        public static void GetRawTextFromUrl(this UnityEngine.Object obj, string url, Action<bool, string> onGetResult)
        {
            #if UNITY_EDITOR
            EditorCoroutineUtility.StartCoroutine(IGetRequest(url, onGetResult), obj);
            #endif
        }

        private static IEnumerator IGetRequest(string url, Action<bool, string> onGetResult)
        {
            using var webRequest = UnityWebRequest.Get(url);
            // Request and wait for the desired page.
            Debug.Log($"<b>GET</b> data from <color=yellow>{url}</color>");
                
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
                onGetResult?.Invoke(true, webRequest.downloadHandler.text);
            else
                onGetResult?.Invoke(false, webRequest.result.ToString());
        }
    }
}
