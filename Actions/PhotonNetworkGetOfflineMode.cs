// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Checks if the PhotonNetwork runs offline or not")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1108")]
	public class PhotonNetworkGetOfflineMode : FsmStateAction
	{
		[Tooltip("True if PhotonNetwork works offline")]
		[UIHint(UIHint.Variable)]
		public FsmBool offlineMode;

		[Tooltip("Event to send if offline mode is true.")]
		public FsmEvent isOfflineEvent;
		
		[Tooltip("Event to send if offline mode if false")]
		public FsmEvent isOnlineEvent;

		public override void Reset()
		{
			offlineMode  = null;
			isOfflineEvent = null;
			isOnlineEvent = null;
		}

		public override void OnEnter()
		{
			bool _offline = PhotonNetwork.OfflineMode;

			if (!offlineMode.IsNone)
			{
				offlineMode.Value = _offline;
			}

			Fsm.Event(_offline?isOfflineEvent:isOnlineEvent);

			Finish();
		}

	}
}