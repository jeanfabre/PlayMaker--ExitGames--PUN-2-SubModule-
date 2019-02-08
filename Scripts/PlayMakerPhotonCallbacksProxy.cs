// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Part of "PlayMaker Photon Callbacks Proxy" prefab.
/// This behavior implements *All* messages from Photon, and broadcast associated global events.
/// note: the instantiate call is featured in the PlayMakerPhotonGameObjectProxy component
/// 
/// The playmaker events corresponding to each Photon messages are declared in the Fsm named "Photon messages interface" in the "PlayMaker Photon Proxy" prefab.
/// 
/// Example: the photon message OnPhotonPlayerConnected (PhotonPlayer player) is translated as a global event "PHOTON / PHOTON PLAYER CONNECTED"
/// the PhotonPlayer passed in these messages is stored in lastMessagePhotonPlayer and can be retrieved using the action "PhotonViewGetLastMessagePLayerProperties"
/// 
/// This behavior also watch the connection state and broadcast associated global events.
/// example: PhotonNetwork.connectionState.Connecting is translated as a global event named "PHOTON / STATE : CONNECTING"
public class PlayMakerPhotonCallbacksProxy : MonoBehaviourPunCallbacks
{

	/// <summary>
	/// output in the console activities of the various elements.
	/// TODO: should be set to false for release
	/// </summary>
	public bool debug = true;
	
	/// <summary>
	/// The last state of the connection. This is used to watch connection state changes and broadcast related Events.
	/// So you can receive an event when the connection is "disconnecting" or "Connecting", something not available as messages.
	/// TOREVIEW: this is a goodie, but very useful within playmaker environment, it's easier and more adequate then watching for the connection state within a fsm.
	/// </summary>
	private ClientState lastConnectionState;	
	
	/// <summary>
	/// The photon player sent with a message like OnPhotonPlayerConnected, OnPhotonPlayerDisconnected or OnMasterClientSwitched
	/// Only the last instance is stored. Use PhotonNetworkGetMessagePlayerProperties Action to retrieve it within PlayMaker.
	/// This also store the player from the photonMessageinfo of the RPC calls implemented in this script.
	/// </summary>
	public Player lastMessagePhotonPlayer;
	
	/// <summary>
	/// The last disconnection or connection failure cause
	/// </summary>
	public DisconnectCause lastDisconnectCause;


	/// <summary>
	/// The last authentication failed debug message. Message is reseted when authentication is triggered again.
	/// </summary>
	public static string lastAuthenticationDebugMessage = string.Empty;
	
	/// <summary>
	/// Is True if the last authentication failed.
	/// </summary>
	public static bool lastAuthenticationFailed = false;
	
	
	/// <summary>
	/// Watch connection state
	/// </summary>
	void Update ()
	{		
		Update_connectionStateWatcher ();
	}
	
	#region connection state watcher
	
	/// <summary>
	/// Watch connection state and broadcast associated FsmEvent.
	/// </summary>
	private void Update_connectionStateWatcher ()
	{

		if (lastConnectionState != PhotonNetwork.NetworkClientState) {
			if (debug) {
				Debug.Log ("PhotonNetwork.NetworkClientState changed from '" + lastConnectionState + "' to '" + PhotonNetwork.NetworkClientState + "'");
			}
			
			lastConnectionState = PhotonNetwork.NetworkClientState;
            PlayMakerFSM.BroadcastEvent(PlayMakerPhotonLUT.ClientStateEnumEvents[PhotonNetwork.NetworkClientState]);
        }

	}// Update_connectionStateWatcher

    #endregion

    #region Photon Messages

    public override void OnConnected()
    {
        if (debug)
        {
            Debug.Log("PlayMaker Photon proxy: OnConnected");
        }

        PlayMakerFSM.BroadcastEvent(
            PlayMakerPhotonLUT.CallbacksEvents[PlayMakerPhotonLUT.PhotonCallbacks.OnConnected]
            );
    }

    /// <summary>
    /// compose this message to dispatch the associated global Fsm Event. 
    /// This method is called when Custom Authentication is setup for your app but fails for any reasons.
    /// </summary>
    /// <remarks>
    /// Unless you setup a custom authentication service for your app (in the Dashboard), this won't be called.
    /// If authentication is successful, this method is not called but OnJoinedLobby, OnConnectedToMaster and the 
    /// others will be called.
    /// </remarks>
    /// <param name="debugMessage"></param>
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        lastAuthenticationDebugMessage = debugMessage;
        lastAuthenticationFailed = true;
		
		if (debug) {
			Debug.Log ("PlayMaker Photon proxy: OnCustomAuthenticationFailed: " + debugMessage);
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / ON CUSTOM AUTHENTICATION FAILED");
		
    }

    /// <summary>
    /// compose this message to dispatch the associated global Fsm Event. 
    /// the player properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
    /// </summary>
    /// <param name='player'>
    /// Player.
    /// </param>
    void OnPhotonPlayerConnected (Player player)
	{
		if (debug) {
			Debug.Log ("PlayMaker Photon proxy: OnPhotonPlayerConnected: " + player);
		}
		
		lastMessagePhotonPlayer = player;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / ON PHOTON PLAYER CONNECTED");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// the player properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='player'>
	/// Player.
	/// </param>
	void OnPhotonPlayerDisconnected (Player player)
	{
		if (debug) {
			Debug.Log ("PlayMaker Photon proxy OnPlayerDisconneced: " + player);
		}
		
		lastMessagePhotonPlayer = player;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / ON PHOTON PLAYER DISCONNECTED");
	}
	

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
    public override void OnJoinedRoom ()
	{
		if (debug) {
			Debug.Log ("PlayMaker Photon proxy: OnJoinedRoom: ");
		}
		
	 	BroadCastToAll("PHOTON / ON JOINED ROOM");
			
	}

    /// <summary>
    /// compose this message to dispatch the associated global Fsm Event. 
    /// </summary>
    public override void OnCreatedRoom ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnCreatedRoom: ");
		}
		PlayMakerFSM.BroadcastEvent ("PHOTON / ON CREATED ROOM");
	}

    /// <summary>
    /// compose this message to dispatch the associated global Fsm Event. 
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnCreateRoomFailed");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / ON CREATE ROOM FAILED");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonJoinRoomFailed ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonJoinRoomFailed");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON JOIN ROOM FAILED");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonRandomJoinFailed ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonRandomJoinFailed");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / PHOTON RANDOM JOIN FAILED");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnLeftRoom ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnLeftRoom (local)");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / LEFT ROOM");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnReceivedRoomList ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnReceivedRoomList");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / RECEIVED ROOM LIST");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnReceivedRoomListUpdate ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnReceivedRoomListUpdate");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / RECEIVED ROOM LIST UPDATE");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// the player properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
	/// </summary>
	/// <param name='player'>
	/// Player.
	/// </param>
	void OnMasterClientSwitched (Player player)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnMasterClientSwitched: " + player);
		}
		
		lastMessagePhotonPlayer = player;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / MASTER CLIENT SWITCHED");    
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnConnectedToPhoton ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnConnectedToPhoton");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / CONNECTED TO PHOTON");
	}

	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnConnectedToMaster ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnConnectedToMaster");
		}
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / CONNECTED TO MASTER");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnDisconnectedFromPhoton ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnDisconnectedFromPhoton");
		}
        
		PlayMakerFSM.BroadcastEvent ("PHOTON / DISCONNECTED FROM PHOTON");
	}

    /// <summary>
    /// compose this message to dispatch the associated global Fsm Event. 
    /// </summary>
    /// <param name="cause">Cause.</param>
    void OnConnectionFail (DisconnectCause cause)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnConnectionFail " + cause);
		}
		
		lastDisconnectCause = cause;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / CONNECTION FAIL");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
    /// <param name="cause">Cause.</param>
	void OnFailedToConnectToPhoton (DisconnectCause cause)
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy: OnFailedToConnectToPhoton " + cause);
		}
		
		lastDisconnectCause = cause;
		
		PlayMakerFSM.BroadcastEvent ("PHOTON / FAILED TO CONNECT TO PHOTON");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnJoinedLobby ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnJoinedLobby");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / JOINED LOBBY");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnLeftLobby ()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnLeftLobby");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / LEFT LOBBY");
	}
	
	/// <summary>
	/// compose this message to dispatch the associated global Fsm Event. 
	/// </summary>
	void OnPhotonMaxCccuReached()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonMaxCccuReached");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / MAX CCCU REACHED");
	}
	
	
	void OnPhotonCustomRoomPropertiesChanged()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonCustomRoomPropertiesChanged");
		}
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / CUSTOM ROOM PROPERTIES CHANGED");
	}
	

	void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		Player player = playerAndUpdatedProps[0] as Player;
		//Hashtable props = playerAndUpdatedProps[1] as Hashtable;
			
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnPhotonPlayerPropertiesChanged:"+ player);
		}
		
		lastMessagePhotonPlayer = player;
       
		PlayMakerFSM.BroadcastEvent ("PHOTON / PLAYER PROPERTIES CHANGED");
	}

	/// <summary>
	/// called when the list PhotonNetwork.Friends is refreshed. Request initiated with PhotonNetworkFindFriends action.
	/// </summary>
	public void OnUpdatedFriendList()
	{
		if (debug) {
			Debug.Log ("PLayMaker Photon proxy:OnUpdatedFriendList ");
		}

		PlayMakerFSM.BroadcastEvent ("PHOTON / FRIEND LIST UPDATED");
	}


	#endregion
	

	#region OwnerShip Request
	
	public void OnOwnershipRequest(object[] viewAndPlayer)
	{
		PhotonView view = viewAndPlayer[0] as PhotonView;
		Player requestingPlayer = viewAndPlayer[1] as Player;

		if (debug) {
			Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");
		}

		view.TransferOwnership(requestingPlayer.ActorNumber);
	}
	
	#endregion

	#region utils
	
	// this is a fix to the regular BroadCast that for some reason delivers events too late when player joins a room
	
	public static void BroadCastToAll(string fsmEvent)
	{
	
		var list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);
			
		foreach (PlayMakerFSM fsm in list){
			
			//if (true)
			//{
				fsm.SendEvent(fsmEvent); // fine
			//}else{	
			//	fsm.Fsm.ProcessEvent(FsmEvent.GetFsmEvent(fsmEvent)); // too late. This is the PlayMakerFsm.BroadcastEvent() way as far as I can tell.
			//}
		}
		
	}

		
	#endregion
}