﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player1;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        var player1_gameObject = GameObject.Find("Player") ?? GameObject.Find("Player1");
        _player1 = player1_gameObject.GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();

        if(_player1 == null)
        {
            Debug.LogError("_player is null.");
        }

        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Animator is null.");
        }

    }


        // Update is called once per frame
        void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null) 
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            GetComponent<Collider2D>().enabled = false;
            Destroy(GetComponent<Collider2D>()); // destroys the collider so we don't play the effects again
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            if (other.transform.parent != null && 
                other.transform.parent.name.ToLower().Contains("triple"))
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }

            if (_player1 != null)
            {
                _player1.AddToScore(10);
            }

            Debug.Log("Ship died from laser");
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _canFire = Time.maximumDeltaTime;
            Destroy(GetComponent<Collider2D>()); // destroys the collider so we don't play the effects again
            Destroy(this.gameObject, 2.8f);
        }
    }
}
