using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTypeSC : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _stdMass = 1f;
    private float _stdScale;
    private Vector3 _initPosition;
    private float _positionAttraction = 20f;
    private float _drag = 2f;
    private Color _defaultColor;
    private Renderer _renderer;
    private float _attractTimeFreq = 1f;
    

    void Interact(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }

    void AttractToOrigin()
    {
        Vector3 distVec = this.transform.position - _initPosition;
        if (distVec.sqrMagnitude > .1f)
        {
            Interact(- _positionAttraction * Time.fixedDeltaTime * distVec);
        }
    }

    void IntensityColor(float intensity)
    {
        _renderer.material.SetColor("_EmissionColor", intensity * _defaultColor);
    }


    IEnumerator OnHitFadeIn()
    {   
        float peakIntensity = 2f;
        float t = 0;
        while (t < peakIntensity)
        {
            IntensityColor(t);
            t += Time.fixedDeltaTime;
            yield return null;
        }

        while (t > 1f)
        {
            IntensityColor(t);
            t -= Time.fixedDeltaTime;
            yield return null;
        }


    }
    void OnCollisionEnter(Collision collision)
    {
        if ( collision.collider.gameObject.layer == 6)
        {
            StartCoroutine("OnHitFadeIn");
        }
        

        
    }
    private void Awake() 
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.mass = _stdMass;
        _rigidbody.drag = _drag;
        _rigidbody.freezeRotation = true;
        _stdScale = this.gameObject.transform.localScale.x;
        _initPosition = this.gameObject.transform.position;

        _renderer = this.GetComponent<Renderer>();
        _renderer.material.EnableKeyword("_EMISSION");
        _defaultColor = _renderer.material.GetColor("_EmissionColor");
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        AttractToOrigin();
        //_rigidbody.AddForce( - _positionAttraction * Time.fixedDeltaTime * (this.transform.position - _initPosition));
        
    }
}
