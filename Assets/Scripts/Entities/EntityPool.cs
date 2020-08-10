using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class EntityPool : MonoBehaviour
    {
        [SerializeField] private Transform entityPool = null;
        [SerializeField] private GameObject[] securityPrefab = null;
        [SerializeField] private GameObject[] techPrefab = null;
        [SerializeField] private GameObject[] guardPrefab = null;
        [SerializeField] private GameObject[] suitPrefab = null;
        [SerializeField] private GameObject[] dronePrefab = null;
        [SerializeField] private List<IEntity> pool = new List<IEntity>();

        public List<IEntity> Pool { get { return this.pool; } }

        public void Awake()
        {
            this.entityPool = entityPool ?? this.transform;
        }

        public void DestroyEntities()
        {
            for (int i = 0; i < entityPool.childCount; i++)
            {
                Destroy(entityPool.GetChild(i).gameObject);
            }

            this.pool.Clear();
        }

        public Vector3 FindGuardSpawn(Vector3 playerPosition)
        {
            
            var spawns = GameObject.FindGameObjectsWithTag("Respawn");

            var pos = spawns[Random.Range(0, spawns.Length)].transform.position;

            return new Vector3(pos.x + 0.5f, 0.275f, 0);

        }

        public IEntity SpawnSecurity(Vector3 position)
        {
            GameObject go = Instantiate(securityPrefab[Random.Range(0, securityPrefab.Length)], position, Quaternion.identity, entityPool);
            IEntity entity = go.GetComponent<IEntity>();
            pool.Add(entity);
            return entity;
        }

        public IEntity SpawnGuard(Vector3 position)
        {
            GameObject go = Instantiate(guardPrefab[Random.Range(0, guardPrefab.Length)], position, Quaternion.identity, entityPool);
            IEntity entity = go.GetComponent<IEntity>();
            pool.Add(entity);
            return entity;
        }

        public IEntity SpawnDrone(Vector3 position)
        {
            GameObject go = Instantiate(dronePrefab[Random.Range(0, dronePrefab.Length)], position, Quaternion.identity, entityPool);
            IEntity entity = go.GetComponent<IEntity>();
            pool.Add(entity);
            return entity;
        }

        public IEntity SpawnSuit(Vector3 position)
        {
            GameObject go = Instantiate(suitPrefab[Random.Range(0, suitPrefab.Length)], position, Quaternion.identity, entityPool);
            IEntity entity = go.GetComponent<IEntity>();
            pool.Add(entity);
            return entity;
        }
    }
}
