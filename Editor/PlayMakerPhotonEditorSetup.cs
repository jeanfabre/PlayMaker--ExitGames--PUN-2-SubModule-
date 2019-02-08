using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;

[InitializeOnLoad]
public class PlayMakerPhotonEditorSetup
{

	static bool _eventAdded;


	static PlayMakerPhotonEditorSetup()
	{
		#if PLAYMAKER_1_9_OR_NEWER
			FsmEditorSettings.ShowNetworkSync = true;
		#endif

		SanitizeGlobalEventSetup ();
	}


	public static void SanitizeGlobalEventSetup()
	{
		// add global events if needed.

		_eventAdded = FsmEvent.IsEventGlobal(PlayMakerPhotonLUT.PhotonEvents[0]);

	

			Debug.Log ("SanitizeGlobalEventSetup init?");

		if (!_eventAdded)
		{
			Debug.Log ("Creating Photon Events");
			foreach (string _event in PlayMakerPhotonLUT.PhotonEvents)
			{
				_eventAdded = PlayMakerUtils.CreateIfNeededGlobalEvent (_event);
			}
		}
	}

}