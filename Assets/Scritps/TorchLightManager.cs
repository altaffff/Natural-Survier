using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TorchLightManager : MonoBehaviour
{
    [SerializeField] private Slider torchlightBar;
    [SerializeField] private float torchMaxRange;
    [SerializeField] private float torchMinRange;
    [SerializeField] private float torchFlickerRange;
    [SerializeField] private Light flashLight;
    [SerializeField] private float torchDrainRate;
    [SerializeField] private float currentTorchValue;
    
    
    private bool canOn;
    private bool wasFKeyPressed;
    
    private void OnValidate()
    {
        if (torchlightBar == null)
        {
            torchlightBar = GetComponent<Slider>();
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        canOn = false;
        torchMaxRange = 1f;
        torchMinRange = 0f;
        torchFlickerRange = 0.1f;
        currentTorchValue = torchMaxRange;
        flashLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F) && currentTorchValue > torchMinRange)
        {
            canOn = !canOn;
        }

        if (canOn)
        {
            TorchLight();
        }
        
    }

    void TorchLight()
    {
        if (canOn)
        {
            
        if (currentTorchValue > torchFlickerRange)
        {
            currentTorchValue -=  torchDrainRate * Time.deltaTime;
            torchlightBar.value = currentTorchValue/torchMaxRange;
            flashLight.enabled = true;
            Debug.Log("Flashlight is turning on ");
        }

        else if (currentTorchValue > torchMinRange)
        {
            currentTorchValue -= torchDrainRate * Time.deltaTime;
            torchlightBar.value = currentTorchValue/torchMaxRange;
            StartCoroutine("TorchFlickerEffect");
            Debug.Log("Flicker Effect");
        }

        else
        {
            StopAllCoroutines();
            flashLight.enabled = false;
            Debug.Log("Flashlight is turned off");
        }
        }
    }

    IEnumerator TorchFlickerEffect()
    {
        while (true)
        {
            
            flashLight.enabled = true;
            yield return new WaitForSeconds(0.3f);
            flashLight.enabled = false;
    
            yield return new WaitForSeconds(0.5f);
            flashLight.enabled = true;
        }
    }

    public void HideSlider()
    {
        if (GameManager.instance.isGameOver)
        {
            torchlightBar.enabled = false;
            torchlightBar.gameObject.SetActive(false);
            canOn = false;
        }
        
    }
    

}
