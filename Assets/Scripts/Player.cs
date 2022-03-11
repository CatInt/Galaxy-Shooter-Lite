using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    //public or private reference
    [SerializeField]
    private float _initialSpeed = 4.5f;
    [SerializeField]
    private float _speed = 0;
    [SerializeField]
    private float _speedUpFactor = 1.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private Vector3 _primaryLaserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField]
    private float _initialFireThreshold = 0.2f;
    [SerializeField]
    private float _fireThreshold = 0;
    [SerializeField]
    private float _fireThresholdUpFactor = 0.04f;
    [SerializeField]
    private float _nextFireThreshold = 0f;
    [SerializeField]
    private int _tripleShotActive = 0;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject _sheild;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _primaryEngineDamaged;
    [SerializeField]
    private GameObject _leftEngineDamaged;
    [SerializeField]
    private GameObject _rightEngineDamaged;

    [SerializeField]
    private AudioClip _laserFireSound;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private UiManager _uiManager;

    private Vector2 _move;

    private void GetManagers()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL!");
        }

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpwanManager is NULL!");
        }

        _uiManager = GameObject.Find("UI Manager").GetComponent<UiManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UiManager is NULL!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetManagers();
        StartNewGame();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (IsAlive() && context.phase == InputActionPhase.Started && Time.time > _nextFireThreshold)
        {
            FireLaser();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_lives > 0)
        {
            if (_move.sqrMagnitude > 0.01)
            {
                float horizontalInput = _move.x;
                float verticalInput = _move.y;
                CalculateMovement(horizontalInput, verticalInput);
            }
        }
    }

    private void StartNewGame()
    {
        ResetPosition();
        _score = 0;
        _lives = 3;
        _speed = _initialSpeed;
        _fireThreshold = _initialFireThreshold;
        _tripleShotActive = 0;
        _uiManager.UpdateScoreText(_score);
        _uiManager.UpdateLiveDisplay(_lives);
        // _spawnManager.StartSpawning();
        _thruster.SetActive(true);
        _primaryEngineDamaged.SetActive(false);
        _leftEngineDamaged.SetActive(false);
        _rightEngineDamaged.SetActive(false);
        _sheild.SetActive(false);
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void CalculateMovement(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        Vector3 pos = transform.position;
        Vector3 clippedPosY = new Vector3(pos.x, Mathf.Clamp(pos.y, -3.2f, 0), pos.z);
        transform.position = clippedPosY;
        if (pos.x >= 11)
        {
            transform.position = new Vector3(-8.5f, pos.y, pos.z);
        } else if (pos.x <= -11f)
        {
            transform.position = new Vector3(8.5f, pos.y, pos.z);
        }
    }

    void FireLaser()
    {
        _nextFireThreshold = Time.time + _fireThreshold;
        
        if (_tripleShotActive > 0)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _primaryLaserOffset, Quaternion.identity);
        }

        AudioSource.PlayClipAtPoint(_laserFireSound, transform.position);
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            ShieldOff();
            return;
        }
        _lives--;
        _uiManager.UpdateLiveDisplay(_lives);

        DamageEngine();

        if (IsDead())
        {
            GameOver();
        }
    }

    private void DamageEngine()
    {
        _rightEngineDamaged.SetActive(_lives < 3);
        _leftEngineDamaged.SetActive(_lives < 2);
        _primaryEngineDamaged.SetActive(_lives < 1);
    }

    private void StopSpawning()
    {
        _spawnManager.StopSpawning();
    }

    private void GameOver()
    {
        _thruster.SetActive(false);
        StopSpawning();
        ResetPosition();
        _gameManager.GameOver();
    }

    public void EnableTripleLaser()
    {
        _tripleShotActive ++;
        StartCoroutine(PowerOffRoutine(5f));
    }

    private IEnumerator PowerOffRoutine(float delayInSecond)
    {
        yield return new WaitForSeconds(delayInSecond);
        _tripleShotActive --;
    }

    public void SpeedUp()
    {
        _speed *= _speedUpFactor;
        _fireThreshold -= _fireThresholdUpFactor;
        StartCoroutine(SpeedDownRoutine(5f));
    }

    private IEnumerator SpeedDownRoutine(float delayInSecond)
    {
        yield return new WaitForSeconds(delayInSecond);
        _speed /= _speedUpFactor;
        _fireThreshold += _fireThresholdUpFactor;
    }

    public void ShieldOn()
    {
        _isShieldActive = true;
        _sheild.SetActive(true);
    }

    public void ShieldOff()
    {
        _isShieldActive = false;
        _sheild.SetActive(false);
    }

    public void Score(int s)
    {
        _score += s;
        _uiManager.UpdateScoreText(_score);
    }

    public bool IsDead()
    {
        return _lives < 1;
    }

    public bool IsAlive()
    {
        return _lives > 0;
    }
}
