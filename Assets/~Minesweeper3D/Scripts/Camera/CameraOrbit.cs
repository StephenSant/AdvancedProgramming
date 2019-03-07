using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper3D
{
    public class CameraOrbit : MonoBehaviour
    {
        public float zoomSpeed = 5,
                     xSpeed = 120, ySpeed = 120,
                     yMin = -80, yMax = 80,
                     distanceMin = 10, distanceMax = 15;
        private float x, y,
                      distance;

        void Start()
        {
            distance = distanceMax;
        }

        void LateUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                x += mouseX * xSpeed * Time.deltaTime;
                y -= mouseY * ySpeed * Time.deltaTime;
                y = Mathf.Clamp(y, yMin, yMax);

            }
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            distance -= scrollWheel * zoomSpeed;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            transform.rotation = Quaternion.Euler(y, x, 0);
            transform.position = -transform.forward * distance;
        }
    }
}