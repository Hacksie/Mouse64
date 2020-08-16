using UnityEngine;

namespace HackedDesign
{
    public class ExitTrigger : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (GameManager.Instance.CurrentState.PlayerActionAllowed && GameManager.Instance.Data.currentLevel.completed && other.CompareTag("Player"))
            {
                if (GameManager.Instance.RandomGame)
                {
                    GameManager.Instance.SetMainMenu();
                }
                else
                {
                    GameManager.Instance.SetMissionComplete();
                }
            }
        }
    }
}