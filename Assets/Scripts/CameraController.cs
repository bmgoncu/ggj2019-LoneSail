using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Ship;

    Transform _transform;

    public float Radius;

    public float RotationSpeed;
    public float RadiusSpeed;
    public Vector3 CameraModeOffset;
    public float CameraModeTransitionDuration = 2;
    public RectTransform TopLetterBox;
    public RectTransform BottomLetterBox;
    public bool CameraModeOn = false;

    private Vector3 _offset = new Vector3(0f, 5f, 10f);


    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraModeOn) {
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

    public void Activate1(Transform destroying, Action onDestroyingFinnished) 
    {
        CameraModeOn = true;
        Sequence destroySequence = DOTween.Sequence();
        destroySequence.Insert(0, TopLetterBox.DOAnchorPosY(0, CameraModeTransitionDuration).SetEase(Ease.Linear));
        destroySequence.Insert(0, BottomLetterBox.DOAnchorPosY(0, CameraModeTransitionDuration).SetEase(Ease.Linear));
        destroySequence.Insert(0, transform.DOMove(destroying.position + CameraModeOffset, CameraModeTransitionDuration).SetEase(Ease.Linear));
        destroySequence.Insert(0, transform.DOMove(destroying.position + CameraModeOffset, CameraModeTransitionDuration).SetEase(Ease.Linear));
        destroySequence.Insert(0, transform.DORotate(Vector3.zero, CameraModeTransitionDuration));
        destroySequence.AppendCallback(() => onDestroyingFinnished());
    }

    public void Activate2(Transform newOne, Action onNewOneFinnished)
    {
        CameraModeOn = true;

        float angle = Mathf.Atan2(Ship.transform.forward.z, Ship.transform.forward.x);
        Vector3 destination = Ship.position + new Vector3(Radius * Mathf.Cos(angle), 5f, Radius * Mathf.Sin(angle));

        Sequence destroySequence = DOTween.Sequence();
        destroySequence.Append(transform.DOLookAt(newOne.position, CameraModeTransitionDuration, AxisConstraint.Y));
        destroySequence.Append(transform.DOMove(destination, CameraModeTransitionDuration));
        destroySequence.Append(transform.DOLookAt(Ship.position, 1f));
        destroySequence.AppendCallback(() => onNewOneFinnished());
    }

    public void Release() {

        Sequence seq = DOTween.Sequence();
        seq.Insert(0, TopLetterBox.DOAnchorPosY(50, CameraModeTransitionDuration).SetEase(Ease.Linear));
        seq.Insert(0, BottomLetterBox.DOAnchorPosY(-50, CameraModeTransitionDuration).SetEase(Ease.Linear));
        CameraModeOn = false;
    }
}
