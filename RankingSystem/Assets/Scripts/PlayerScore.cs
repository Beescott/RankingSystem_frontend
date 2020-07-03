using System;
namespace RankingSystem
{
    [Serializable]
    public struct PlayerScore
    {
        public string name;
        public float score;

        public PlayerScore(string n, float s)
        {
            name = n;
            score = s;
        }
    }
}
