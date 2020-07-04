using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RankingSystem
{
    public struct ServerMessage
    {
        public string status;
        public string message;

        public override string ToString()
        {
            return $"<color=blue>SERVER MESSAGE : {status}, {message}</color>";
        }
    }
}