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

        #region Action event
        public Action<ServerMessage> OnServerMessage = delegate { };
        public Action<List<PlayerScore>> OnServerPlayersUpdate = delegate { };
        public Action<PlayerScore> OnServerPlayerRequest = delegate { };
        public Action OnNullServerPlayerRequest = delegate { };
        #endregion

        #region Private variables
        private SocketIOComponent _socketComponent;
        #endregion

        private void Start()
        {
            _socketComponent = GetComponent<SocketIOComponent>();
            ConnectToServer();
        }

        /// <summary>
        /// Connect to the server and listen for all the needed callbacks
        /// </summary>
        private void ConnectToServer()
        {
            _socketComponent.Connect();

            _socketComponent.On("pong", OnPongCallback);
            _socketComponent.On("event_status", OnEventStatusCallback);
            _socketComponent.On("send_scores", OnSendPlayersCallback);
            _socketComponent.On("player_removed", OnPlayerRemoved);
            _socketComponent.On("send_player_score", OnSendPlayerScoreCallback);
        }

        /// <summary>
        /// Ping
        /// </summary>
        public void SayHi()
        {
            _socketComponent.Emit("ping");
        }

        /// <summary>
        /// Request the PlayerScore list
        /// </summary>
        public void RequestScores()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            int numberOfWantedPlayers = RankingSystemController.Instance.requestAllPlayers ? 0 : RankingSystemController.Instance.wantedNumberOfPlayers;
            data.Add("numberPlayers", numberOfWantedPlayers.ToString());
            _socketComponent.Emit("request_scores", new JSONObject(data));
        }

        /// <summary>
        /// Push a score into the server or update it if the player already exists
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="score"></param>
        public void PushScore(string playerName, string score)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("name", playerName);
            data.Add("score", score);

            _socketComponent.Emit("push_score", new JSONObject(data));
        }

        /// <summary>
        /// Remove a player from the database
        /// </summary>
        /// <param name="playerName"></param>
        public void RemovePlayer(string playerName)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("playerName", playerName);
            _socketComponent.Emit("remove_player", new JSONObject(data));
        }

        /// <summary>
        /// Request the score of a given player
        /// </summary>
        /// <param name="playerName"></param>
        public void RequestPlayerScore(string playerName)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("playerName", playerName);
            _socketComponent.Emit("request_player_score", new JSONObject(data));
        }

        #region Callbacks
        /// <summary>
        /// Pong
        /// </summary>
        /// <param name="args"></param>
        private void OnPongCallback(SocketIOEvent args)
        {
            Debug.Log("Pong");
        }

        /// <summary>
        /// Send the server message to all listeners of OnServerMessage
        /// </summary>
        /// <param name="args"></param>
        private void OnEventStatusCallback(SocketIOEvent args)
        {
            ServerMessage serverMessage = JsonUtility.FromJson<ServerMessage>(args.data.ToString());
            OnServerMessage(serverMessage);
        }

        /// <summary>
        /// Send list of player to all listeners of OnServerPlayersUpdate
        /// </summary>
        /// <param name="args"></param>
        private void OnSendPlayersCallback(SocketIOEvent args)
        {
            List<PlayerScore> playerScore = new List<PlayerScore>();
            
            foreach (var i in args.data.GetField("players").list)
            {
                string name = i.GetField("_name").ToString();
                name = name.Replace("\"", "");

                float score = float.Parse(i.GetField("_score").ToString());

                PlayerScore ps = new PlayerScore(name, score);
                playerScore.Add(ps);
            }

            OnServerPlayersUpdate(playerScore);
        }

        /// <summary>
        /// Update the list when removing a player
        /// </summary>
        /// <param name="args"></param>
        private void OnPlayerRemoved(SocketIOEvent args)
        {
            RequestScores();
        }

        /// <summary>
        /// Send a player to all listeners of OnServerPlayerRequest
        /// </summary>
        /// <param name="args"></param>
        private void OnSendPlayerScoreCallback(SocketIOEvent args)
        {
            if (args.data.ToString() == "{}")
            {
                OnNullServerPlayerRequest();
                return;
            }

            string name = args.data.GetField("player").GetField("_name").ToString();
            float score = float.Parse(args.data.GetField("player").GetField("_score").ToString());
            PlayerScore requestedPlayer = new PlayerScore(name, score);

            OnServerPlayerRequest(requestedPlayer);
        }
        #endregion
    }
}