// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Request a client to disconnect (KICK). Only the master client can do this" +
		"Only the target player gets this event. That player will disconnect automatically, which is what the others will notice, too")]
//	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W922")]
	public class PhotonNetworkCloseConnection : FsmStateAction
	{
		[Tooltip("The Photon player Id")]
		[RequiredField]
		public FsmInt playerId;

		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("false if there is no known room or game server to return to. True will attempt reconnection")]
		public FsmBool result;
		
		[Tooltip("Event to send if the reconnection will be attempted")]
		public FsmEvent willProceed;
		
		[Tooltip("Event to send if there is no known room or game server to return to")]
		public FsmEvent willNotProceed;

		public override void Reset()
		{
			playerId = null;
			result = null;
			willProceed = null;
			willNotProceed = null;
		}

		public override void OnEnter()
		{
			bool _result =	PhotonNetwork.CloseConnection(PhotonNetwork.LocalPlayer.Get(playerId.Value));
			
            if (!result.IsNone)
            {
				result.Value = _result;
			}

			Fsm.Event(_result ? willProceed : willNotProceed);
			Finish();
		}

	}
}