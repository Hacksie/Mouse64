using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class BulletDrop : AbstractAction
    {


        [Header("Settings")]
        [SerializeField] private int bulletRecovery = 1;


        public override void Invoke()
        {
            Logger.Log(this, "bullet pickup");
            GameManager.Instance.ConsumeBullet(-1 * bulletRecovery);
            AudioManager.Instance.PlayBulletPickup();
        }

    }
}