﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _quadripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    private float _canFirePlayerTwo = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedPowerupActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    private GameManager _gameManager;

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;


    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is null.");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI manager is null.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio source on the player is null.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if(_gameManager == null)
        {
            Debug.LogError("Game manager is null.");
        }

        if (!_gameManager.isCoOpMode)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOne)
        {
            CalculateMovement();

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && isPlayerOne)
            {
                FireLaser();
            }
        }
        
        if (isPlayerTwo)
        {
            CalculateMovementPlayerTwo();

            if(Input.GetKeyDown(KeyCode.KeypadEnter) && Time.time > _canFirePlayerTwo && isPlayerTwo)
            {
                FireLaser();
            }
        }
        

    }

    void CalculateMovement()
    {
        float hoizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(hoizontalInput, verticalInput, 0);
        if (!_isSpeedPowerupActive)
        {
            transform.Translate(direction * _speed * Time.deltaTime);

        }
        else
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void CalculateMovementPlayerTwo()
    {
        if (_isSpeedPowerupActive)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * _speedMultiplier * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * _speedMultiplier * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * _speedMultiplier * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * _speedMultiplier * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        // Cool down for firing
        _canFire = Time.time + _fireRate;
        // Quaternion.identity ==> default rotation

        if (_isTripleShotActive)
        {
            var adjustVec = new Vector3(-0.7f, 1.05f, 0);
            Instantiate(_tripleShotPrefab, transform.position + adjustVec, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }


    public void Damage()
    {
        if (_lives < 1)
        {
            return;
        }

        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1) 
        {
            _spawnManager.OnPlayerDeath(); // turns off enemy spawning if the player dies
            Destroy(this.gameObject); 
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShowPowerDownRoutine());
    }

    IEnumerator TripleShowPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedPowerupActive()
    {
        _isSpeedPowerupActive = true;
        StartCoroutine(SpeedPowerupPowerDownRoutine());
    }

    IEnumerator SpeedPowerupPowerDownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedPowerupActive = false;
    }

    public void ShieldPowerupActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
