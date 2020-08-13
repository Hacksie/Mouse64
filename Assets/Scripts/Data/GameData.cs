using System.Collections.Generic;
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

    [System.Serializable]
    public class Level {
        public string corp = "Arisana";
        public string target = "Z.Bana";
        public string missionLevel = "hotdog";
        public string difficulty = "Easy";
        public int length = 2;
        public int security = 3;
        public int tech = 4;
        public int openGuards = 2;
        public int drones = 2;
        public int gcannon = 2;
        public int wcannon = 2;
        public int rcannon = 2;
        public int doors = 8;
        public int window = 64;
        public LevelAlertSpawn alertSpawn;
        public int maxAlert = 5;
        public List<string> dialogue;
        public int currentDialogue;
        public bool completed = false;
        // boss
        // end condition
    }

    public enum LevelAlertSpawn
    {
        Guard,
        Drone,
        Any
    }
}