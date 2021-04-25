using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Mouse/Settings/Level")]
    public class LevelSettings: ScriptableObject {
        [SerializeField] public GameDifficulty difficulty = GameDifficulty.Easy;
        [SerializeField] public bool randomizeDetails = true;
        [SerializeField] public int length = 2;
        [SerializeField] public int floors = 1;
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
        [SerializeField] public Dialog startingDialogue;
    }
}
