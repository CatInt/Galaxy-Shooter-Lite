using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 45f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioClip _explosionSound;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::SpwanManager is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // float step = ;
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
            Destroy(explosion.gameObject, 2f);
        }
    }
}
