using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

public class TimerUIController : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] Image timerFill;

    ITimeService _timeService;
    float _initialTime;

    [Inject]
    public void Construct(ITimeService timeService)
    {
        _timeService = timeService;
    }

    void Start()
    {
        _timeService.OnTick
            .Subscribe(OnTick)
            .AddTo(this);

        _timeService.OnTimeUp
            .Subscribe(_ => OnTimeUp())
            .AddTo(this);

        _initialTime = 60f;                  
        _timeService.StartCountdown(_initialTime);
    }

    void OnTick(float remaining)
    {
        timerText.text = $"{Mathf.CeilToInt(remaining)}s";

        if (_initialTime > 0)
            timerFill.fillAmount = remaining / _initialTime;
    }

    void OnTimeUp()
    {
        Debug.LogWarning("Time is up! Game Over.");
    }
}
