// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace HutongGames.PlayMaker.Pun2
{
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
    public class PlayMakerPun2CallbacksProxy : MonoBehaviourPunCallbacks
    {

        public static PlayMakerPun2CallbacksProxy Instance;
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
        /// The last Pun 2 callback.
        /// </summary>
        public Pun2Callbacks LastCallback = Pun2Callbacks.Unknown;

        /// <summary>
        /// The last PlayMaker event related to the last Pun2 callback;s
        /// </summary>
        public string LastCallbackEvent = string.Empty;

        /// <summary>
        /// The last disconnection or connection failure cause
        /// </summary>
        public DisconnectCause lastDisconnectCause;


        public Dictionary<string, object> lastCustomAuthenticationResponse;

        public static bool lastCreateRoomFailed = false;
        public short LastCreateRoomFailedReturnCode = 0;
        public string LastCreateRoomFailedMessage = string.Empty;

        public static bool lastJoinRoomFailed = false;
        public short LastJoinRoomFailedReturnCode = 0;
        public string LastJoinRoomFailedMessage = string.Empty;

        public static bool lastJoinRandomRoomFailed = false;
        public short LastJoinRandomRoomFailedReturnCode = 0;
        public string LastJoinRandomRoomFailedMessage = string.Empty;

        public RegionHandler LastRegionHandler;


        public OperationResponse LastWebRpcResponse;

        /// <summary>
        /// The last authentication failed debug message. Message is reseted when authentication is triggered again.
        /// </summary>
        public static string lastAuthenticationDebugMessage = string.Empty;

        /// <summary>
        /// Is True if the last authentication failed.
        /// </summary>
        public static bool lastAuthenticationFailed = false;


        const string DebugLabelPrefix = "<color=navy>PlayMaker Photon proxy: </color>";

        private void Awake()
        {
            Instance = this;
        }
        /// <summary>
        /// Watch connection state
        /// </summary>
        void Update()
        {
            Update_connectionStateWatcher();
        }

        #region connection state watcher

        /// <summary>
        /// Watch connection state and broadcast associated FsmEvent.
        /// </summary>
        private void Update_connectionStateWatcher()
        {

            if (lastConnectionState != PhotonNetwork.NetworkClientState)
            {
                if (debug)
                {
                    Debug.Log(DebugLabelPrefix + "PhotonNetwork.NetworkClientState changed from '" + lastConnectionState + "' to '" + PhotonNetwork.NetworkClientState + "'");
                }

                lastConnectionState = PhotonNetwork.NetworkClientState;
                PlayMakerFSM.BroadcastEvent(PlayMakerPun2LUT.ClientStateEnumEvents[PhotonNetwork.NetworkClientState]);
            }

        }// Update_connectionStateWatcher

        #endregion


        #region Photon Messages Internal Handling

        /// <summary>
        /// The last callback data debug.
        /// Cached to avoid allocations
        /// </summary>
        Dictionary<string, string> _LastCallbackDataDebug = new Dictionary<string, string>();

        /// <summary>
        /// Broadcasts the Last callback.
        /// 
        /// </summary>
        void BroadcastCallback()
        {
            LogEventBroadcasting(LastCallback.ToString(), LastCallbackEvent);

            PlayMakerFSM.BroadcastEvent(LastCallbackEvent);
        }

        /// <summary>
        /// Logs the event broadcasting. Only if Debug is on
        /// </summary>
        /// <param name="punCallback">Pun callback.</param>
        /// <param name="fsmEvent">Fsm event.</param>
        /// <param name="eventData">Event data.</param>
        void LogEventBroadcasting(string punCallback, string fsmEvent, Dictionary<string, string> eventData = null)
        {
            if (debug)
            {
                string _data = string.Empty;
                if (eventData != null)
                {
                    foreach (KeyValuePair<string, string> data in eventData)
                    {
                        _data += "<color=darkblue>" + data.Key + "</color>=<color=<darkblue>" + data.Value + "</color> ";
                    }
                }
                else
                {
                    _data = "No Data associated with this event";
                }

                Debug.Log(DebugLabelPrefix + " Received Callback <color=fuchsia>" + punCallback + "</color> " +
                    "Broadcasting global Event <color=fuchsia>" + fsmEvent + "</color>\n" +
                    _data
                    , this);
            }
        }

        #endregion

        #region Photon Messages

        public override void OnConnected()
        {
            LastCallback = Pun2Callbacks.OnConnected;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }

        public override void OnConnectedToMaster()
        {
            LastCallback = Pun2Callbacks.OnConnectedToMaster;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            LastCallback = Pun2Callbacks.OnDisconnected;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastDisconnectCause = cause;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Cause", cause.ToString());

            BroadcastCallback();
        }

        public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            LastCallback = Pun2Callbacks.OnCustomAuthenticationResponse;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastAuthenticationFailed = false;
            lastCustomAuthenticationResponse = data;

            _LastCallbackDataDebug.Clear();

            foreach (var _d in data)
            {
                _LastCallbackDataDebug.Add(_d.Key, _d.Value.ToString());
            }

            BroadcastCallback();
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
            LastCallback = Pun2Callbacks.OnCustomAuthenticationFailed;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastAuthenticationDebugMessage = debugMessage;
            lastAuthenticationFailed = true;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Message", debugMessage);

            BroadcastCallback();
        }


        public override void OnJoinedLobby()
        {
            LastCallback = Pun2Callbacks.OnJoinedLobby;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }

        public override void OnLeftLobby()
        {
            LastCallback = Pun2Callbacks.OnLeftLobby;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }


        public override void OnCreatedRoom()
        {
            LastCallback = Pun2Callbacks.OnCreatedRoom;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastCreateRoomFailed = false;
            LastCreateRoomFailedMessage = string.Empty;
            LastCreateRoomFailedReturnCode = 0;

            BroadcastCallback();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            LastCallback = Pun2Callbacks.OnCreateRoomFailed;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastCreateRoomFailed = true;
            LastCreateRoomFailedReturnCode = returnCode;
            LastCreateRoomFailedMessage = message;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("ReturnCode", returnCode.ToString());
            _LastCallbackDataDebug.Add("Message", message);

            BroadcastCallback();
        }


        public override void OnJoinedRoom()
        {
            LastCallback = Pun2Callbacks.OnJoinedRoom;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastJoinRoomFailed = false;
            LastJoinRoomFailedReturnCode = 0;
            LastJoinRoomFailedMessage = string.Empty;

            lastJoinRandomRoomFailed = false;
            LastJoinRandomRoomFailedReturnCode = 0;
            LastJoinRandomRoomFailedMessage = string.Empty;

            BroadcastCallback();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            LastCallback = Pun2Callbacks.OnJoinRoomFailed;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastJoinRoomFailed = true;
            LastJoinRoomFailedReturnCode = returnCode;
            LastJoinRoomFailedMessage = message;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("ReturnCode", returnCode.ToString());
            _LastCallbackDataDebug.Add("Message", message);

            BroadcastCallback();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            LastCallback = Pun2Callbacks.OnJoinRandomFailed;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastJoinRandomRoomFailed = true;
            LastJoinRandomRoomFailedReturnCode = returnCode;
            LastJoinRandomRoomFailedMessage = message;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("ReturnCode", returnCode.ToString());
            _LastCallbackDataDebug.Add("Message", message);

            BroadcastCallback();
        }

        public override void OnLeftRoom()
        {
            LastCallback = Pun2Callbacks.OnLeftRoom;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            LastCallback = Pun2Callbacks.OnPlayerEnteredRoom;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastMessagePhotonPlayer = newPlayer;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("New Player", lastMessagePhotonPlayer.ToStringFull());

            BroadcastCallback();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            LastCallback = Pun2Callbacks.OnPlayerLeftRoom;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            lastMessagePhotonPlayer = otherPlayer;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Other Player", lastMessagePhotonPlayer.ToStringFull());

            BroadcastCallback();
        }

        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            base.OnFriendListUpdate(friendList);
        }

        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            base.OnLobbyStatisticsUpdate(lobbyStatistics);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
        }

        public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(target, changedProps);
        }

       

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            LastCallback = Pun2Callbacks.OnRoomPropertiesUpdate;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            LastWebRpcResponse = response;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Response", LastWebRpcResponse.ToStringFull());

            BroadcastCallback();
        }

        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            LastCallback = Pun2Callbacks.OnRegionListReceived;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            LastRegionHandler = regionHandler;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Summary", LastRegionHandler.SummaryToCache);

            BroadcastCallback();
        }

        public override void OnWebRpcResponse(OperationResponse response)
        {
            LastCallback = Pun2Callbacks.OnWebRpcResponse;
            LastCallbackEvent = PlayMakerPun2LUT.CallbacksEvents[LastCallback];

            LastWebRpcResponse = response;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Response", LastWebRpcResponse.ToStringFull());

            BroadcastCallback();

        }



        //void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        //{
        //    Player player = playerAndUpdatedProps[0] as Player;
        //    //Hashtable props = playerAndUpdatedProps[1] as Hashtable;

        //    if (debug)
        //    {
        //        Debug.Log("PLayMaker Photon proxy:OnPhotonPlayerPropertiesChanged:" + player);
        //    }

        //    lastMessagePhotonPlayer = player;

        //    PlayMakerFSM.BroadcastEvent("PHOTON / PLAYER PROPERTIES CHANGED");
        //}




        #endregion


        #region OwnerShip Request

        //public void OnOwnershipRequest(object[] viewAndPlayer)
        //{
        //    PhotonView view = viewAndPlayer[0] as PhotonView;
        //    Player requestingPlayer = viewAndPlayer[1] as Player;

        //    if (debug)
        //    {
        //        Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");
        //    }

        //    view.TransferOwnership(requestingPlayer.ActorNumber);
        //}

        #endregion

        #region utils

        // this is a fix to the regular BroadCast that for some reason delivers events too late when player joins a room

        public static void BroadCastToAll(string fsmEvent)
        {

            var list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);

            foreach (PlayMakerFSM fsm in list)
            {

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
}