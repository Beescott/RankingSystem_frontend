using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{

    public class UserInputsController : MonoBehaviour
    {
        [SerializeField] private InputField userID;
        [SerializeField] private InputField score;
        [SerializeField] private Button pushScoreButton;

        private void Start()
        {
            pushScoreButton.onClick.AddListener(() =>
            {
                NetworkController.Instance.PushScore(userID.text, score.text);
            });
        }
    }

}