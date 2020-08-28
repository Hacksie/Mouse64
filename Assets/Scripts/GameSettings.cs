
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Mouse/Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] public float easyAdj = 1.0f;
        [SerializeField] public float mediumAdj = 0.8f;
        [SerializeField] public float hardAdj = 0.6f;
        [SerializeField] public int alertGuards = 10;
    }
}