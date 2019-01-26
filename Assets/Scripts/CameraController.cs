using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Ship;

    Transform _transform;

    public float Radius;

    public float RotationSpeed;
    public float RadiusSpeed;

    private Vector3 _offset = new Vector3(0f, 5f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(Ship.transform.forward.z, Ship.transform.forward.x);
        //angle += Mathf.PI;
        Vector3 destination = Ship.position + new Vector3(Radius * Mathf.Cos(angle), 5f, Radius * Mathf.Sin(angle));
        _transform.position = Vector3.Lerp(_transform.position, destination, RadiusSpeed * Time.deltaTime);
        //_transform.rotation = Quaternion.Lerp(_transform.rotation, Ship.rotation, RotationSpeed * Time.deltaTime);
        Vector3 focus = Ship.position;
        focus.y = _transform.position.y;
        _transform.LookAt(focus);
    }
}
