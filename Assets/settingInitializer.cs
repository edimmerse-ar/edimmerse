using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingInitializer : MonoBehaviour
{
        public SettingsHandler settingsHandler;

    // Start is called before the first frame update
    void Start()
    {
                settingsHandler.ApplySettings();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
