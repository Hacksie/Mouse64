using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class LevelRenderer : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Transform environmentParent = null;
        [Header("Prefabs")]
        [SerializeField] private GameObject[] missionSelectStartTiles = null;
        [SerializeField] private GameObject[] missionSelectEndTiles = null;
        [SerializeField] private LevelTemplate[] levelTemplates = null;
        [Header("Settings")]
        [SerializeField] private int tileSize = 4;

        private List<int> entitySpawns;


        public void LoadRandomLevel(Level level)
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            if (level.length < 3)
            {
                Logger.Log(this, "Level length less than 2");
                return;
            }

            var template = GetRandomTemplate();
            if (template == null)
            {
                Logger.LogError(this, "invalid level template");
                return;
            }

            RenderStartTile(template);
            RenderCorridorTiles(level, template);
            RenderBossTile(level, template);
            RenderEndTile(level, template);
            RenderDoors(level, template);

            RenderSecurity(level, template);
            RenderOpenGuards(level, template);
            RenderDrones(level, template);
            RenderGCannon(level, template);
            RenderRCannon(level, template);
            RenderWCannon(level, template);
            RenderSuit(level, template);

        }

        public void LoadMissionSelectLevel()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();

            Instantiate(missionSelectStartTiles[Random.Range(0, missionSelectStartTiles.Length)], CalcPosition(0), Quaternion.identity, environmentParent);
            Instantiate(missionSelectEndTiles[Random.Range(0, missionSelectEndTiles.Length)], CalcPosition(1), Quaternion.identity, environmentParent);
        }

        public void DestroyEnvironment()
        {
            for (int i = 0; i < environmentParent.childCount; i++)
            {
                Destroy(environmentParent.GetChild(i).gameObject);
            }
        }

        public LevelTemplate GetRandomTemplate()
        {
            return levelTemplates[Random.Range(0, levelTemplates.Length)];
        }

        public void RenderStartTile(LevelTemplate template)
        {
            Instantiate(template.entryTile, Vector3.zero, Quaternion.identity, environmentParent);
        }

        public void RenderCorridorTiles(Level level, LevelTemplate template)
        {
            for (int i = 1; i < (level.length - 2); i++)
            {
                int index = Random.Range(0, template.randomTiles.Length);
                Instantiate(template.randomTiles[index], CalcPosition(i), Quaternion.identity, environmentParent);
            }
        }

        public void RenderBossTile(Level level, LevelTemplate template)
        {
            Instantiate(template.bossTile, CalcPosition(level.length - 2), Quaternion.identity, environmentParent);
        }

        public void RenderEndTile(Level level, LevelTemplate template)
        {
            Instantiate(template.exitTile, CalcPosition(level.length - 1), Quaternion.identity, environmentParent);
        }

        public void RenderDoors(Level level, LevelTemplate template)
        {
            var spawns = GameObject.FindGameObjectsWithTag("DoorSpawn").ToList();

            for (int i = 0; i < level.doors; i++)
            {
                if (spawns.Count <= 0)
                    break;

                var index = Random.Range(0, spawns.Count);

                Instantiate(template.doorway, spawns[index].transform.position, Quaternion.identity, environmentParent);
                GameManager.Instance.EntityPool.SpawnDoor(spawns[index].transform.position);
                spawns.RemoveAt(index);
            }
        }

        // public void CalcSpawns(Level level, LevelTemplate template)
        // {
        //     entitySpawns = new List<int>(level.length * tileSize);

        // }

        public void RenderSecurity(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.security);

            if ((level.length - 2) < level.security)
            {
                Logger.LogError(this, "Not enough corridor to spawn security");
                return;
            }

            float step = (level.length - 3) / (float)level.security;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.security; i++)
            {
                GameManager.Instance.EntityPool.SpawnSecurity(CalcPosition(Mathf.RoundToInt(Random.Range(min, max))));
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
                return;
            }

            float step = (level.length - 2) / (float)level.openGuards;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.openGuards; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
                Logger.Log(this, "Spawning guard:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnGuard(new Vector3(pos, 0.275f, 0));
                min += step;
                max += step;
            }
        }

        public void RenderDrones(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.drones);

            if ((level.length - 2) < level.drones)
            {
                Logger.LogError(this, "Not enough corridor to spawn drone");
                return;
            }

            float step = (level.length - 2) / (float)level.drones;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.drones; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
                Logger.Log(this, "Spawning drone:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnDrone(new Vector3(pos, 0.275f, 0));
                min += step;
                max += step;
            }
        }

        // TODO: Fix the spawns for cameras and cannons so that there isn't overlap. Don't use * 4, use 1 and add an offset

        public void RenderGCannon(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.gcannon);

            if ((level.length - 2) < level.gcannon)
            {
                Logger.LogError(this, "Not enough corridor to spawn gcannon");
                return;
            }

            float step = (level.length - 2) / (float)level.gcannon;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.gcannon; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
                Logger.Log(this, "Spawning gcannon:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnGCannon(new Vector3(pos + 0.5f, 0.275f, 0));
                min += step;
                max += step;
            }
        }

        public void RenderRCannon(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.rcannon);

            if ((level.length - 2) < level.rcannon)
            {
                Logger.LogError(this, "Not enough corridor to spawn rcannon");
                return;
            }

            float step = (level.length - 2) / (float)level.rcannon;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.rcannon; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
                Logger.Log(this, "Spawning rcannon:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnRCannon(new Vector3(pos + 0.5f, 3.5f, 0));
                min += step;
                max += step;
            }
        }

        public void RenderWCannon(Level level, LevelTemplate template)
        {
            List<int> taken = new List<int>(level.wcannon);

            if ((level.length - 2) < level.wcannon)
            {
                Logger.LogError(this, "Not enough corridor to spawn wcannon");
                return;
            }

            float step = (level.length - 2) / (float)level.wcannon;
            float min = 1;
            float max = min + step;

            for (int i = 0; i < level.wcannon; i++)
            {
                int pos = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
                Logger.Log(this, "Spawning wcannon:", pos.ToString(), " (", min.ToString(), " - ", max.ToString(), ")");
                GameManager.Instance.EntityPool.SpawnWCannon(new Vector3(pos + 0.5f, 2.5f, 0));
                min += step;
                max += step;
            }
        }

        public void RenderSuit(Level level, LevelTemplate template)
        {
            float min = level.length - 2;
            float max = level.length - 1;
            int pos = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
            Logger.Log(this, "Spawning suit");
            GameManager.Instance.EntityPool.SpawnSuit(new Vector3(pos, 0.275f, 0));
        }

        public Vector3 CalcPosition(int position)
        {
            return new Vector3(position * tileSize, 0, 0);
        }
    }

    [System.Serializable]
    public class LevelTemplate
    {
        public string name;
        public GameObject entryTile;
        public GameObject bossTile;
        public GameObject exitTile;

        public GameObject[] randomTiles;
        public GameObject doorway;
        public GameObject[] lightGreebles;
        public GameObject[] floorGreebles;

    }
}