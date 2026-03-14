using UnityEngine;
using UnityEngine.UI;

public class PPTHandler : MonoBehaviour
{
	[Header("UI References")]
	public Image targetImage;

	[Header("Slide Settings")]
	public string baseName = "Ed_";
	public int totalSlides = 10; 

	private int currentIndex = 0;

	public Button preBtn;
	public Button nextBtn;

	void Start()
	{
		// Load the first slide
		LoadImage(currentIndex);
	}

	void LoadImage(int index)
	{
		if (currentIndex < 0 || currentIndex > totalSlides) return;

		string slideName = baseName + index;
		Sprite sprite = Resources.Load<Sprite>("Slides/" + slideName);

		if (sprite != null)
		{
			targetImage.sprite = sprite;
			targetImage.preserveAspect = true;
		}
		else
		{
			Debug.LogWarning("Slide not found: " + slideName);
		}
	}

	public void NextSlide()
	{
		if (currentIndex < totalSlides)
		{
			currentIndex++;
			LoadImage(currentIndex);
		}

		toggelButtons();
	}

	public void PrevSlide()
	{
		if (currentIndex > 0)
		{
			currentIndex--;
			LoadImage(currentIndex);
		}

		toggelButtons();
	}

	public void toggelButtons()
	{
		if (currentIndex == totalSlides)
		{
			nextBtn.interactable = (false);

			GlobalVariables.updateScore(10);
		}
		else
		{
			nextBtn.interactable = (true);
		}

		if (currentIndex == 0)
		{
			preBtn.interactable = (false);
		}
		else
		{
			preBtn.interactable = (true);
		}
	}
}
