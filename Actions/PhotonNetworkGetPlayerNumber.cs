// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Pun 2")]
	[Tooltip("Get a PhotonPlayer Player number, requires PlayerNumbering component to be in the scene")]
	public class PhotonNetworkGetPlayerNumber : FsmStateAction
	{
	
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached to get the related PhotonPlayer.")]
		public FsmOwnerDefault gameObject;


		[UIHint(UIHint.Variable)]
		[Tooltip("The room index of the PhotonPlayer owning this photonView.")]
		public FsmInt playerRoomIndex;

		private GameObject go;
		
		private PhotonView _networkView;

		public override void Reset()
		{
			gameObject = null;
			playerRoomIndex = null;
		}

		public override void OnEnter()
		{
			_getNetworkView();

			getPlayerRoomIndex();

			Finish();

		}

		void getPlayerRoomIndex()
		{
			playerRoomIndex.Value = _networkView.Owner.GetPlayerNumber ();
			
		}

		private void _getNetworkView()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_networkView =  go.GetComponent<PhotonView>();
		}


	}
}