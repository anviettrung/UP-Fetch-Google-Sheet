using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;

namespace AVT.FetchGoogleSheet
{
    public static class FetchGoogleSheetUtility
    {
        public static bool IsFetching { get; private set; }

        #region Download Text From Web

        public static void GetRawTextFromUrl(string url, Action<bool, string> onGetResult)
        {
#if UNITY_EDITOR
            if (!IsFetching)
            {
                IsFetching = true;
                EditorCoroutineUtility.StartCoroutineOwnerless(IGetRequest(url, onGetResult));
            }
#endif
        }

        private static IEnumerator IGetRequest(string url, Action<bool, string> onGetResult)
        {
            using var webRequest = UnityWebRequest.Get(url);
            
            // Request and wait for the desired page.
            Debug.Log($"<color=yellow><b>GET</b></color> data from Google Sheet\n<i>{url}</i>");
                
            yield return webRequest.SendWebRequest();
            
            IsFetching = false;

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                onGetResult?.Invoke(true, webRequest.downloadHandler.text);
                Debug.Log($"<color=green><b>COMPLETE</b></color> get data from Google Sheet\n<i>{url}</i>");
            }
            else
            {
                onGetResult?.Invoke(false, webRequest.result.ToString());
                Debug.Log($"<color=red><b>FAILED</b></color> get data from Google Sheet: <color=red>{webRequest.result.ToString()}</color>\n<i>{url}</i>");
            }
        }

        #endregion
    }
}
