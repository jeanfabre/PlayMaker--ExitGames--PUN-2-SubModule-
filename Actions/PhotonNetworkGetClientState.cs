// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Gets Photon networking client connection state")]
//[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W904")]
	public class PhotonNetworkGetClientState : FsmStateAction
	{

        [Tooltip("The current client state")]
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(ClientState))]
        public FsmEnum clientState;

        [Tooltip("The previous client state")]
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(ClientState))]
        public FsmEnum previousClientState;

        [Tooltip("Event sent when client state changed")]
        public FsmEvent OnClientStateChanged;

		[Tooltip("Repeat every frame. Useful for watching the network state over time.")]
		public bool everyFrame;

		public override void Reset()
		{
            clientState = ClientState.Disconnected;
            previousClientState = ClientState.Disconnected;

            OnClientStateChanged = null;
            everyFrame = false;
		}

		public override void OnEnter()
		{
            PhotonNetwork.NetworkingClient.StateChanged += NetworkingClient_StateChanged;

            previousClientState.Value = PhotonNetwork.NetworkClientState;
            clientState.Value = PhotonNetwork.NetworkClientState;

            if (!everyFrame)
			{
                Finish();
			}
        }

        public override void OnExit()
        {
            PhotonNetwork.NetworkingClient.StateChanged -= NetworkingClient_StateChanged;
        }

        void NetworkingClient_StateChanged(ClientState arg1, ClientState arg2)
        {
            previousClientState.Value = arg1;
            clientState.Value = arg2;

            
          //  OnClientStateChanged
        }

	}
}