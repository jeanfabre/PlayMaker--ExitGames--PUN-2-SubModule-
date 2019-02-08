// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the Photon network time, synched with the server. This time value depends on the server's Environment.TickCount. It is different per server" +
		"but inside a Room, all clients should have the same value (Rooms are on one server only)." +
		"" +
		"This is not a DateTime!  Use this value with care:" +
		"It can start with any positive value." +
		"It will 'wrap around' from 4294967.295 to 0!")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W904")]
	public class PhotonNetworkGetServerTime : FsmStateAction
	{
		[Tooltip("The Photon network time")]
		[UIHint(UIHint.Variable)]
		public FsmFloat serverTimeStamp;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			serverTimeStamp = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetTimeStamp();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetTimeStamp();
		}

		void DoGetTimeStamp()
		{
			serverTimeStamp.Value = (int)PhotonNetwork.ServerTimestamp;
		}
	}
}