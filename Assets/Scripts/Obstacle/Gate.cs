using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class Gate : MonoBehaviour
    {
        public event Action OnChecked;

        public List<ParticleSystem> FrameFxs;

        private bool _isChecked;

        private void Awake()
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(5, 50, 1);
            boxCollider.center = Vector3.up * 0.5f;
            gameObject.layer = LayerMask.NameToLayer("Gates");

            var gatePass = new GameObject("GatePass").AddComponent<GatePass>();
            gatePass.transform.parent = transform;
            gatePass.transform.localPosition = Vector3.forward * 6;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isChecked) 
                return;

            foreach (var fx in FrameFxs)
            {
                fx.Play();
            }
            OnChecked?.Invoke();
            _isChecked = true;
        }
    }
}