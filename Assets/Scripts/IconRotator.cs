using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class IconRotator : MonoBehaviour
    {

        public float rotationSpeed = 5f;

        void Update()
        {
            transform.Rotate(0, 1 * rotationSpeed, 0);
        }
    }

