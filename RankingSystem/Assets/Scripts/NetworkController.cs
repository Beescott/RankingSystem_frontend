using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

[RequireComponent(typeof(SocketIOComponent))]
public class NetworkController : MonoBehaviour
{
    #region Singleton
    private static NetworkController Instance = null;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Action event called when the server sends a message to the client
    public Action<string> OnServerMessage = delegate { };
    private SocketIOComponent _socketComponent;

    private void Start()
    {
        _socketComponent = GetComponent<SocketIOComponent>();
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        _socketComponent.Connect();

        _socketComponent.On("pong", OnPongCallback);
        _socketComponent.On("event_status", OnEventStatusCallback);
    }

    public void SayHi()
    {
        _socketComponent.Emit("ping");
    }

    #region Callbacks
    private void OnPongCallback(SocketIOEvent args)
    {
        Debug.Log("Pong");
    }

    private void OnEventStatusCallback(SocketIOEvent args)
    {
        ServerMessage serverMessage = JsonUtility.FromJson<ServerMessage>(args.data.ToString());
        
        // The server only sends a message when the status is not successfull
        if (serverMessage.status != "success")
        {
            OnServerMessage(serverMessage.message);
        }
    }
    #endregion
}