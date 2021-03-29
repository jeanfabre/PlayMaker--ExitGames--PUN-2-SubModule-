// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Set the name of the Photon player User Id.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W922")]
	public class PhotonNetworkSetPlayerUserId : FsmStateAction
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
			if (userId== null)
			{
				return;
			}
			
		//	PhotonNetwork.LocalPlayer.UserId = userId.Value;
			
			Finish();
		}

	}
}