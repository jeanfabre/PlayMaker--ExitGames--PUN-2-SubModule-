// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Retrieve the owner properties of a GameObject.\n A PhotonView component is required on the gameObject")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W919")]
	public class PhotonViewGetOwnerProperties : FsmStateAction
	{
		[ActionSection("set up")]
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[ActionSection("player properties")]
		
		[Tooltip("The Photon player name")]
		[UIHint(UIHint.Variable)]
		public FsmString name;
		
		[Tooltip("The Photon player ID")]
		[UIHint(UIHint.Variable)]
		public FsmInt ID;
		
		[Tooltip("The Photon player isLocal property")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLocal;
		
		[Tooltip("The Photon player isLocal isMasterClient")]
		[UIHint(UIHint.Variable)]
		public FsmBool isMasterClient;
		
		[ActionSection("player custom properties")]
		
		[Tooltip("Custom Properties you have assigned to this player.")]
		[CompoundArray("player Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmVar[] customPropertiesValues;
		
		[ActionSection("Events")] 
		
		[Tooltip("Send this event if the Owner was found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if no Owner was found.")]
		public FsmEvent failureEvent;
			
		private GameObject go;
		
		private PhotonView _networkView;
		
		private void _getNetworkView()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) 
			{
				return;
			}
			
			_networkView =  go.GetComponent<PhotonView>();
		}
		
		public override void Reset()
		{
			name = null;
			ID = null;
			isLocal = null;
			isMasterClient = null;
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
			
			successEvent = null;
			failureEvent = null;
			
		}

		public override void OnEnter()
		{
			
			_getNetworkView();
			
			bool ok;
			ok = getOwnerProperties();
			
			if (ok)
			{
				Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}
			Finish();
		}

		bool getOwnerProperties()
		{
			if (_networkView==null)
			{
				return false;
			}

			Player _player = _networkView.Owner;
			if (_player==null)
			{
				return false;
			}

			name.Value = _player.NickName;
		//ID.Value   = _player.ID; // TODO:
			isLocal.Value = _player.IsLocal;
			isMasterClient.Value = _player.IsMasterClient;
			
			// get the custom properties
			int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				if (_player.CustomProperties.ContainsKey(key.Value))
				{
					PlayMakerUtils.ApplyValueToFsmVar(this.Fsm,customPropertiesValues[i],_player.CustomProperties[key.Value]);
				}else{
					return false;
				}
				i++;
			}
			
			return true;
		}
	
		
		
	}
}