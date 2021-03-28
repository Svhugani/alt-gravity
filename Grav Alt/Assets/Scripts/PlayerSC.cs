using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSC : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private float _stdMass = 1f;
    private float _stdScale;
    private Vector3 _initPosition;
    private float _positionAttraction = 100f;
    private float _drag = .3f;
    private bool _isForceAccumulated = false;
    private float _accumulatedForce = 0f;
    private float _pushForce = 100f;
    private float _applyForce = 0f;
    private Vector3 _applyForceDirection;
    
    public void ScaleTo(float scale)
    {
        this.transform.localScale = scale * this.transform.localScale;
    }
    void Interact(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }

    void ManualControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("TOUCHED THE SCREEN");
                _isForceAccumulated = true;

            }

            else if (touch.phase == TouchPhase.Stationary)
            {
                Debug.Log("STILL PUSHING");
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("FINGER LIFTED");
                _isForceAccumulated = false;

            }
        }
    }

    void ManualControl2()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Stationary)
            {
                _accumulatedForce += _pushForce * Time.deltaTime;
                Debug.Log("Increasing force");
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                
                Debug.Log("Stopped increasing force");
                Debug.Log("Applied Force: " + _accumulatedForce.ToString());
                _applyForceDirection = Camera.main.ViewportToWorldPoint(new Vector3(touch.position.x / Screen.width, touch.position.y / Screen.height, Mathf.Abs(Camera.main.transform.position.z)));
                _applyForceDirection -= _rigidbody.position;
                _applyForce = _accumulatedForce;
                _accumulatedForce = 0;
                //Interact(_accumulatedForce * (screenToWorldPoint - _rigidbody.position));
            }

            

            

        }
    }


    private void Awake() 
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.mass = _stdMass;
        _rigidbody.drag = _drag;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        _stdScale = this.gameObject.transform.localScale.x;
        _initPosition = this.transform.position;
    }
    void Start()
    {
        
    }

    void Update()
    {
        ManualControl2();
    }

    void FixedUpdate()
    {
        //_rigidbody.AddForce(_positionAttraction * Time.fixedDeltaTime * (this.transform.position - _initPosition));
        //if(Random.Range(0, 1f) < 0.04)
        //{
            //Interact(20 * _positionAttraction * new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
        //}
        if( _applyForce > 0 )
        {
            Interact( _applyForce * Mathf.Log( 1 + _applyForce) * _applyForceDirection);
            _applyForce = 0;
        }


        
    }
}
