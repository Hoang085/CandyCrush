using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
    public class RotateScript : MonoBehaviour
    {
        [Range(-500, 0)]public float speed = 0f;
        [SerializeField]float maxSpeed;
        [SerializeField] float minSpeed;
        [SerializeField] bool randomSpeed;

        private void Awake()
        {
            if (randomSpeed)
                speed = Random.Range(minSpeed, maxSpeed);
        }

        private void FixedUpdate()
        {
            Vector3 rot = gameObject.transform.eulerAngles;
            rot.z = rot.z + speed * Time.fixedDeltaTime;
            gameObject.transform.eulerAngles = rot;
        }
    }
}
