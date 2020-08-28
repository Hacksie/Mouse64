
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Mouse/Settings/Player")]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] public float fireRate = 0.5f;
        [SerializeField] public float runSpeed = 3;
        [SerializeField] public float crouchSpeed = 3;
        [Range(0, .3f)] [SerializeField] public float movementSmoothing = .05f;
        [SerializeField] public float stealthRate = 1f;
        [SerializeField] public float lookAngle = 0;
        [SerializeField] public float maxAngle = 75.0f;
        [SerializeField] public float minAngle = -25.0f;
        [SerializeField] public float shootDistance = 4.0f;
        [SerializeField] public float interactDistance = 0.5f;
        [SerializeField] public LayerMask shootMask = 0;
        [SerializeField] public LayerMask interactMask = 0;        
    }
}