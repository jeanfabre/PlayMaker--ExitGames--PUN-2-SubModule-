// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you broadcast a Fsm Event to photon targets ( all players, other players, master).")]
	[HelpUrl("")]
	public class PhotonViewRpcBroadcastFsmEvent : PunComponentActionBase<PhotonView>
	{
		[Tooltip("The rpc targets.")]
		[ObjectType(typeof(RpcTarget))]
        public FsmEnum  rpcTargets;
		
		
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event you want to send.")]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optional string data ( will be injected in the Event data. Use 'get Event Info' action to retrieve it)")]
		public FsmString stringData;

		
		RpcTarget _rpcTargets;
		
	
		public override void Reset()
		{
			// JFF: how can I set this silently without a public variable? if I set it to private, it doesn't work anymore. maybe I forgot a setting?
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			
			remoteEvent = null;
			rpcTargets = null;
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

			_rpcTargets = (RpcTarget)rpcTargets.Value;
			
			if (eventTarget.target == FsmEventTarget.EventTarget.BroadcastAll)
			{
				
				if (! stringData.IsNone && stringData.Value != ""){
					_proxy.PhotonRpcBroadcastFsmEventWithString(_rpcTargets, remoteEvent.Name,stringData.Value);
				}else{
					_proxy.PhotonRpcBroadcastFsmEvent(_rpcTargets, remoteEvent.Name);
				}
			}else{
				
				if (PlayMakerPhotonProxy.Instance==null)
				{
						Debug.LogWarning("PlayMakerPhotonProxy is missing");
					return;
				}
				
				if (! stringData.IsNone && stringData.Value != ""){
			//		PlayMakerPhotonProxy.Instance.PhotonRpcSendFsmEventWithString(_rpcTargets, remoteEvent.Name,stringData.Value);
				}else{
			//		PlayMakerPhotonProxy.Instance.PhotonRpcSendFsmEvent(_rpcTargets,remoteEvent.Name);
				}
			}
			
		}
		
		
		
		public override string ErrorCheck()
		{
			if (eventTarget.target != FsmEventTarget.EventTarget.BroadcastAll)
			{
				return "eventTarget must be set to broadcast";	
			}
			
			if (remoteEvent == null)
			{
				return "Remote Event not set";
			}
	
            return string.Empty;
		}
		
		
		
	}
}