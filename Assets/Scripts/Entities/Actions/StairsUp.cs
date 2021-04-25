using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class StairsUp : AbstractAction
    {

        public override void Invoke()
        {
            Logger.Log(this, "stairs up");
            var pos = GameManager.Instance.Player.transform.position;
            pos.x += 1;
            pos.y += 4;
            GameManager.Instance.Player.transform.position = pos;
            //GameManager.Instance.ConsumeStealth(-1 * powerRecovery);
        }
    }
}