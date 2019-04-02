// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Disconnect to Photon Network: \n" +
		"Makes this client disconnect from the photon server, a process that leaves any room and calls OnDisconnectedFromPhoton on completion.")]
	[HelpUrl("")]
	public class Pun2Disconnect : FsmStateAction
	{
		public override void OnEnter()
		{
			PhotonNetwork.Disconnect();
			Finish();
		}
	}
}