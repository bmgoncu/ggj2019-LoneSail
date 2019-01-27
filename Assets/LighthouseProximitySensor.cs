using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LighthouseProximitySensor : MonoBehaviour
{
    public GameObject target;
    public float triggerDistanceToTarget = 100f;

    public bool isTriggered = false;

    public GameObject nextLighthouse;

    public string Title;
    public string Desc;

    public Text UITitle;
    public Text UIDesc;

    public AudioSource Horn;
    private AudioSource Thunder;
    
    // Start is called before the first frame update
    void Start()
    {
        Thunder = GetComponent<AudioSource>();
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
        Horn.Play();
        UIDesc.text = Desc;
        UITitle.text = Title;
        Sequence seq = DOTween.Sequence();

        seq.Append(UITitle.GetComponent<RectTransform>().DOAnchorPosX(-50f, 0f));
        seq.Insert(1, UITitle.DOFade(1f, 1f));
        seq.Insert(1, UITitle.GetComponent<RectTransform>().DOAnchorPosX(75f, 1f).SetEase(Ease.OutSine));

        seq.Insert(1, UIDesc.GetComponent<RectTransform>().DOAnchorPosY(-100f, 0f).SetEase(Ease.OutSine));
        seq.Insert(1, UIDesc.DOFade(1f, 1f).SetDelay(0.4f));
        seq.Insert(1, UIDesc.GetComponent<RectTransform>().DOAnchorPosY(0f, 1f).SetDelay(0.4f));

        seq.AppendCallback(() => Thunder.Play());
        seq.Insert(4, transform.DOMove(transform.position - Vector3.up * 40f, 3f).SetEase(Ease.OutSine));
        seq.Insert(4, Camera.main.transform.DOShakePosition(3f).SetEase(Ease.OutSine));
        seq.AppendCallback(()=> Destroy(gameObject));
        seq.Append(UITitle.DOFade(0f, 1f));
        seq.Append(UIDesc.DOFade(0f, 1f));
        seq.OnComplete(() => {
            if (nextLighthouse != null)
            {
                createNextLighthouse();
                Camera.main.GetComponent<CameraController>().Activate2(nextLighthouse.transform, () => {
                    Camera.main.GetComponent<CameraController>().Release();
                });
            }
            else {
                SceneManager.LoadScene(0);
            }
        });


    }
}
