using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using System.Globalization;

namespace RankingSystem
{

    [RequireComponent(typeof(SocketIOComponent))]
    public class NetworkController : MonoBehaviour
    {
        #region Singleton
        public static NetworkController Instance = null;

        private void Awake()
        {
            Instance = this;
        }
        #endregion

        // Action event called when the server sends a message to the client
        public Action<ServerMessage> OnServerMessage = delegate { };
        public Action<List<PlayerScore>> OnServerPlayersUpdate = delegate { };
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
            _socketComponent.On("send_scores", OnSendPlayersCallback);
        }

        public void SayHi()
        {
            _socketComponent.Emit("ping");
        }

        public void RequestScores()
        {
            Debug.Log("Requesting scores");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("numberPlayers", "10");
            _socketComponent.Emit("request_scores", new JSONObject(data));
        }

        public void PushScore(string playerName, string score)
        {
            Debug.Log("Pushing score");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("name", playerName);
            data.Add("score", score);

            _socketComponent.Emit("push_score", new JSONObject(data));
        }

        #region Callbacks
        private void OnPongCallback(SocketIOEvent args)
        {
            Debug.Log("Pong");
        }

        private void OnEventStatusCallback(SocketIOEvent args)
        {
            ServerMessage serverMessage = JsonUtility.FromJson<ServerMessage>(args.data.ToString());
            OnServerMessage(serverMessage);

            Debug.Log(serverMessage.ToString());
        }

        private void OnSendPlayersCallback(SocketIOEvent args)
        {
            List<PlayerScore> playerScore = new List<PlayerScore>();

            foreach (var i in args.data.GetField("players").list)
            {
                string name = i.GetField("_name").ToString();
                float score = float.Parse(i.GetField("_score").ToString());

                PlayerScore ps = new PlayerScore(name, score);
                playerScore.Add(ps);
            }

            OnServerPlayersUpdate(playerScore);
        }
        #endregion
    }
}