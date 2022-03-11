using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(0, 1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y > 10)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }
    }
}
