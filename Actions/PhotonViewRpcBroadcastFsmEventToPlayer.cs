// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you send a Fsm Event to a specific photon player.")]
	//[HelpUrl("")]
	public class PhotonViewRpcBroadcastFsmEventToPlayer : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The targeted player")]
		public PlayerReferenceProperty player;
		
		//JFF: TOFIX: SHOULD NOT BE PUBLIC, BUT I NEED THIS TO DISPLAY GLOBAL EVENTS 
		[Tooltip("Leave to BroadCastAll.")]
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event you want to send.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optional string data ( will be injected in the Event data. Use 'get Event Info' action to retrieve it)")]
		public FsmString stringData;

		private Player _player;
		
		public override void Reset()
		{
			player = null;
	
			// JFF: how can I set this silently without a public variable? if I set it to private, it doesn't work anymore. maybe I forgot a setting?
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			remoteEvent = null;
			stringData = null;
		}

		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}

		void ExecuteAction()
		{
			
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
			
			if (go == null )
			{
				return;
			}
			
			if (remoteEvent.Name ==""){
				return;
			}
			
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				return;
			}

			_player = player.GetPlayer(this);

			if (_player == null)
			{
				return;
			}
			
			if (! stringData.IsNone && stringData.Value != ""){
				_proxy.PhotonRpcFsmBroadcastEventWithString(_player,remoteEvent.Name,stringData.Value);
			}else{
				_proxy.PhotonRpcBroadcastFsmEvent(_player,remoteEvent.Name);
			}	
		}
	}
}