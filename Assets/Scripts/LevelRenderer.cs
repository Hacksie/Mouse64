using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class LevelRenderer : MonoBehaviour
    {
        [SerializeField] private Transform environmentParent;
        [SerializeField] private LevelTemplate[] levelTemplates;

        public void LoadLevel(Level level)
        {
            DestroyEnvironment();
            if (level.length < 2)
            {
                Logger.Log(this, "Level length less than 2");
                return;
            }

            var template = FindTemplate(level.name);
            if (template == null)
            {
                Logger.LogError(this, "invalid level template");
                return;
            }

            RenderStart(template);
            RenderCorridor(level, template);
            RenderEnd(level, template);
            if (template.random)
            {
                RenderSecurity(level, template);
                RenderOpenGuards(level, template);
                //RenderSuit(level, template);
            }

        }

        public void DestroyEnvironment()
        {
            for (int i = 0; i < environmentParent.childCount; i++)
            {
                Destroy(environmentParent.GetChild(i).gameObject);
            }
        }

        public LevelTemplate FindTemplate(string name)
        {
            return levelTemplates.FirstOrDefault(t => t.name == name);
        }

        public void RenderStart(LevelTemplate template)
        {
            Instantiate(template.entry, Vector3.zero, Quaternion.identity, environmentParent);
        }

        public void RenderCorridor(Level level, LevelTemplate template)
        {
            if (template.random)
            {
                for (int i = 1; i < (level.length - 1); i++)
                {
                    int index = Random.Range(0, template.randomTiles.Length);
                    Instantiate(template.randomTiles[index], CalcPosition(i, template), Quaternion.identity, environmentParent);
                }
            }
            else
            {
                for (int i = 0; i < template.fixedTiles.Length; i++)
                {
                    Instantiate(template.fixedTiles[i], CalcPosition(i + 1, template), Quaternion.identity, environmentParent);
                }
            }
        }

        public void RenderEnd(Level level, LevelTemplate template)
        {
            if (template.random)
            {
                Instantiate(template.end, CalcPosition(level.length - 1, template), Quaternion.identity, environmentParent);
            }
            else
            {
                Instantiate(template.end, CalcPosition(template.fixedTiles.Length + 1, template), Quaternion.identity, environmentParent);
            }
        }

        public void RenderSecurity(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.security);

            if ((level.length - 2) < level.security)
            {
                Logger.LogError(this, "Not enough corridor to spawn security");
            }

            float step = (level.length - 2) / (float)level.security;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.security; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * template.tileSize);
                Logger.Log(this, "Spawning cam:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnSecurity(new Vector3(pos, 0, 0));
                min += step;
                max += step;
            }
        }

        public void RenderOpenGuards(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.openGuards);

            if ((level.length - 2) < level.openGuards)
            {
                Logger.LogError(this, "Not enough corridor to spawn guard");
            }

            float step = (level.length - 2) / (float)level.openGuards;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.openGuards; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * template.tileSize);
                Logger.Log(this, "Spawning guard:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnGuard(new Vector3(pos, 0.275f, 0));
                min += step;
                max += step;
            }
        }
        public void RenderSuit(Level level, LevelTemplate template)
        {
            int pos = Mathf.RoundToInt(Random.Range(level.length - 1, level.length) * template.tileSize);
            Logger.Log(this, "Spawning suit");
            GameManager.Instance.EntityPool.SpawnSuit(new Vector3(pos, 0.275f, 0));


        }

        private Vector3 CalcPosition(float length, LevelTemplate template)
        {
            return Vector3.zero;
        }

        private Vector3 CalcPosition(int length, LevelTemplate template)
        {
            return new Vector3(length * template.tileSize, 0, 0);
        }
    }

    [System.Serializable]
    public class LevelTemplate
    {
        public string name;
        public int tileSize = 4;
        public bool random = true;
        public GameObject entry;
        public GameObject end;
        public GameObject[] randomTiles;
        public GameObject[] fixedTiles;
    }
}