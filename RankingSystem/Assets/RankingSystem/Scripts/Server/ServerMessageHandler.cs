using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    /// <summary>
    /// Class that handles server message (eventStatus)
    /// </summary>
    public class ServerMessageHandler : MonoBehaviour
    {
        public Text resultStatusField;
        public Text serverMessageField;

        public Color successColor;
        public Color errorColor;

        private void Start()
        {
            NetworkController.Instance.OnServerMessage += DisplayServerMessage;
        }

        private void DisplayServerMessage(ServerMessage serverMessage)
        {
            Color colorToDisplay = serverMessage.status == "success" ? successColor : errorColor;

            resultStatusField.text = serverMessage.status;
            serverMessageField.text = serverMessage.message;

            resultStatusField.color = colorToDisplay;
            serverMessageField.color = colorToDisplay;
        }
    }

}