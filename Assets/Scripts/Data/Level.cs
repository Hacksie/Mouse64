using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [System.Serializable]
    public class Level {
        [Header("Level settings")]
        [SerializeField] public string corp = "Arisana";
        [SerializeField] public string target = "Z.Bana";
        [SerializeField] public string missionLevel = "hotdog";
        [SerializeField] public string difficulty = "Easy";
        [SerializeField] public int length = 2;
        [SerializeField] public int security = 3;
        [SerializeField] public int tech = 4;
        [SerializeField] public int openGuards = 2;
        [SerializeField] public int drones = 2;
        [SerializeField] public int gcannon = 2;
        [SerializeField] public int wcannon = 2;
        [SerializeField] public int rcannon = 2;
        [SerializeField] public int doors = 8;
        [SerializeField] public int window = 64;
        [SerializeField] public LevelAlertSpawn alertSpawn;
        [SerializeField] public int maxAlert = 5;
        [SerializeField] public List<string> dialogue;
        [SerializeField] public int currentDialogue;
        [SerializeField] public bool completed = false;
        [SerializeField] public int score = 0;
        [SerializeField] public int reactions = 0;
    }
}