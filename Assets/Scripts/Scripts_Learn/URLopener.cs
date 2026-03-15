using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class URLopener : MonoBehaviour
{
	public UpdateScore updateScore;

	public void openURL()
    {
        Application.OpenURL("https://github.com/edimmerse-ar/edimmerse");
		updateScore.Submit(1);
	}
    public void openURL1()
    {
        Application.OpenURL("https://youtu.be/Aem1ApwpcJ4");
		updateScore.Submit(1);
	}

	//.....

	public void openArduino()
	{
		Application.OpenURL("https://youtu.be/tX95cfYHOeQ");
		updateScore.Submit(1);
	}

	public void openRaspberryPi()
	{
		Application.OpenURL("https://youtu.be/weBZBKXlTBk");
		updateScore.Submit(1);
	}
	
    public void openBreadboard()
	{
		Application.OpenURL("https://youtu.be/3BFhqtEKutU");
		updateScore.Submit(1);
	}

	//Wires

	public void openResistor()
	{
		Application.OpenURL("https://youtu.be/-KAa11m3H2g");
		updateScore.Submit(1);
	}

	public void openDH11()
    {
        Application.OpenURL("https://youtu.be/wqI8PM4mACE");
		updateScore.Submit(1);
	}

	//Ultrasonic sensor

	public void openPIR()
	{
		Application.OpenURL("https://youtu.be/YL2gp4ki7qA");
		updateScore.Submit(1);
	}
	
	public void openLDR()
	{
		Application.OpenURL("https://youtu.be/xL1QiN8CIkc");
		updateScore.Submit(1);
	}

	//IR Sensor
	//LDC
	//DC Motor
	//Servo Motor
	//LED
	//Button

	public void openPotentiometer()
    {
        Application.OpenURL("https://youtu.be/dBFf4OG513Y");
		updateScore.Submit(1);
	}

	//Buzzer
	//Battery
}
