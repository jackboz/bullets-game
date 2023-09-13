using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField]
        Transform _leftShootingPoint;
        [SerializeField]
        Transform _leftShootingAxis;
        [SerializeField]
        private Transform _leftShootingPointInitialPos;
        [SerializeField]
        Transform _rightShootingPoint;
        [SerializeField]
        Transform _rightShootingAxis;
        [SerializeField]
        private Transform _rightShootingPointInitialPos;
        [SerializeField]
        GameObject _bulletPrefab;
        [SerializeField]
        EnemyManager _enemyManager;

        [SerializeField]
        private float _fireRadius = 12.5f;
        [SerializeField]
        private float _radiusAcceleration = 2f;
        [SerializeField]
        private float _fireDelay = 0.25f; // in sec
        //[SerializeField]
        //private float _fireHeight = 1.22f;

        //public float LeftAngleDelta { get; private set; }
        //float RightAngleDelta { get; private set; }

        private bool _isOn;
        private float _fireDelayTimer = 0;
        private Vector3 _currentDirection1;
        private Vector3 _currentDirection2;
        private Vector3 _initialDirection1;
        private Vector3 _initialDirection2;
        private float _minLeftAngle = 60f;
        private float _maxLeftAngle = 180f;
        private float _minRightAngle = 0;
        private float _maxRightAngle = 120f;

        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                _fireDelayTimer = 0;
            }
        }

        void Awake()
        {
            if (_bulletPrefab == null)
            {
                Debug.LogError("Bullet Prefab is not set");
            }
            if ((_leftShootingPoint == null) || (_rightShootingPoint == null))
            {
                Debug.LogError("Shooting Points are not set");
            }
            if ((_leftShootingAxis == null) || (_rightShootingAxis == null))
            {
                Debug.LogError("Shooting Axis are not set");
            }
            if ((_leftShootingPointInitialPos == null) || (_rightShootingPointInitialPos == null))
            {
                Debug.LogError("Shooting Initial Positions are not set");
            }
            if (_enemyManager == null)
            {
                Debug.LogError("Enemy Manager is not set");
            }

            _currentDirection1 = _leftShootingPoint.position - _leftShootingAxis.position;
            _currentDirection1.y = 0;
            _initialDirection1 = transform.InverseTransformDirection(_currentDirection1);
            _currentDirection2 = _rightShootingPoint.position - _rightShootingAxis.position;
            _currentDirection2.y = 0;
            _initialDirection2 = transform.InverseTransformDirection(_currentDirection2);

            //LeftAngleDelta = 0;
            //RightAngleDelta = 0;
        }

        void Start()
        {
            IsOn = true;
        }

        void Update()
        {
            if (!_isOn) return;

            if (_fireDelayTimer < _fireDelay)
            {
                _fireDelayTimer += Time.deltaTime;
            }
            else
            {
                Fire();
                _fireDelayTimer = 0;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _enemyManager.AddEnemyToFireZone(other.transform);
            }
        }

        void OnTriggerExit(Collider other)
        {
            //Debug.Log(other.gameObject.name);
            if (other.CompareTag("Enemy"))
            {
                _enemyManager.RemoveEnemyFromFireZone(other.transform);
            }
        }

        void Fire()
        {
            Vector3 targetDirection;
            //Transform enemy = _enemyManager.GetClosest(_leftShootingPointInitialPos.position);
            Transform enemy = GetClosestEnemy(_leftShootingPointInitialPos.position, _fireRadius);
            if (enemy == null)
            {
                targetDirection = transform.TransformDirection(_initialDirection1);
            }
            else
            {
                GameObject bullet = Instantiate(_bulletPrefab, _leftShootingPoint.transform.position, Quaternion.identity);
                bullet.GetComponent<Projectile>()?.Fire(_currentDirection1);

                targetDirection = enemy.position - _leftShootingAxis.position;
                targetDirection.y = 0;
            }
            Vector3 direction = Vector3.Lerp(_currentDirection1, targetDirection, Time.deltaTime * _radiusAcceleration);
            direction.y = 0;
            //LeftAngleDelta = Vector3.SignedAngle(_currentDirection1, direction, Vector3.up);
            //_leftShootingAxis.Rotate(Vector3.up, Vector3.SignedAngle(_currentDirection1, direction, Vector3.up));
            TurnLeftGun(direction);

            //enemy = _enemyManager.GetClosest(_rightShootingPointInitialPos.position);
            enemy = GetClosestEnemy(_rightShootingPointInitialPos.position, _fireRadius);
            if (enemy == null)
            {
                targetDirection = transform.TransformDirection(_initialDirection2);
            }
            else
            {
                GameObject bullet = Instantiate(_bulletPrefab, _rightShootingPoint.transform.position, Quaternion.identity);
                bullet.GetComponent<Projectile>()?.Fire(_currentDirection2);

                targetDirection = enemy.position - _rightShootingAxis.position;
                targetDirection.y = 0;
            }
            direction = Vector3.Lerp(_currentDirection2, targetDirection, Time.deltaTime * _radiusAcceleration);
            direction.y = 0;
            //RightAngleDelta = Vector3.SignedAngle(_currentDirection2, direction, Vector3.up);
            //_rightShootingPoint.Rotate(Vector3.up, Vector3.SignedAngle(_currentDirection2, direction, Vector3.up));
            TurnRightGun(direction);
        }

        private void TurnLeftGun(Vector3 direction)
        {
            float newAngle = Mathf.Clamp(Vector3.SignedAngle(direction, transform.right, Vector3.up), _minLeftAngle, _maxLeftAngle);
            Vector3 newDirectioin = Quaternion.Euler(0, -newAngle, 0) * transform.right;
            _leftShootingPoint.RotateAround(_leftShootingAxis.position, Vector3.up, Vector3.SignedAngle(_currentDirection1, newDirectioin, Vector3.up));
            _currentDirection1 = _leftShootingPoint.transform.position - _leftShootingAxis.position;
            _currentDirection1.y = 0;
        }

        private void TurnRightGun(Vector3 direction)
        {
            float newAngle = Mathf.Clamp(Vector3.SignedAngle(direction, transform.right, Vector3.up), _minRightAngle, _maxRightAngle);
            Vector3 newDirectioin = Quaternion.Euler(0, -newAngle, 0) * transform.right;
            _rightShootingPoint.RotateAround(_rightShootingAxis.position, Vector3.up, Vector3.SignedAngle(_currentDirection2, newDirectioin, Vector3.up));
            _currentDirection2 = _rightShootingPoint.transform.position - _rightShootingAxis.position;
            _currentDirection2.y = 0;
        }

        private Transform GetClosestEnemy(Vector3 centerPoint, float raduis)
        {
            SortedList<float, Transform> enemies = new SortedList<float, Transform>();
            Collider[] colliders = Physics.OverlapSphere(centerPoint, raduis);
            Vector3 distance;
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    distance = collider.transform.position - centerPoint;
                    if (! enemies.ContainsKey(distance.sqrMagnitude))
                    {
                        enemies.Add(distance.sqrMagnitude, collider.transform);
                    }     
                }
            }
            if (enemies.Count == 0) return null;
            return enemies.Values[0];
        }

        public Vector3 GetAveEnemyDirection(Vector3 playerPosition)
        {
            Vector3 res = Vector3.zero;
            Collider[] colliders = Physics.OverlapSphere(playerPosition, _fireRadius);
            int enemies = 0;
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    res += collider.transform.position - playerPosition;
                    enemies += 1;
                }
            }
            if (enemies == 0) return Vector3.up;
            return res / enemies;
        }
    }
}