using Crystal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class SafeAreaController : MonoBehaviour
{

    public List<SafeArea> SafeAreas;
    private void Awake()
    {
        foreach (SafeArea area in SafeAreas)
        {
            area.Initialized();
        }
    }

    private void Update()
    {
        foreach (SafeArea area in SafeAreas)
        {
            if (area.IsRunning)
            {
                area.Refresh();
            }
           
        }
    }
}
