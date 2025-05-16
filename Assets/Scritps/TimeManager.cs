using TMPro;
using UnityEngine;

public class TicksSystem : MonoBehaviour
{
    public static TicksSystem instance;

    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private float remainingTime = 120f;
    [SerializeField] private float maxTime = 120f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
        }

        
        if (remainingTime <= 30f)
        {
            clockText.color = Color.red;
        }
        else if (remainingTime <= 60f)
        {
            clockText.color = Color.yellow;
        }
        else
        {
            clockText.color = Color.white;
        }

        
        if (remainingTime == 0f)
        {
            GameManager.instance.GameOver();
            clockText.enabled = false;
        }

        
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool TryUseTimeIncreaser(float amount)
    {
        if (remainingTime < maxTime)
        {
            remainingTime = Mathf.Clamp(remainingTime + amount, 0f, maxTime);
            return true;
        }

        return false;
    }
}