// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Leave a lobby to stop getting updates about available rooms." +
		"This does not reset PhotonNetwork.lobby! This allows you to join this particular lobby later easily." +
		"" +
		"The values countOfPlayers, countOfPlayersOnMaster, countOfPlayersInRooms and countOfRooms" +
		"are received even without being in a lobby." +
		"" +
		"You can use JoinRandomRoom without being in a lobby." +
		"Use autoJoinLobby to not join a lobby when you connect.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W915")]
	public class PhotonNetworkLeaveLobby : FsmStateAction
	{
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
			result = null;
			willProceed = null;
			willNotProceed = null;
		}


		public override void OnEnter()
		{
			bool _result = PhotonNetwork.LeaveLobby();

			if (!result.IsNone)
			{
				result.Value = _result;
			}
			
			Fsm.Event(_result ? willProceed : willNotProceed);

			Finish();
		}
	}
}