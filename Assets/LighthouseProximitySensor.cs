using UnityEngine;

public class LighthouseProximitySensor : MonoBehaviour
{
    public GameObject target;
    public float triggerDistanceToTarget = 100f;

    public bool isTriggered = false;

    public GameObject nextLighthouse;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            destroyLighthouse();
            return;
        }

        Vector3 targetPos = target.transform.position;
        Vector3 thisPos = this.transform.position;

        float distance = (targetPos - thisPos).magnitude;

        if (triggerDistanceToTarget > distance)
        {
            isTriggered = true;
            createNextLighthouse();
        }
    }

    private void createNextLighthouse()
    {
        nextLighthouse.SetActive(true);
    }

    private void destroyLighthouse()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - Time.deltaTime, pos.z);
    }
}
