
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Mouse/Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] public bool invulnerability = true;
        [SerializeField] public bool skipTutorial = false;
        [SerializeField] public float easyAdj = 1.0f;
        [SerializeField] public float mediumAdj = 0.8f;
        [SerializeField] public float hardAdj = 0.6f;
        [SerializeField] public int alertGuards = 10;
        [SerializeField] public Color defaultLightColor = Color.gray;
        [SerializeField] public Color preludeLightColor = Color.gray;
        [SerializeField] public Color roomLightColor = Color.gray;
        [SerializeField] public Color alertLightColor = Color.red;
        [SerializeField] public bool useMouse = false;
        [SerializeField] public bool skipDialog = true;
    }
}