// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines Authentication values to use for connection ( using PhotonNetworkConnectUsingSettings or PhotonNetworkConnectManually).\n" +
		"Failed Custom Authentication will fire a global Photon event 'CUSTOM AUTHENTICATION FAILED' event.")]
	[HelpUrl("")]
	public class PunSetAuthenticationValues : FsmStateAction
	{
		[Tooltip("The type of custom authentication provider that should be used. Set to 'None' to turn off.")]
		[ObjectType(typeof(CustomAuthenticationType))]
		public FsmEnum authenticationType;

		[Tooltip("Name or other end-user ID used in custom authentication service.")]
		[RequiredField]
		public FsmString authName;
		
		[Tooltip("Token provided by authentication service to be used on initial 'login' to Photon.")]
		[RequiredField]
		public FsmString authToken;
		
		[Tooltip("Sets the data to be passed-on to the auth service via POST. Empty string will set AuthPostData to null.")]
		public FsmString authPostData;
		
		public override void Reset()
		{
			authenticationType = CustomAuthenticationType.Custom;
			authName = null;
			authToken = null;
			authPostData = new FsmString(){UseVariable=true};
		}

		public override void OnEnter()
		{
			ExecuteAction();

			Finish();
		}

		void ExecuteAction()
		{
			PhotonNetwork.AuthValues = new AuthenticationValues();
			
			PhotonNetwork.AuthValues.AuthType = (CustomAuthenticationType)authenticationType.Value;

			PhotonNetwork.AuthValues.AddAuthParameter("username", authName.Value);
			PhotonNetwork.AuthValues.AddAuthParameter("token", authToken.Value);

			PhotonNetwork.AuthValues.SetAuthPostData(authPostData.Value);
		}

	}
}