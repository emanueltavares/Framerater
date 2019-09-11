using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.StressTest
{
    public class NucleonSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnTimeInterval = 1f;
        [SerializeField] private float _spawnDistance = 1f;
        [SerializeField] private Neutron[] _prefabs = new Neutron[0];
        [SerializeField] private float _minAttractionForce = 10f;
        [SerializeField] private float _maxAttractionForce = 20f;

        private float _timeSinceLastSpawn = 0f;

        protected virtual void OnEnable()
        {
            _timeSinceLastSpawn = 0f;
        }

        protected virtual void FixedUpdate()
        {
            if (Time.realtimeSinceStartup - _timeSinceLastSpawn >= _spawnTimeInterval)
            {
                SpawnNeutron();
                _timeSinceLastSpawn = Time.realtimeSinceStartup;
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _spawnDistance);
        }

        private void SpawnNeutron()
        {
            int rndIdex = Random.Range(0, _prefabs.Length);
            Neutron randomNeutronPrefab = _prefabs[rndIdex];
            Neutron neutronInstance = Instantiate(randomNeutronPrefab, randomNeutronPrefab.transform.position, randomNeutronPrefab.transform.rotation, transform);
            neutronInstance.transform.localPosition = Random.insideUnitSphere * _spawnDistance;
            neutronInstance.AttractionForce = Random.Range(_minAttractionForce, _maxAttractionForce);
        }
    }

}