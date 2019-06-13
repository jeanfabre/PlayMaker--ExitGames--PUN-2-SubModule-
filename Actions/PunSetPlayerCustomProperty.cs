// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Updates and synchronizes the named custom property of the local player. New properties are added, existing values are updated. CustomProperties can be set before entering a room and will be synced as well.")]
	[HelpUrl("")]
	public class PunSetPlayerCustomProperty : FsmStateAction
	{
		[Tooltip("The Custom Property to set or update")]
		public FsmString customPropertyKey;
		
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar customPropertyValue;

		public override void Reset()
		{
			customPropertyKey = "My Property";
			customPropertyValue = null;
		}
		
		public override void OnEnter()
		{
			SetPlayerProperty();
			
			Finish();
		}
		
		void SetPlayerProperty()
		{
			if (customPropertyValue==null)
			{
				LogError("customPropertyValue is null ");
				return;
			}
			
			ExitGames.Client.Photon.Hashtable _prop = new ExitGames.Client.Photon.Hashtable();
			//Log(" set key "+customPropertyKey.Value+"="+ PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertyValue));
			
			_prop[customPropertyKey.Value] = PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertyValue);
			PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
		}

	}
}