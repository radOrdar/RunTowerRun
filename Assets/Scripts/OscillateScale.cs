using System.Collections;
using UnityEngine;

public class OscillateScale : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float period = 2f;
    
    private Transform _t;
    private float _periodFactor;
    
    private void OnEnable()
    {
        _t = transform;
        _periodFactor = 2 * Mathf.PI / period;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            _t.localScale = (1f + amplitude * Mathf.Sin(_periodFactor * Time.time )) * Vector3.one;
            yield return null;
        }
    }
}
