using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{

    public class UserInputsController : MonoBehaviour
    {
        [SerializeField] private InputField userID = null;
        [SerializeField] private InputField score = null;
        [SerializeField] private Button pushScoreButton = null;

        private void Start()
        {
            pushScoreButton.onClick.AddListener(() =>
            {
                NetworkController.Instance.PushScore(userID.text, score.text);
            });
        }
    }

}