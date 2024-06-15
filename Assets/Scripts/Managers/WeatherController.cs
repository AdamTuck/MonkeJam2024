using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DigitalRuby.WeatherMaker;

public class WeatherController : MonoBehaviour
{
    [SerializeField] DateTime inGameTime;
    [SerializeField] private string currentPrecipitation;
    [SerializeField] private float currentVisibility;
    [SerializeField] private float currentWindSpeed;
    [SerializeField] private float currentCloudinessLevel;


    // Singleton
    public static WeatherController Instance;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        RollRandomWeather();
    }

    public void RollRandomWeather()
    {
        float randomWeatherRoll = UnityEngine.Random.Range(0f, 1f);
        float randomCloudiness = UnityEngine.Random.Range(0f, 49f);

        int currentMonth = DigitalRuby.WeatherMaker.WeatherMakerDayNightCycleManagerScript.Instance.DateTime.Month;

        if (randomWeatherRoll > 0.8f)
            UpdateWeathermaker("Rain", 10000, 25, 70);
        else if (randomWeatherRoll > 0.7f)
            UpdateWeathermaker("Drizzle", 10000, 10, 50);
        else
            UpdateWeathermaker("None", 10000, 0, randomCloudiness);
    }

    public void UpdateWeathermaker(string precipitationType, float visibility, float windSpeed, float cloudinessLevel)
    {
        //Debug.Log($"Setting Weather: {precipitationType}, {visibility}");

        currentPrecipitation = precipitationType;
        currentVisibility = visibility;
        currentWindSpeed = windSpeed;
        currentCloudinessLevel = cloudinessLevel;

        //snowSurface.SetActive(false);

        // Precipitation
        if (precipitationType == "Rain")
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = (WeatherMakerPrecipitationType.Rain);
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = 0.5f;
            //SoundManager.instance.SetPrecipitationSound("heavyRain");
        }
        else if (precipitationType == "Drizzle")
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = (WeatherMakerPrecipitationType.Rain);
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = 0.2f;
            //SoundManager.instance.SetPrecipitationSound("lightRain");
        }
        else if (precipitationType == "Snow")
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = (WeatherMakerPrecipitationType.Snow);
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = 0.4f;
            //SoundManager.instance.SetPrecipitationSound("none");
            //snowSurface.SetActive(true);
            //snowMeltTimer = daysUntilSnowMelt;
        }
        else if (precipitationType == "None")
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = (WeatherMakerPrecipitationType.None);
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = 0f;
            //SoundManager.instance.SetPrecipitationSound("none");
        }

        //Fog
        if (visibility <= 1000)
        {
            WeatherMakerFullScreenFogScript.Instance.TransitionFogDensity(0, 0.2f, 2);
        }
        else if (visibility <= 3000)
        {
            WeatherMakerFullScreenFogScript.Instance.TransitionFogDensity(0, 0.1f, 2);
        }
        else if (visibility <= 6000)
        {
            WeatherMakerFullScreenFogScript.Instance.TransitionFogDensity(0, 0.05f, 2);
        }
        else if (visibility <= 9000)
        {
            WeatherMakerFullScreenFogScript.Instance.TransitionFogDensity(0, 0.01f, 2);
        }
        else
        {
            WeatherMakerFullScreenFogScript.Instance.TransitionFogDensity(0, 0, 0);
        }

        // Wind
        if (windSpeed > 30)
        {
            WeatherMakerWindScript.Instance.SetWindProfileAnimated(WeatherMakerScript.Instance.LoadResource<WeatherMakerWindProfileScript>("WeatherMakerWindProfile_HeavyWind"), 0.0f, 5.0f);
        }
        else if (windSpeed > 20)
        {
            WeatherMakerWindScript.Instance.SetWindProfileAnimated(WeatherMakerScript.Instance.LoadResource<WeatherMakerWindProfileScript>("WeatherMakerWindProfile_MediumWind"), 0.0f, 5.0f);
        }
        else if (windSpeed > 10)
        {
            WeatherMakerWindScript.Instance.SetWindProfileAnimated(WeatherMakerScript.Instance.LoadResource<WeatherMakerWindProfileScript>("WeatherMakerWindProfile_LightWind"), 0.0f, 5.0f);
        }
        else
        {
            WeatherMakerWindScript.Instance.SetWindProfileAnimated(WeatherMakerScript.Instance.LoadResource<WeatherMakerWindProfileScript>("WeatherMakerWindProfile_None"), 0.0f, 5.0f);
        }


        // Cloudiness
        if (cloudinessLevel >= 90)
        {
            WeatherMakerFullScreenCloudsScript.Instance.ShowCloudsAnimated(2, "WeatherMakerCloudProfile_Overcast");
        }
        else if (cloudinessLevel >= 50)
        {
            WeatherMakerFullScreenCloudsScript.Instance.ShowCloudsAnimated(2, "WeatherMakerCloudProfile_Gloomy");
        }
        else if (cloudinessLevel >= 40)
        {
            WeatherMakerFullScreenCloudsScript.Instance.ShowCloudsAnimated(2, "WeatherMakerCloudProfile_HeavyScattered");
        }
        else if (cloudinessLevel >= 25)
        {
            WeatherMakerFullScreenCloudsScript.Instance.ShowCloudsAnimated(2, "WeatherMakerCloudProfile_LightScattered");
        }
        else if (cloudinessLevel >= 10)
        {
            WeatherMakerFullScreenCloudsScript.Instance.ShowCloudsAnimated(2, "WeatherMakerCloudProfile_PartlyCloudy");
        }
        else
        {
            WeatherMakerFullScreenCloudsScript.Instance.ShowCloudsAnimated(2, "WeatherMakerCloudProfile_None");
        }
    }
}
