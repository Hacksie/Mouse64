using UnityEngine;

namespace HackedDesign
{
    public class ExitTrigger: MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if(GameManager.Instance.Data.currentLevel.completed && other.CompareTag("Player"))
            {
                Logger.Log(this, "Exit trigger!");
                GameManager.Instance.SetMissionComplete();
            }
        }
    }
}