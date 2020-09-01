using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [System.Serializable]
    public class Level {
        [Header("Level settings")]
        [SerializeField] public string corp = "Arisana";
        [SerializeField] public string target = "Z.Bana";
        [SerializeField] public List<string> dialogue;
        [SerializeField] public Dialog currentDialogue;
        [SerializeField] public int currentDialogueIndex;
        [SerializeField] public bool completed = false;
        [SerializeField] public int score = 0;
        [SerializeField] public int reactions = 0;
        [SerializeField] public LevelSettings settings;
    }
}