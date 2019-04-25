// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Destroy all GameObjects/PhotonViews of this player. can only be called on the local player. The only exception is the master client which can call this for all players.")]
	[HelpUrl("")]
	public class PhotonNetworkDestroyPlayer : PunActionBase
	{
		[RequiredField]
		[Tooltip("Destroys this Player. If left to none, destroy the local player")]
		public FsmString playerName;

		public override void Reset()
		{
			playerName = new FsmString(){UseVariable=true};
			
		}

		public override void OnEnter()
		{
			doDestroyPlayer();
			
			Finish();
		}
		
		
		void doDestroyPlayer()
		{
			
			if (playerName.IsNone)
			{
				PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
			}else{
				foreach(Player _player in PhotonNetwork.PlayerListOthers)
				{
					if (string.Equals(_player.NickName,playerName.Value))
					{
						PhotonNetwork.DestroyPlayerObjects(_player);
						return;
					}
				}
			}
			
		}// doDestroy
	
	}
}