using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Enemy : MonoBehaviour
    {
        private enum EnemyState
        {
            Walking,
            Wait
        }

        [SerializeField]
        private float _speed = 1.25f;
        [SerializeField]
        private float _turningSpeed = 120f; // degrees per sec

        private EnemyState _currentState;

        private Transform _playerTransform;
        //private CharacterController _controller;
        private Rigidbody _rigidbody;

        void Awake()
        {
            _playerTransform = GameObject.Find("PlayerArmature")?.GetComponent<Transform>();
            if (_playerTransform == null)
            {
                Debug.LogError("PlayerArmature game object not found");
            }
            //_controller = GetComponent<CharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
            //_rigidbody = GetComponent<Rigidbody>();
            Vector3 direction = _playerTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            _currentState = EnemyState.Wait;
        }

        void FixedUpdate()
        {
            switch (_currentState)
            {
                case EnemyState.Wait:
                    break;
                case EnemyState.Walking:
                    WalkingBehavior();
                    break;
                default:
                    Debug.LogError("Unexpected Enemy State");
                    break;
            }

        }

        void WalkingBehavior()
        {
            Vector3 direction = _playerTransform.position - _rigidbody.position;
            direction.y = 0;
            direction.Normalize();

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _turningSpeed * Time.deltaTime);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, _turningSpeed * Time.deltaTime));

            _rigidbody.MovePosition(_rigidbody.position + transform.forward * _speed * Time.deltaTime);
            //transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            //_controller.Move(Vector3.forward * _speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Actions.OnLevelFailed?.Invoke();
            }
        }

        public void ActivateEnemy()
        {
            _currentState = EnemyState.Walking;
        }
    }
}