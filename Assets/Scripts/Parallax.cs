using UnityEngine;

namespace HackedDesign
{
    public class Parallax : MonoBehaviour
    {
        private Vector2 bounds, startpos;

        [SerializeField]
        private Camera cam = null;

        [SerializeField]
        private float parallaxEffect = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            if (cam == null)
            {
                Logger.LogError(this, "cam not set");
            }
            startpos = transform.position;
            bounds = GetComponent<SpriteRenderer>().bounds.size;
        }

        void Update()
        {
            Vector2 temp = (cam.transform.position * (1 - parallaxEffect));
            Vector2 dist = new Vector2(cam.transform.position.x * parallaxEffect, 0);

            transform.position = startpos + dist;

            if (temp.x > (startpos.x + bounds.x))
                startpos.x += bounds.x;
            if (temp.x < (startpos.x - bounds.x))
                startpos.x -= bounds.x;
            if (temp.y > (startpos.y + bounds.y))
                startpos.y += bounds.y;
            if (temp.y < (startpos.y - bounds.y))
                startpos.y -= bounds.y;
        }
    }
}