using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class Power : AbstractAction
    {
      

        [Header("Settings")]
        [SerializeField] private int powerRecovery = 33;

        public override void Invoke()
        {
            Logger.Log(this, "use power");
            GameManager.Instance.ConsumeStealth(-1 * powerRecovery);
        }
    }
}