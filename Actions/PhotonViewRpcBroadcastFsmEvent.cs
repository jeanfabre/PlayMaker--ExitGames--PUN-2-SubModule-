// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you broadcast a Fsm Event to photon targets ( all players, other players, master).")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W920")]
	public class PhotonViewRpcBroadcastFsmEvent : FsmStateAction
	{
        public RpcTarget rpcTargets;
		
		// ONLY ACCEPTS BROADCAST OR SELF
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event you want to send.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optionnal string data ( will be injected in the Event data. Use 'get Event Info' action to retrieve it)")]
		public FsmString stringData;
		
	
		public override void Reset()
		{
			// JFF: how can I set this silently without a public variable? if I set it to private, it doesn't work anymore. maybe I forgot a setting?
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			
			remoteEvent = null;
			rpcTargets = RpcTarget.All;
			stringData = null;
		}

		public override void OnEnter()
		{
			DoREC();
			
			Finish();
		}

		void DoREC()
		{
			
			// get the photon proxy for Photon RPC access
			GameObject go = GameObject.Find("PlayMaker Photon Proxy");
			
			if (go == null )
			{
				return;
			}
			
			
			if (remoteEvent != null && remoteEvent.IsGlobal == false)
			{ 
				return;
			}
			
		
			// get the proxy component
			PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
			if (_proxy==null)
			{
				Debug.LogWarning("PlayMakerPhotonProxy is missing");
				return;
			}
			
			if (eventTarget.target == FsmEventTarget.EventTarget.BroadcastAll)
			{
				
				if (! stringData.IsNone && stringData.Value != ""){
					_proxy.PhotonRpcBroacastFsmEventWithString(rpcTargets, remoteEvent.Name,stringData.Value);
				}else{
					_proxy.PhotonRpcBroacastFsmEvent(rpcTargets, remoteEvent.Name);
				}
			}else{
				
				PlayMakerPhotonGameObjectProxy _goProxy = Owner.GetComponent<PlayMakerPhotonGameObjectProxy>();
				if (_proxy==null)
				{
						Debug.LogWarning("PlayMakerPhotonProxy is missing");
					return;
				}
				
				if (! stringData.IsNone && stringData.Value != ""){
					_goProxy.PhotonRpcSendFsmEventWithString(rpcTargets, remoteEvent.Name,stringData.Value);
				}else{
					_goProxy.PhotonRpcSendFsmEvent(rpcTargets,remoteEvent.Name);
				}
			}
			
		}
		
		
		
		public override string ErrorCheck()
		{
			
			if (remoteEvent == null)
			{
				return "Remote Event not set";
			}
			
			if (remoteEvent != null && !remoteEvent.IsGlobal)
			{
				return "Remote Event must be a global event";
			}

            return string.Empty;
		}
		
		
		
	}
}