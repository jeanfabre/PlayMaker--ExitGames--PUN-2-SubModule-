// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
        "Uses: PhotonNetwork.ConnectToBestCloudServer()")]
	public class Pun2ConnectToBestCloudServer : FsmStateAction
	{
		
		[Tooltip("The AppId. Leave to none or empty to use the one from the Server Settings")]
		public FsmString appIdRealtime;

        public FsmBool resetBestRegionInPref;


        public override void Reset()
        {
            resetBestRegionInPref = false;
            appIdRealtime = new FsmString(){UseVariable=true};
		}

		public override void OnEnter()
		{
			// reset authentication failure properties.
			PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
			PlayMakerPhotonProxy.lastAuthenticationFailed=false;

            #if !(UNITY_WINRT || UNITY_WP8 || UNITY_PS3 || UNITY_WIIU)
            if (!appIdRealtime.IsNone || string.IsNullOrEmpty(appIdRealtime.Value))
            {
                PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            }
            else
            {
                PhotonNetwork.NetworkingClient.AppId = appIdRealtime.Value;
            }

            if (resetBestRegionInPref.Value)
            {
                ServerSettings.ResetBestRegionCodeInPreferences();
            }

            PhotonNetwork.ConnectToBestCloudServer();

            #else
                Debug.Log("Connect to Best Server is not available on this platform");
            #endif
            
            Finish();
		}
		
		public override string ErrorCheck()
		{
			#if (UNITY_WINRT || UNITY_WP8 || UNITY_PS3 || UNITY_WIIU)
				return "Connect to Best Server is not available on this platform, the normal connection protocol will be used instead.";
			#endif	
			
			return string.Empty;
		}

	}
}