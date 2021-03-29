using UnityEditor;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Editor
{
	[InitializeOnLoad]
	public class PlayMakerPhotonEditorSetup
	{
		static bool _eventAdded;

	
		static PlayMakerPhotonEditorSetup()
		{
#if PLAYMAKER_1_9_OR_NEWER
			FsmEditorSettings.ShowNetworkSync = true;
#endif

			if (!Application.isPlaying)
			{
				SanitizeGlobalEventSetup();
			}

		}


		public static void SanitizeGlobalEventSetup()
		{
			// add global events if needed.

			_eventAdded = FsmEvent.IsEventGlobal(PlayMakerPunLUT.PhotonEvents[0]);

			Debug.Log("SanitizeGlobalEventSetup init?");

			if (!_eventAdded)
			{
				Debug.Log("Creating Photon Events");
				foreach (string _event in PlayMakerPunLUT.PhotonEvents)
				{
					_eventAdded = PlayMakerUtils.CreateIfNeededGlobalEvent(_event);
				}
			}
		}
	}
}