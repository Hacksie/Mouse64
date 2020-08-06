using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class EntityPool : MonoBehaviour
    {
        [SerializeField] private Transform entityPool = null;
        [SerializeField] private GameObject[] securityPrefab;
        [SerializeField] private GameObject[] techPrefab;
        [SerializeField] private GameObject[] guardPrefab;
        [SerializeField] private GameObject[] suitPrefab;
        [SerializeField] private GameObject[] npcPrefab;
        [SerializeField] private List<IEntity> pool = new List<IEntity>();

        public List<IEntity> Pool { get { return this.pool; }}

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

        public IEntity SpawnSuit(Vector3 position)
        {
            GameObject go = Instantiate(suitPrefab[Random.Range(0, suitPrefab.Length)], position, Quaternion.identity, entityPool);
            IEntity entity = go.GetComponent<IEntity>();
            pool.Add(entity);
            return entity;
        }        
    }
}
