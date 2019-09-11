using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framerater.StressTest
{
    public class Neutron : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Rigidbody _rigidbody;
#pragma warning restore CS0649

        public float AttractionForce { get; set; }

        protected virtual void FixedUpdate()
        {
            _rigidbody.AddForce(transform.localPosition * -AttractionForce);
        }
    }
}