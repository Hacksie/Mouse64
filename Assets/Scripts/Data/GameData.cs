using UnityEngine;

namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        [Header("Save Properties")]
        [SerializeField] public bool newGame = true;
        [SerializeField] public string gameVersion = "1.0";
        [SerializeField] public string saveName = "20200811 2153";
        [SerializeField] public int gameSlot = 0;
        [SerializeField] public int health = 100;
        [SerializeField] public float energy = 100;
        [SerializeField] public float maxEnergy = 100;
        [SerializeField] public int bullets = 6;
        [SerializeField] public int maxBullets = 6;
        [SerializeField] public float timer = 64;
        [SerializeField] public int alert = 0;
        [SerializeField] public int score = 0;
        [SerializeField] public int currentLevelIndex = 0;
        [SerializeField] public Level currentLevel;
        [SerializeField] public int seed = 0;
        [SerializeField] public bool alertTriggered = false;
        
    }
}