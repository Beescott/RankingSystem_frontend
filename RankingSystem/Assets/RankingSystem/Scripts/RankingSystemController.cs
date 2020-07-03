using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;

namespace RankingSystem
{
    public enum RankingSystemStyle
    {
        Ascending,
        Descending
    }

    [RequireComponent(typeof(SocketIOComponent))]
    [RequireComponent(typeof(NetworkController))]
    public class RankingSystemController : MonoBehaviour
    {
        #region Singleton
        public static RankingSystemController Instance = null;
        #endregion

        #region Public variables
        public string websocketServerURL;
        public string lastEventStatus;

        public Transform targetCanvas;
        public Font fontToUse;

        public List<Image> primaryColorUsers;
        public List<Image> secondaryColorUsers;

        public int wantedNumberOfPlayers;

        [Range(0, 10)]
        public int floatPrecision;

        public RankingSystemStyle systemStyle;

        public GameObject rankElementPrefab;
        #endregion

        #region Private variables
        private NetworkController _networkController;
        private SocketIOComponent _socketComponent;
        private Text[] _allTextInTarget;

        private List<PlayerScore> _playerScores;
        #endregion

        private void Awake()
        {
            Instance = this;

            _networkController = GetComponent<NetworkController>();
            _socketComponent = GetComponent<SocketIOComponent>();

            _socketComponent.url = websocketServerURL;
            _socketComponent.autoConnect = false;

            _playerScores = new List<PlayerScore>();
        }

        private void Start()
        {

        }

        public void ChangeFont()
        {
            _allTextInTarget = targetCanvas.GetComponentsInChildren<Text>(true);
            if (fontToUse != null)
            {
                foreach (Text text in _allTextInTarget)
                {
                    text.font = fontToUse;
                }
            }
        }

        public List<PlayerScore> GetPlayerScoreList()
        {
            if(systemStyle == RankingSystemStyle.Ascending)
                return _playerScores;
            
            List<PlayerScore> tempList = new List<PlayerScore>(_playerScores);
            tempList.Reverse();

            return tempList;
        }

        private void UpdatePlayerScoreList(List<PlayerScore> ps)
        {
            _playerScores = ps;
        }
    }
}
