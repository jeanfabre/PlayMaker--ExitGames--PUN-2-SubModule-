// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network using Server Settings: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
        "Uses: PhotonNetwork.ConnectUsingSettings()")]
	[HelpUrl("")]
	public class PhotonNetworkConnectUsingSettings : FsmStateAction
	{
		public override void OnEnter()
		{
			// reset authentication failure properties.
			PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
			PlayMakerPhotonProxy.lastAuthenticationFailed=false;

            PhotonNetwork.ConnectUsingSettings();
			
			Finish();
		}
	}
}