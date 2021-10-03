using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiScript : MonoBehaviour
{
    public int money;
    public int research;
    public int currentEngineCount;
    public int maxEngineCount;
    public int requiredMoney;
    public int requiredMoneyStep;
    public EngineInfoScript engineInfo;
    public int currentEngineLevel;
    public int requiredResearch;
    public float currentEngineAltitude;
    public PlatformRootScript platformRoot;
    public StormScript storm;

    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI accelerationText;
    public TextMeshProUGUI massText;
    public TextMeshProUGUI liftText;
    public TextMeshProUGUI dragText;
    public TextMeshProUGUI pressureText;
    public TextMeshProUGUI stormPushText;

    public TextMeshProUGUI moneyText;
    public Image moneyBar;
    public TextMeshProUGUI researchText;
    public Image researchBar;

    public Image journeyBar;
    public Image engineAltitudeLine;
    public Image stormTopLine;
    public Image stormBottomLine;

    private float goalAltitude = 100000.0f;

    // Start is called before the first frame update
    void Start()
    {
        requiredResearch = engineInfo.engineResearchArray[currentEngineLevel + 1];
        currentEngineAltitude = engineInfo.engineAltitudeArray[currentEngineLevel];
    }

    private void UpdatePlatform()
    {
        altitudeText.text = "Altitude: " + ((int) platformRoot.altitude).ToString() + " m";
        speedText.text = "Speed: " + ((float) ((int) (platformRoot.speed * 100.0f)) / 100).ToString() + " m/s"; // 2 decimal places
        accelerationText.text = "Acceleration: " + ((float) ((int) (platformRoot.acceleration * 100.0f)) / 100).ToString() + " m/s^2"; // 2 decimal places
        massText.text = "Mass: " + ((int) (platformRoot.mass * 1000.0f)).ToString() + " kg";
        liftText.text = "Lift: " + ((int) (platformRoot.lift * 1000.0f)).ToString() + " N";
        dragText.text = "Drag: " + ((int) (platformRoot.drag * 1000.0f)).ToString() + " N";
        pressureText.text = "Pressure: " + ((int) (platformRoot.pressure * 1000.0f)).ToString() + " Pa";
    }

    private void UpdateMoney()
    {
        if (currentEngineCount < maxEngineCount)
        {
            if (money >= requiredMoney)
            {
                money = money - requiredMoney;
                currentEngineCount = currentEngineCount + 1;
                requiredMoney = requiredMoney + requiredMoneyStep;
                platformRoot.AddEngine();
            }
            moneyText.text = "Engine " + (currentEngineCount + 1).ToString() + " (" + money.ToString() + " / " + requiredMoney.ToString() + ")";
            float fraction = Mathf.Min((float) money / (float) requiredMoney, 1.0f);
            moneyBar.rectTransform.transform.localScale = new Vector3(fraction, 1.0f, 1.0f);
        }
        else
        {
            moneyText.text = "MAX ENGINES";
            moneyBar.rectTransform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void UpdateResearch()
    {
        if (currentEngineLevel < engineInfo.engineNameArray.Length - 1)
        {
            if (research >= requiredResearch)
            {
                research = research - requiredResearch;
                currentEngineLevel = currentEngineLevel + 1;
                if (currentEngineLevel < engineInfo.engineNameArray.Length - 1)
                {
                    requiredResearch = engineInfo.engineResearchArray[currentEngineLevel + 1];
                }
                currentEngineAltitude = engineInfo.engineAltitudeArray[currentEngineLevel];
                platformRoot.ChangeAllEngines();
            }
            researchText.text = engineInfo.engineNameArray[currentEngineLevel + 1] + " (" + research.ToString() + " / " + requiredResearch.ToString() + ")";
            float fraction = Mathf.Min((float) research / (float) requiredResearch, 1.0f);
            //researchBar.rect.scale.x = fraction;
            //researchBar.style.width.value = fraction;
            researchBar.rectTransform.transform.localScale = new Vector3(fraction, 1.0f, 1.0f);
        }
        else
        {
            researchText.text = "MAX RESEARCH";
            researchBar.rectTransform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void UpdateJourney()
    {
        stormPushText.text = "Storm Push: " + ((float)((int)(storm.stormPressure * 100.0f)) / 100).ToString() + " m/s^2"; // 2 decimal places
        journeyBar.rectTransform.transform.localScale = new Vector3(1.0f, (platformRoot.altitude / goalAltitude), 1.0f);
        engineAltitudeLine.rectTransform.transform.localPosition = new Vector3(-8.0f, 500.0f * ((currentEngineAltitude / goalAltitude) - 0.5f), -1.5f);
        stormTopLine.rectTransform.transform.localPosition = new Vector3(-8.0f, 500.0f * ((storm.stormTopAltitude / goalAltitude) - 0.5f), -1.5f);
        if (storm.stormBottomAltitude > 0.0f)
        {
            stormBottomLine.rectTransform.transform.localPosition = new Vector3(-8.0f, 500.0f * ((storm.stormBottomAltitude / goalAltitude) - 0.5f), -1.5f);
        }
        else
        {
            stormBottomLine.rectTransform.transform.localPosition = new Vector3(-8.0f, -5000.0f, -1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate is called once per physics step
    void FixedUpdate()
    {

    }

    // LateUpdate is used for GUI stuff. I don't know why.
    void LateUpdate()
    {
        UpdatePlatform();
        UpdateMoney();
        UpdateResearch();
        UpdateJourney();
        if (platformRoot.altitude >= goalAltitude) // Victory Royale.
        {
            SceneManager.LoadScene("SpaceScene");
        }
        else if (platformRoot.altitude < storm.stormBottomAltitude) // Defeat.
        {
            SceneManager.LoadScene("StormScene");
        }
    }
}
