using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUpdate : MonoBehaviour
{
    public Text trafficCone;
    public Text barricade;
    public Text airhorn;

    public void Update()
    {
        trafficCone.text = "X " + StaticItems.TrafficConeCount;
        barricade.text = "X " + StaticItems.BarricadeCount;
        airhorn.text = "X " + StaticItems.AirhornCount;
    }
}
