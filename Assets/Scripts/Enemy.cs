using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private bool _isDead = false;

    private Player _player;
    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
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
        if (_isDead)
        {
            return;
        } else if (other.tag == "Player")
        {
            if (_player != null)
            { 
                _player.Damage();  
            }
            SelfDestroy();
        } else if (other.tag == "Laser")
        {
            if (_player != null)
            { 
                _player.Score(10);  
            }
            Destroy(other.gameObject);
            SelfDestroy();
        }
    }

    private void SelfDestroy()
    {
        _isDead = true;
        AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
        Destroy(this.gameObject, 3f);
        _animator.SetTrigger("OnEnemyDeath");
    }
}
