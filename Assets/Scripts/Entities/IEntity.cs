using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public interface IEntity  {
        void UpdateBehaviour();
        void UpdateLateBehaviour();
        void Hit();
        void Alert();
    }
}