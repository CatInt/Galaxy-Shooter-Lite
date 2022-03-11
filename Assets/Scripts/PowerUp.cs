using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    // 0 for Triple Laser
    // 1 for Speed Up
    // 2 for Shied
    [SerializeField]
    private int _ID = 0;

    private Player _player;

    [SerializeField]
    private AudioClip _powerUpAudio;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -9 || _player.IsDead())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            { 
                ApplyPowerUp(_player);
            }
            Destroy(this.gameObject);
        }
    }

    private void ApplyPowerUp(Player player)
    {
        AudioSource.PlayClipAtPoint(_powerUpAudio, transform.position);
        switch (_ID)
        {
            case 0:
                player.EnableTripleLaser();
                break;
            case 1:
                player.SpeedUp();
                break;
            case 2:
                player.ShieldOn();
                break;
            default:
                break;
        }
    }
}
