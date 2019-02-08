// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Set a New MasterClient. Can only be done on the current MasterClient.")]
//	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W922")]
	public class PhotonNetworkSetMasterClient : FsmStateAction
	{
		[Tooltip("The Photon player ID, if none, will switch to the next player in the room")]
		public FsmInt playerID;

		[Tooltip("false if setting the master failed, true if request was executed")]
		[UIHint(UIHint.Variable)]
		public FsmBool result;

		[Tooltip("event sent if request was executed")]
		public FsmEvent successEvent;

		[Tooltip("event sent if request was not executed, likely because not on the master")]
		public FsmEvent errorEvent;

		[Tooltip("Event sent if ID was not found or next player was not found neither")]
		public FsmEvent playerIdNotFoundEvent;

		public override void Reset()
		{
			playerID = new FsmInt() {UseVariable=true};
			playerIdNotFoundEvent = null;
		}

		bool _result;
		Player _p;
		public override void OnEnter()
		{
			if (playerID.IsNone) {
				_p = PhotonNetwork.MasterClient.GetNext();
			} else {
				_p = PhotonNetwork.LocalPlayer.Get(playerID.Value);

			}

			if (_p == null) {
				Fsm.Event(playerIdNotFoundEvent);
			} else {
				_result = PhotonNetwork.SetMasterClient (_p);
			}

			if (!result.IsNone) {
				result.Value = _result;
			}

			Fsm.Event (_result ? successEvent : errorEvent);

			Finish();
		}

	}
}