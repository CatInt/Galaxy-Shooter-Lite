using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Text _gameOver;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private bool _isGameOver = false;

    [SerializeField]
    private GameObject _virtualStick;

    [SerializeField]
    private GameObject _virtualFireButton;

    [SerializeField]
    private GameObject _virtualRButton;

    [SerializeField]
    private bool _showVirtualControls = false;

    void Awake()
    {
        #if UNITY_ANDROID
            _showVirtualControls = true;
        #else
        #endif
        _virtualStick.SetActive(_showVirtualControls);
        _virtualFireButton.SetActive(_showVirtualControls);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText(0);
        UpdateLiveDisplay(3);
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLiveDisplay(int lives)
    {
        _livesImage.sprite = _liveSprites[lives];
        if (lives == 0)
        {
            GameOver();
        } else if (_isGameOver)
        {
            GameStart();
        }
    }

    private void GameOver()
    {
        _isGameOver = true;
        StartCoroutine(GameOverFlashingRoutine());
        _restartText.gameObject.SetActive(true);
        _virtualRButton.SetActive(_showVirtualControls);
    }

    private IEnumerator GameOverFlashingRoutine()
    {
        while (_isGameOver)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.6f);
        }
    }

    private void GameStart()
    {
        _isGameOver = false;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _virtualRButton.SetActive(false);
    }
}
