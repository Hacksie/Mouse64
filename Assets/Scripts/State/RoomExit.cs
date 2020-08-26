using UnityEngine;

namespace HackedDesign
{
    public class RoomExit : MonoBehaviour
    {
        public void Activate()
        {
            GameManager.Instance.SetMissionSelect();
        }
    }
}