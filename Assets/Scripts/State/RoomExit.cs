using UnityEngine;

namespace HackedDesign
{
    public class RoomExit : MonoBehaviour
    {

        public void Activate()
        {
            GameManager.Instance.Reset();
            //GameManager.Instance.EntityPool.DestroyEntities();
            //GameManager.Instance.LevelRenderer.LoadRandomLevel(GameManager.Instance.Data.currentLevel);
            //AudioManager.Instance.PlayRandomGameMusic();
            GameManager.Instance.NextLevel();
            GameManager.Instance.SetTutorialMissionSelect();
            //GameManager.Instance.SetMissionSelect();
        }
    }
}