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

			if (!Application.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				SanitizeGlobalEventSetup();
			}

		}


		public static void SanitizeGlobalEventSetup()
		{
		
			Debug.Log("PlayMaker Photon : Sanitizing Global Events");

				bool _eventAdded = true;
//				Debug.Log("Creating Photon Events");
				foreach (string _event in PlayMakerPunLUT.PhotonEvents)
				{
					if (PlayMakerUtils.CreateIfNeededGlobalEvent(_event))
					{
						_eventAdded = true;
					}
				}
				
			if (_eventAdded)	FsmEditor.SaveGlobals();
			
		}
	}
}