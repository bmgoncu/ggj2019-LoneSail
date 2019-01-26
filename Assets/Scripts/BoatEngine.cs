using UnityEngine;
using System.Collections;

public class BoatEngine : MonoBehaviour 
{
    //Drags
    public Transform waterJetTransform;
    public Transform SailMast;

    //How fast should the engine accelerate?
    public float powerFactor;

    //What's the boat's maximum engine power?
    public float maxPower;

    private float _tempMaxPower;

    //The boat's current engine power is public for debugging
    public float currentJetPower;

    private float thrustFromWaterJet = 0f;

    private Rigidbody boatRB;

    public float WaterJetRotation_Y = 0f;
    public float WaterJetTurnSpeed = 15f;

    BoatController boatController;

    void Start() 
    {
        _tempMaxPower = maxPower;
        boatRB = GetComponent<Rigidbody>();
        boatController = GetComponent<BoatController>();
    }


    void Update() 
    {
        UserInput();
    }

    void FixedUpdate()
    {
        UpdateWaterJet();
    }

    void UserInput()
    {

        float deltaY = WaterJetTurnSpeed * Time.deltaTime;
        //Forward / reverse
        if (Input.GetKey(KeyCode.W))
        {
            if (boatController.CurrentSpeed < 50f && currentJetPower < maxPower)
            {
                currentJetPower += 1f * powerFactor;
            }
        }
        else
        {
            currentJetPower = 0f;
        }

        //Steer left
        if (Input.GetKey(KeyCode.A))
        {
            maxPower = _tempMaxPower / 3;
            if (currentJetPower > maxPower)
                currentJetPower = maxPower;
            WaterJetRotation_Y = ((waterJetTransform.localEulerAngles.y+360f)%360f) + deltaY;

            if (WaterJetRotation_Y > 30f && WaterJetRotation_Y < 180f)
            {
                WaterJetRotation_Y = 30f;
            } else if (WaterJetRotation_Y > 180f && WaterJetRotation_Y < 330f) {
                WaterJetRotation_Y = 330f;
            }

            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

            waterJetTransform.localEulerAngles = newRotation;
        }
        //Steer right
        else if (Input.GetKey(KeyCode.D))
        {
            maxPower = _tempMaxPower / 3;
            if (currentJetPower > maxPower)
                currentJetPower = maxPower;
            WaterJetRotation_Y = ((waterJetTransform.localEulerAngles.y+360f)%360f) - deltaY;

            if (WaterJetRotation_Y < 330f && WaterJetRotation_Y > 180f)
            {
                WaterJetRotation_Y = 330f;
            } else if (WaterJetRotation_Y < 180f && WaterJetRotation_Y > 30) {
                WaterJetRotation_Y = 30f;
            }

            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

            waterJetTransform.localEulerAngles = newRotation;
        } else {
            if (waterJetTransform.localEulerAngles.y >= 329f)
            {
                WaterJetRotation_Y = ((waterJetTransform.localEulerAngles.y + 360f) % 360f) + deltaY;
                maxPower = _tempMaxPower / 3;
            }
            else if (waterJetTransform.localEulerAngles.y <= 31f && waterJetTransform.localEulerAngles.y > 0f)
            {
                WaterJetRotation_Y = ((waterJetTransform.localEulerAngles.y + 360f) % 360f) - deltaY;
                maxPower = _tempMaxPower / 3;
            }
            else
                maxPower = _tempMaxPower;

            if (Mathf.Abs(WaterJetRotation_Y - 360f) < 0.1f)
                WaterJetRotation_Y = 0f;
            Vector3 newRotation = new Vector3(0f, WaterJetRotation_Y, 0f);

            waterJetTransform.localEulerAngles = newRotation;
        }

        float sailAngle = WaterJetRotation_Y;
        if (WaterJetRotation_Y > 180f)
        {
            sailAngle = WaterJetRotation_Y -360;
        }
        SailMast.localEulerAngles = Vector3.up * sailAngle / 2;

    }

    void UpdateWaterJet()
    {
        //Debug.Log(boatController.CurrentSpeed);

        Vector3 forceToAdd = -waterJetTransform.forward * currentJetPower;

        //Only add the force if the engine is below sea level
        float waveYPos = WaterController.current.GetWaveYPos(waterJetTransform.position, Time.time);

        if (waterJetTransform.position.y < waveYPos)
        {
            boatRB.AddForceAtPosition(forceToAdd, waterJetTransform.position);
        }
        else
        {
            boatRB.AddForceAtPosition(Vector3.zero, waterJetTransform.position);
        }
    }
}