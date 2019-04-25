// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the View ID Photon network View is controlled by a GameObject.\n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewGetViewID : PunComponentActionBase<PhotonView>
    {
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
        [RequiredField]
		[Tooltip("The PhotonView ID as int")]
		public FsmInt viewID;

        [Tooltip("Send this event if there was no PhotonView found on the GamoObject")]
        public FsmEvent failure;

        public override void Reset()
		{
			gameObject = null;
            viewID = null;
            failure = null;
		}

        public override void OnEnter()
        {
            ExecuteAction();

            Finish();
        }

        void ExecuteAction()
        {
            if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
            {
                if (failure != null) Fsm.Event(failure);
                return;
            }

            viewID.Value = this.photonView.ViewID;
			
			Finish();
		}

	}
}