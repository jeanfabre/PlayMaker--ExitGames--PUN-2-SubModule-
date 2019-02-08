// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get The current server's millisecond timestamp." +
		"This can be useful to sync actions and events on all clients in one room. The timestamp is based on the server's Environment.TickCount." +
		"It will overflow from a positive to a negative value every so often, so be careful to use only time-differences to check the time delta when things happen.")]
	//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W904")]
	public class PhotonNetworkGetServerTimeStamp : FsmStateAction
	{
		[Tooltip("The current server's millisecond timestamp")]
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
			DoGetTime();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetTime();
		}

		void DoGetTime()
		{
			serverTimeStamp.Value = (int)PhotonNetwork.Time;
		}
	}
}