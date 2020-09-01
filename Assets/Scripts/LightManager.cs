using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace HackedDesign
{
    public class LightManager : MonoBehaviour
    {
        [Header("GameObjects")]
        [SerializeField] private Light2D globalLight = null;

        public void Awake()
        {
            this.globalLight = this.globalLight ?? GetComponent<Light2D>();
           
        }

        public void SetGlobalLight(GlobalLight light)
        {
            switch (light)
            {
                case GlobalLight.Alert:
                    this.globalLight.color = GameManager.Instance.GameSettings.alertLightColor;
                    break;
                case GlobalLight.Room:
                    this.globalLight.color = GameManager.Instance.GameSettings.roomLightColor;
                    break;
                case GlobalLight.Default:
                default:
                    this.globalLight.color = GameManager.Instance.GameSettings.defaultLightColor;
                    break;
            }
        }        
    }

    public enum GlobalLight {
        Default,
        Room,
        Alert
    }    
}