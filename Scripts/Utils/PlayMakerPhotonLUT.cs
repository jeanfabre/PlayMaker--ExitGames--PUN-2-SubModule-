using System.Collections.Generic;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2
{

    public enum Pun2Callbacks
    {
        OnConnected                             = 1,
        OnConnectedToMaster                     = 2,
        OnDisconnected                          = 3,
        OnCustomAuthenticationResponse          = 4,
        OnCustomAuthenticationFailed            = 5,
        OnJoinedLobby                           = 6,
        OnLeftLobby                             = 7,
        OnCreatedRoom                           = 8,
        OnCreateRoomFailed                      = 9,
        OnJoinedRoom                            = 10,
        OnJoinRoomFailed                        = 11,
        OnJoinRandomFailed                      = 12,
        OnLeftRoom                              = 13,
        OnPlayerEnteredRoom                     = 14,
        OnPlayerLeftRoom                        = 15,
        OnFriendListUpdate                      = 16,
        OnRoomListUpdate                        = 17,
        OnRoomPropertiesUpdate                  = 18,
        OnPlayerPropertiesUpdate                = 19,
        OnLobbyStatisticsUpdate                 = 20,
        OnMasterClientSwitched                  = 21,
        OnRegionListReceived                    = 22,
        OnWebRpcResponse                        = 23,
        Unknown                                 = 0
    }

    public enum Pun2CallbacksWithData
    {
        OnDisconnected                          = 3,
        OnCustomAuthenticationResponse          = 4,
        OnCustomAuthenticationFailed            = 5,
        OnCreateRoomFailed                      = 8,
        OnJoinRoomFailed                        = 11,
        OnJoinRandomFailed                      = 12,
        OnPlayerEnteredRoom                     = 14,
        OnPlayerLeftRoom                        = 15

    }

    public class PlayMakerPun2LUT
    {
        public static readonly Dictionary<Pun2Callbacks, string> CallbacksEvents = new Dictionary<Pun2Callbacks, string>()
    {
        {Pun2Callbacks.OnConnected,                     "PHOTON / ON CONNECTED"},
        {Pun2Callbacks.OnConnectedToMaster,             "PHOTON / ON CONNECTED TO MASTER"},
        {Pun2Callbacks.OnDisconnected,                  "PHOTON / ON DISCONNECTED"},
        {Pun2Callbacks.OnCustomAuthenticationResponse,  "PHOTON / ON CUSTOM AUTHENTICATION RESPONSE"},
        {Pun2Callbacks.OnCustomAuthenticationFailed,    "PHOTON / ON CUSTOM AUTHENTICATION FAILED"},
        {Pun2Callbacks.OnJoinedLobby,                   "PHOTON / ON JOINED LOBBY"},
        {Pun2Callbacks.OnLeftLobby,                     "PHOTON / ON LEFT LOBBY"},
        {Pun2Callbacks.OnCreatedRoom,                   "PHOTON / ON CREATED ROOM"},
        {Pun2Callbacks.OnCreateRoomFailed,              "PHOTON / ON CREATE ROOM FAILED "},
        {Pun2Callbacks.OnJoinedRoom,                    "PHOTON / ON JOINED ROOM"},
        {Pun2Callbacks.OnJoinRoomFailed,                "PHOTON / ON JOINED ROOM FAILED"},
        {Pun2Callbacks.OnJoinRandomFailed,              "PHOTON / ON JOIN RANDOM ROOM FAILED"},
        {Pun2Callbacks.OnLeftRoom,                      "PHOTON / ON LEFT ROOM"},
        {Pun2Callbacks.OnPlayerEnteredRoom,             "PHOTON / ON PLAYER ENTERED ROOM"},
        {Pun2Callbacks.OnPlayerLeftRoom,                "PHOTON / ON PLAYER LEFT ROOM"}
    };


        public static readonly Dictionary<ClientState, string> ClientStateEnumEvents = new Dictionary<ClientState, string>()
    {
        {ClientState.Authenticated,                     "PHOTON / CLIENT STATE / AUTHENTICATED"},
        {ClientState.Authenticating,                    "PHOTON / CLIENT STATE / AUTHENTICATING"},
        {ClientState.ConnectedToGameserver,             "PHOTON / CLIENT STATE / CONNECTED TO GAMESERVER"},
        {ClientState.ConnectedToMasterserver,           "PHOTON / CLIENT STATE / CONNECTED TO MASTERSERVER"},
        {ClientState.ConnectedToNameServer,             "PHOTON / CLIENT STATE / CONNECTED TO NAMESERVER"},
        {ClientState.ConnectingToGameserver,            "PHOTON / CLIENT STATE / CONNECTING TO GAMESERVER"},
        {ClientState.ConnectingToNameServer,            "PHOTON / CLIENT STATE / CONNECTING TO NAMESERVER"},
        {ClientState.ConnectingToMasterserver,          "PHOTON / CLIENT STATE / CONNECTING TO MASTERSERVER"},
        {ClientState.Disconnected,                      "PHOTON / CLIENT STATE / DISCONNECTED"},
        {ClientState.Disconnecting,                     "PHOTON / CLIENT STATE / DISCONNECTING"},
        {ClientState.DisconnectingFromGameserver,       "PHOTON / CLIENT STATE / DISCONNECTING FROM GAMESERVER"},
        {ClientState.DisconnectingFromMasterserver,     "PHOTON / CLIENT STATE / DISCONNECTING FROM MASTERSERVER"},
        {ClientState.DisconnectingFromNameServer,       "PHOTON / CLIENT STATE / DISCONNECTING FROM NAMESERVER"},
        {ClientState.Joined,                            "PHOTON / CLIENT STATE / JOINED"},
        {ClientState.JoinedLobby,                       "PHOTON / CLIENT STATE / JOINED LOBBY"},
        {ClientState.Joining,                           "PHOTON / CLIENT STATE / JOINING"},
        {ClientState.JoiningLobby,                      "PHOTON / CLIENT STATE / JOINING LOBBY"},
        {ClientState.Leaving,                           "PHOTON / CLIENT STATE / LEAVING"},
        {ClientState.PeerCreated,                       "PHOTON / CLIENT STATE / PEER CREATED"}
    };


        static List<string> _photonEvents;

        public static List<string> PhotonEvents
        {
            get
            {
                if (_photonEvents == null)
                {
                    _photonEvents = new List<string>();
                    _photonEvents.AddRange(ClientStateEnumEvents.Values);
                    _photonEvents.AddRange(CallbacksEvents.Values);

                }

                return _photonEvents;
            }
        }

    }
}