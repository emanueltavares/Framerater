using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.StressTest
{
    public class Neutron : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public float AttractionForce { get; set; }

        protected virtual void FixedUpdate()
        {
            _rigidbody.AddForce(transform.localPosition * -AttractionForce);
        }
    }
}