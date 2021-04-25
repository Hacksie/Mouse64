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
        [SerializeField] private GameObject[] prelude1Tiles = null;
        [SerializeField] private GameObject[] prelude2Tiles = null;
        [SerializeField] private GameObject[] prelude3Tiles = null;
        [SerializeField] private GameObject[] prelude4Tiles = null;
        [SerializeField] private GameObject[] prelude5Tiles = null;
        [SerializeField] private GameObject[] missionTutorialStartTiles = null;
        [SerializeField] private GameObject[] missionSelectStartTiles = null;
        [SerializeField] private GameObject[] missionSelectEndTiles = null;
        [SerializeField] private GameObject[] lights = null;
        [SerializeField] private LevelTemplate[] levelTemplates = null;

        [Header("Settings")]
        [SerializeField] private int tileSize = 4;
        [SerializeField] private int tileHeight = 4;

        private bool[,] entitySpawns;
        //private List<bool> entitySpawns;
        private List<List<bool>> stairSpawns;


        public void LoadRandomLevel(Level level)
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            if (level.settings.length <= 3)
            {
                Logger.LogError(this, "Level length less than 3");
                return;
            }

            var template = GetRandomTemplate();
            if (template == null)
            {
                Logger.LogError(this, "invalid level template");
                return;
            }

            //entitySpawns = new List<bool>(level.settings.length * tileSize);
            entitySpawns = new bool[level.settings.length * tileSize, level.settings.floors];
            //stairSpawns = new List<bool>(level.settings.length);

            for (int y = 0; y < level.settings.floors; y++)
            {
                for (int x = 0; x < (level.settings.length - 2) * tileSize; x++)
                {
                    entitySpawns[x, y] = false;
                    //entitySpawns.Add(false);
                }
            }

            RenderStartTile(level, template);
            RenderCorridorTiles(level, template);

            RenderBossTile(level, template);
            RenderEndTile(level, template);

            RenderDoors(level, template);

            if (GameManager.Instance.GameSettings.spawn)
            {
                RenderSecurity(level, template);
                RenderOpenGuards(level, template);
                RenderDrones(level, template);
                RenderGCannon(level, template);
                RenderRCannon(level, template);
                RenderWCannon(level, template);
                RenderSuit(level, template);
            }

            RenderLights(level, template);

        }

        public void DestroyEnvironment()
        {
            for (int i = 0; i < environmentParent.childCount; i++)
            {
                Destroy(environmentParent.GetChild(i).gameObject);
            }
        }

        public void LoadPrelude1Level()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            for (int i = 0; i < prelude1Tiles.Length; i++)
            {
                Instantiate(prelude1Tiles[i], CalcPosition(i, 0), Quaternion.identity, environmentParent);
            }
        }

        public void LoadPrelude2Level()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            for (int i = 0; i < prelude2Tiles.Length; i++)
            {
                Instantiate(prelude2Tiles[i], CalcPosition(i, 0), Quaternion.identity, environmentParent);
            }
        }

        public void LoadPrelude3Level()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            for (int i = 0; i < prelude3Tiles.Length; i++)
            {
                Instantiate(prelude3Tiles[i], CalcPosition(i, 0), Quaternion.identity, environmentParent);
            }
        }

        public void LoadPrelude4Level()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            for (int i = 0; i < prelude4Tiles.Length; i++)
            {
                Instantiate(prelude4Tiles[i], CalcPosition(i, 0), Quaternion.identity, environmentParent);
            }
        }

        public void LoadPrelude5Level()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();
            for (int i = 0; i < prelude5Tiles.Length; i++)
            {
                Instantiate(prelude5Tiles[i], CalcPosition(i, 0), Quaternion.identity, environmentParent);
            }
        }

        public void LoadMissionTutorialLevel()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();

            Instantiate(missionTutorialStartTiles[Random.Range(0, missionTutorialStartTiles.Length)], CalcPosition(0, 0), Quaternion.identity, environmentParent);
            Instantiate(missionSelectEndTiles[Random.Range(0, missionSelectEndTiles.Length)], CalcPosition(1, 0), Quaternion.identity, environmentParent);
        }

        public void LoadMissionSelectLevel()
        {
            Random.InitState(GameManager.Instance.Data.seed);
            DestroyEnvironment();

            Instantiate(missionSelectStartTiles[Random.Range(0, missionSelectStartTiles.Length)], CalcPosition(0, 0), Quaternion.identity, environmentParent);
            Instantiate(missionSelectEndTiles[Random.Range(0, missionSelectEndTiles.Length)], CalcPosition(1, 0), Quaternion.identity, environmentParent);
        }



        public LevelTemplate GetRandomTemplate()
        {
            return levelTemplates[Random.Range(0, levelTemplates.Length)];
        }

        public void RenderStartTile(Level level, LevelTemplate template)
        {
            for (int y = 0; y < level.settings.floors; y++)
            {
                Instantiate(template.entryTile, CalcPosition(0, y), Quaternion.identity, environmentParent);
            }
        }

        public void RenderCorridorTiles(Level level, LevelTemplate template)
        {
            for (int i = 1; i < (level.settings.length - 2); i++)
            {
                for (int y = 0; y < level.settings.floors; y++)
                {

                    if (y < (level.settings.floors - 1) && Random.value < (1.0f / 10.0f))
                    {
                        Logger.Log(this, "stair tile");
                        Instantiate(template.stairupTile, CalcPosition(i, y), Quaternion.identity, environmentParent);
                        Instantiate(template.stairdownTile, CalcPosition(i, y + 1), Quaternion.identity, environmentParent);
                        // Stair up

                        // Stair down
                        y++;
                    }
                    else
                    {
                        int index = Random.Range(0, template.randomTiles.Length);
                        Instantiate(template.randomTiles[index], CalcPosition(i, y), Quaternion.identity, environmentParent);
                    }
                }

            }
        }

        public void RenderBossTile(Level level, LevelTemplate template)
        {
            for (int y = 0; y < level.settings.floors; y++)
            {
                Instantiate(template.bossTile, CalcPosition(level.settings.length - 2, y), Quaternion.identity, environmentParent);
            }
        }

        public void RenderEndTile(Level level, LevelTemplate template)
        {
            for (int y = 0; y < level.settings.floors; y++)
            {
                Instantiate(template.exitTile, CalcPosition(level.settings.length - 1, y), Quaternion.identity, environmentParent);
            }
        }

        public void RenderDoors(Level level, LevelTemplate template)
        {
            var spawns = GameObject.FindGameObjectsWithTag("DoorSpawn").ToList();

            for (int i = 0; i < level.settings.doors; i++)
            {
                if (spawns.Count <= 0)
                    break;

                var index = Random.Range(0, spawns.Count);

                Instantiate(template.doorway, spawns[index].transform.position, Quaternion.identity, environmentParent);
                GameManager.Instance.EntityPool.SpawnDoor(spawns[index].transform.position);
                //entitySpawns[Mathf.RoundToInt(spawns[index].transform.position.x)] = true;
                spawns.RemoveAt(index);
            }
        }


        public void RenderSecurity(Level level, LevelTemplate template)
        {
            if ((level.settings.length - 2) < level.settings.security)
            {
                Logger.LogError(this, "Not enough corridor to spawn security");
                return;
            }


            for (int i = 0; i < level.settings.security; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x, y] = true;
                    GameManager.Instance.EntityPool.SpawnSecurity(CalcPosition(x, y));
                    break;
                }
            }

        }

        public void RenderOpenGuards(Level level, LevelTemplate template)
        {
            if ((level.settings.length - 2) < level.settings.openGuards)
            {
                Logger.LogError(this, "Not enough corridor to spawn guard");
                return;
            }

            for (int i = 0; i < level.settings.openGuards; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x, y] = true;

                    GameManager.Instance.EntityPool.SpawnGuard(new Vector3(x, (y * tileHeight) + 0.275f, 0));
                    break;
                }
            }
        }

        public void RenderDrones(Level level, LevelTemplate template)
        {
            if ((level.settings.length - 2) < level.settings.drones)
            {
                Logger.LogError(this, "Not enough corridor to spawn drone");
                return;
            }

            for (int i = 0; i < level.settings.drones; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x, y] = true;
                    GameManager.Instance.EntityPool.SpawnDrone(new Vector3(x, (y * tileHeight) + 0.275f, 0));
                    break;
                }
            }
        }

        // TODO: Fix the spawns for cameras and cannons so that there isn't overlap. Don't use * 4, use 1 and add an offset

        public void RenderGCannon(Level level, LevelTemplate template)
        {
            if ((level.settings.length - 2) < level.settings.gcannon)
            {
                Logger.LogError(this, "Not enough corridor to spawn gcannon");
                return;
            }

            for (int i = 0; i < level.settings.gcannon; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x, y] = true;

                    GameManager.Instance.EntityPool.SpawnGCannon(new Vector3(x + 0.5f, (y * tileHeight) + 0.275f, 0));
                    break;
                }
            }
        }

        public void RenderRCannon(Level level, LevelTemplate template)
        {
            if ((level.settings.length - 2) < level.settings.rcannon)
            {
                Logger.LogError(this, "Not enough corridor to spawn rcannon");
                return;
            }

            for (int i = 0; i < level.settings.rcannon; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x, y] = true;

                    GameManager.Instance.EntityPool.SpawnRCannon(new Vector3(x + 0.5f, (y * tileHeight) + 0.275f, 0));
                    break;
                }
            }
        }

        public void RenderWCannon(Level level, LevelTemplate template)
        {
            if ((level.settings.length - 2) < level.settings.wcannon)
            {
                Logger.LogError(this, "Not enough corridor to spawn wcannon");
                return;
            }

            for (int i = 0; i < level.settings.wcannon; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x, y] = true;
                    GameManager.Instance.EntityPool.SpawnWCannon(new Vector3(x + 0.5f, (y * tileHeight) + 3.5f, 0));
                    break;
                }
            }
        }

        public void RenderSuit(Level level, LevelTemplate template)
        {
            float min = level.settings.length - 2;
            float max = level.settings.length - 1;
            int x = Mathf.RoundToInt(Random.Range(min, max) * tileSize);
            var y = Random.Range(0, level.settings.floors);
            Logger.Log(this, "Spawning suit");
            GameManager.Instance.EntityPool.SpawnSuit(new Vector3(x, (y * tileHeight) + 0.275f, 0));
        }

        public void RenderLights(Level level, LevelTemplate template)
        {
            var count = Random.Range(0, 5);

            for (int i = 0; i < count; i++)
            {
                for (int r = 0; r < 1000; r++)
                {
                    var x = Random.Range(tileSize, (level.settings.length - 3) * tileSize);
                    var y = Random.Range(0, level.settings.floors);

                    if (entitySpawns[x, y])
                    {
                        continue;
                    }

                    entitySpawns[x,y] = true;

                    var index = Random.Range(0, lights.Length);

                    Instantiate(lights[index], new Vector3(x, y * tileHeight, 0), Quaternion.identity, environmentParent);
                    break;
                }
            }
        }

        public Vector3 CalcPosition(int position, int floor)
        {
            return new Vector3(position * tileSize, floor * tileHeight, 0);
        }
    }

    [System.Serializable]
    public class LevelTemplate
    {
        public string name;
        public GameObject entryTile;
        public GameObject bossTile;
        public GameObject exitTile;
        public GameObject stairupTile;
        public GameObject stairdownTile;

        public GameObject[] randomTiles;
        public GameObject doorway;
    }
}