using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        [Header("Save Properties")]
        [SerializeField] private int gameVersion = 0;
        [SerializeField] private int gameSlot = 0;
        [SerializeField] public int health = 100;
        [SerializeField] public float energy = 100;
        [SerializeField] public int bullets = 10;
        [SerializeField] public float timer = 180;
        [SerializeField] public int alert = 0;
        [SerializeField] public int maxAlert = 10;
        [SerializeField] public int score = 0;
        //[SerializeField] public string currentLevelName = "blue";
        //[SerializeField] public int currentLevelLength = 2;
        [SerializeField] public int currentLevelIndex = 0;
        [SerializeField] public Level currentLevel;
    }

    [System.Serializable]
    public class Level {
        public string name = "intro";
        public string missionLevel = "hotdog";
        public string corp = "Arisana";
        public string target = "Z.Bana";
        public string difficulty = "Easy";
        public int length = 2;
        public int security = 3;
        public int tech = 4;
        public int openGuards = 2;
        public int drones = 2;
        public int window = 180;
        public string[] dialogue;
        public int currentDialogue;
        public bool completed = false;
        // boss
        // end condition
    }
}