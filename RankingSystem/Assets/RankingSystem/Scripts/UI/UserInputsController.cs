using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{

    public class UserInputsController : MonoBehaviour
    {
        public InputField userID;
        public InputField score;
        public Button pushScoreButton;

        private void Start()
        {
            pushScoreButton.onClick.AddListener(() =>
            {
                NetworkController.Instance.PushScore(userID.text, score.text);
            });
        }
    }

}