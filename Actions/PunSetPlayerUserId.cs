// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Set the name of the Photon player User Id.\n" +
	         "Available when the room got created with RoomOptions.PublishUserId = true.\n" +
	         "Useful for PhotonNetwork.FindFriends and blocking slots in a room for expected players (e.g. in PhotonNetwork.CreateRoom).")]
	[HelpUrl("")]
	public class PunSetPlayerUserId : FsmStateAction
	{
		[Tooltip("The Photon player userId")]
		[RequiredField]
		public FsmString userId;
		
		public override void Reset()
		{
			userId = null;
		}

		public override void OnEnter()
		{
			if (!userId.IsNone)
			{
				PhotonNetwork.LocalPlayer.UserId = userId.Value;
			}
			else
			{
				LogError("userId undefined");
			}
			
			Finish();
		}

	}
}