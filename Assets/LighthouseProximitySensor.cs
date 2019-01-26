using DG.Tweening;
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
            return;
        }

        Vector3 targetPos = target.transform.position;
        Vector3 thisPos = this.transform.position;

        float distance = (targetPos - thisPos).magnitude;

        if (triggerDistanceToTarget > distance)
        {
            isTriggered = true;
            Camera.main.GetComponent<CameraController>().Activate1(transform,() => {
                destroyLighthouse();
            });
        }
    }

    private void createNextLighthouse()
    {
        nextLighthouse.SetActive(true);
    }

    private void destroyLighthouse()
    {
        transform.DOMove(transform.position - Vector3.up * 40f, 2f).SetEase(Ease.OutSine).OnComplete(() => {
            if (nextLighthouse != null) {
                createNextLighthouse();
                Camera.main.GetComponent<CameraController>().Activate2(nextLighthouse.transform, () => {
                    Camera.main.GetComponent<CameraController>().Release();
                });
            }
        });
    }
}
