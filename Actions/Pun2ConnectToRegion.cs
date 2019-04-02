// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network region: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
		"Uses: Connect(string serverAddress, int port, string uniqueGameID)")]
	public class Pun2ConnectToRegion : FsmStateAction
	{

		[Tooltip("The region")]
		public FsmString region;


		public override void Reset()
		{
			region = null;
		
		}

		public override void OnEnter()
		{
			// reset authentication failure properties.
			PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
			PlayMakerPhotonProxy.lastAuthenticationFailed=false;

			PhotonNetwork.ConnectToRegion(region.Value);



			Finish();
		}

	}
}