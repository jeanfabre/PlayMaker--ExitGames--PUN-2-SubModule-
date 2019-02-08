using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayMakerPhotonLUT
{
    public static readonly Dictionary<ClientState, string> ClientStateEnumEvents = new Dictionary<ClientState, string>()
    {
        {ClientState.Authenticated,                 "PHOTON/CLIENT STATE/AUTHENTICATED"},
        {ClientState.Authenticating,                "PHOTON/CLIENT STATE/AUTHENTICATING"},
        {ClientState.ConnectedToGameserver,         "PHOTON/CLIENT STATE/CONNECTED TO GAMESERVER"},
        {ClientState.ConnectedToMasterserver,       "PHOTON/CLIENT STATE/CONNECTED TO MASTERSERVER"},
        {ClientState.ConnectedToNameServer,         "PHOTON/CLIENT STATE/CONNECTED TO NAMESERVER"},
        {ClientState.ConnectingToGameserver,        "PHOTON/CLIENT STATE/CONNECTING TO GAMESERVER"},
        {ClientState.ConnectingToNameServer,        "PHOTON/CLIENT STATE/CONNECTING TO NAMESERVER"},
        {ClientState.ConnectingToMasterserver,      "PHOTON/CLIENT STATE/CONNECTING TO MASTERSERVER"},
        {ClientState.Disconnected,                  "PHOTON/CLIENT STATE/DISCONNECTED"},
        {ClientState.Disconnecting,                 "PHOTON/CLIENT STATE/DISCONNECTING"},
        {ClientState.DisconnectingFromGameserver,   "PHOTON/CLIENT STATE/DISCONNECTING FROM GAMESERVER"},
        {ClientState.DisconnectingFromMasterserver, "PHOTON/CLIENT STATE/DISCONNECTING FROM MASTERSERVER"},
        {ClientState.DisconnectingFromNameServer,   "PHOTON/CLIENT STATE/DISCONNECTING FROM NAMESERVER"},
        {ClientState.Joined,                        "PHOTON/CLIENT STATE/JOINED"},
        {ClientState.JoinedLobby,                   "PHOTON/CLIENT STATE/JOINED LOBBY"},
        {ClientState.Joining,                       "PHOTON/CLIENT STATE/JOINING"},
        {ClientState.JoiningLobby,                  "PHOTON/CLIENT STATE/JOINING LOBBY"},
        {ClientState.Leaving,                       "PHOTON/CLIENT STATE/LEAVING"},
        {ClientState.PeerCreated,                   "PHOTON/CLIENT STATE/PEER CREATED"}
    };

    public enum PhotonCallbacks
    {
        OnConnected

    }

    public static readonly Dictionary<PhotonCallbacks, string> CallbacksEvents = new Dictionary<PhotonCallbacks, string>()
    {
        {PhotonCallbacks.OnConnected,               "PHOTON/ON CONNECTED"}
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
