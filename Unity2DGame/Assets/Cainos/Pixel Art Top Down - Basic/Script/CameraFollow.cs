using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        private Transform target;
        public float lerpSpeed = 1.0f;

        //private Vector3 offset;

        private Vector3 targetPos;

        public Transform Target
        {
            set
            {
                target = value;
                transform.position = target.position;
            }
        }

        /*private void FixedUpdate()
        {
            
        }*/
        private void LateUpdate()
        {
            if (target == null) return;

            targetPos = target.position;// + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
