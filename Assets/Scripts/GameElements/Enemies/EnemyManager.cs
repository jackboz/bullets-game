using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        GameObject _enemyPrefab;
        [SerializeField]
        GameObject _enemyBigPrefab;
        [SerializeField]
        Transform _parent;

        SpawnDesc _levelSpawnDesc;
        private List<Transform> _enemiesInFireZone = new List<Transform>();
        private Vector3 _pointToFindClosestEnemy;
        private int _totalEnemies = 0;
        private int _killedEnemies = 0;

        void OnEnable()
        {
            Actions.OnEnemyKilled += DestroyEnemy;
        }

        void OnDisable()
        {
            Actions.OnEnemyKilled -= DestroyEnemy;
        }

        private void Start()
        {
            _levelSpawnDesc = Resources.Load<SpawnDesc>("level" + GameProgressStatic.LevelNumber);
#if TEST
            _levelSpawnDesc = Resources.Load<SpawnDesc>("test");
#endif
            if (_levelSpawnDesc == null)
            {
                Debug.LogError("Can't load SpawnDesc");
            }
            int _spawnPointNumber = 0;
            int rows, columns;
            foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {

                if (_spawnPointNumber >= _levelSpawnDesc.SquadSize.Length)
                {
                    rows = _levelSpawnDesc.DefaultSize.x;
                    columns = _levelSpawnDesc.DefaultSize.y;
                }
                else
                {
                    rows = _levelSpawnDesc.SquadSize[_spawnPointNumber].x;
                    columns = _levelSpawnDesc.SquadSize[_spawnPointNumber].y;
                }
                SpawnRect(spawnPoint.transform.position, rows, columns);
                _spawnPointNumber++;
            }
            foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPointBig"))
            {

                if (_spawnPointNumber >= _levelSpawnDesc.SquadSize.Length)
                {
                    rows = _levelSpawnDesc.DefaultSize.x;
                    columns = _levelSpawnDesc.DefaultSize.y;
                }
                else
                {
                    rows = _levelSpawnDesc.SquadSize[_spawnPointNumber].x;
                    columns = _levelSpawnDesc.SquadSize[_spawnPointNumber].y;
                }
                SpawnRect(spawnPoint.transform.position, rows, columns, true);
                _spawnPointNumber++;
            }
        }

        void SpawnRect(Vector3 leftTop, int rows, int columns, bool big = false)
        {
            Vector3 spawnPosition;
            GameObject enemyPrefab = big ? _enemyBigPrefab : _enemyPrefab;
            Renderer enemyRenderer = enemyPrefab.GetComponent<Renderer>();
            if (enemyRenderer == null)
            {
                Debug.LogError("Enemy doesn't have Renderer");
            }
            float zSize = enemyRenderer.bounds.size.z;
            float xSize = enemyRenderer.bounds.size.x;
            for (int i = 0; i < rows; i++)
            {
                spawnPosition = leftTop - Vector3.forward * zSize * 1.05f * i;
                for (int j = 0; j < columns; j++)
                {
                    var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, _parent);
                    _totalEnemies += 1;
                    enemy.name = "Enemy" + i * columns + j + 1;
                    spawnPosition += Vector3.right * xSize * 1.05f;
                }
            }
        }

        public void AddEnemyToFireZone(Transform enemyTransform)
        {
            _enemiesInFireZone.Add(enemyTransform);
            //Debug.Log("In zone " + enemyTransform.name + " Total " + _enemiesInFireZone.Count);
        }

        public void RemoveEnemyFromFireZone(Transform enemyTransform)
        {
            _enemiesInFireZone.Remove(enemyTransform);
            //Debug.Log("Out zone " + enemyTransform.name + " Total " + _enemiesInFireZone.Count);
        }

        public Transform GetClosest(Vector3 toPoint)
        {
            if (_enemiesInFireZone.Count == 0) return null;
            _pointToFindClosestEnemy = toPoint;
            _enemiesInFireZone.Sort(this.ClosestToPoint);
            return _enemiesInFireZone[0];
        }

        private void DestroyEnemy(Enemy enemy)
        {
            RemoveEnemyFromFireZone(enemy.transform);
            Destroy(enemy.gameObject);
            _killedEnemies += 1;
            GameProgressStatic.LevelProgress = (float)_killedEnemies / _totalEnemies;
            //Debug.Log("" + enemy.name + " destroyed");
        }

        private int ClosestToPoint(Transform x, Transform y)
        {
            float sqrDistX = (x.position - _pointToFindClosestEnemy).sqrMagnitude;
            float sqrDistY = (y.position - _pointToFindClosestEnemy).sqrMagnitude;
            return sqrDistX >= sqrDistY ? 1 : -1;
        }

        public Vector3 GetAveEnemyDirection(Vector3 playerPosition)
        {
            if (_enemiesInFireZone.Count == 0) return Vector3.up;

            Vector3 res = Vector3.zero;
            foreach (var enemy in _enemiesInFireZone)
            {
                res += enemy.position - playerPosition;
            }
            return res / _enemiesInFireZone.Count;
        }
    }
}
