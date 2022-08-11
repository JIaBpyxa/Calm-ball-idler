namespace Vorval.CalmBall.Service
{
    public class SaveService
    {
        private const string ScoreKey = "Score";

        public static void SaveScore(int score)
        {
            SecurePlayerPrefs.SetInt(ScoreKey, score);
        }

        public static int GetScore()
        {
            return SecurePlayerPrefs.GetInt(ScoreKey, 0);
        }
    }
}