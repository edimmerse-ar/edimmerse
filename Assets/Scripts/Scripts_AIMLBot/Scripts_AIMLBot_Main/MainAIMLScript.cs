using UnityEngine;
using System.Collections;
//
using System.Xml;
using System.Collections.Generic;

using UnityEngine.UI;
/*
    Import AIML files within the Resources
*/

public class MainAIMLScript : MonoBehaviour
{
    private TextAsset[] aimlFiles;
    private List<string> aimlXmlDocumentListFileName = new List<string>();
    private List<XmlDocument> aimlXmlDocumentList = new List<XmlDocument>();
    //
    private TextAsset GlobalSettings, GenderSubstitutions, Person2Substitutions, PersonSubstitutions, Substitutions, DefaultPredicates, Splitters;
    //
    private ChatbotMobileWeb bot;
    public InputField InputBox;
    public InputField OutputBox;

    public UpdateScore updateScore;

	// Use this for initialization
	void Start()
    {
        bot = new ChatbotMobileWeb();
        LoadFilesFromConfigFolder();
        bot.LoadSettings(GlobalSettings.text, GenderSubstitutions.text, Person2Substitutions.text, PersonSubstitutions.text, Substitutions.text, DefaultPredicates.text, Splitters.text);
        TextAssetToXmlDocumentAIMLFiles();
        bot.loadAIMLFromXML(aimlXmlDocumentList.ToArray(), aimlXmlDocumentListFileName.ToArray());
        bot.LoadBrain();
        
        if (InputBox != null)
        {
            InputBox.text = !string.IsNullOrEmpty(DIYProjectHandler.selectedQuestion) ? DIYProjectHandler.selectedQuestion : "";
        }
    }


    /// <summary>
    /// Button to send the question to the robot
    /// </summary>
    public void SendQuestionToRobot()
    {
		updateScore.Submit(2);
		StartCoroutine("SendQuesToBot");
    }

    IEnumerator SendQuesToBot()
    {
        if (string.IsNullOrEmpty(InputBox.text) == false)
        {
            // Response Bot AIML
            var answer = bot.getOutput(InputBox.text);

            InputBox.text = string.Empty;

            OutputBox.text = ("Typing.");
            yield return new WaitForSeconds(0.3f);
            OutputBox.text = ("Typing..");
            yield return new WaitForSeconds(0.6f);
            OutputBox.text = ("Typing...");
            yield return new WaitForSeconds(0.9f);

            // Response BotAIml in the Chat window
            OutputBox.text = answer;
        }
    }

    void LoadFilesFromConfigFolder()
    {
        GlobalSettings = Resources.Load<TextAsset>("AIMLBot/config/Settings");
        GenderSubstitutions = Resources.Load<TextAsset>("AIMLBot/config/GenderSubstitutions");
        Person2Substitutions = Resources.Load<TextAsset>("AIMLBot/config/Person2Substitutions");
        PersonSubstitutions = Resources.Load<TextAsset>("AIMLBot/config/PersonSubstitutions");
        Substitutions = Resources.Load<TextAsset>("AIMLBot/config/Substitutions");
        DefaultPredicates = Resources.Load<TextAsset>("AIMLBot/config/DefaultPredicates");
        Splitters = Resources.Load<TextAsset>("AIMLBot/config/Splitters");
    }

    void TextAssetToXmlDocumentAIMLFiles()
    {
        aimlFiles = Resources.LoadAll<TextAsset>("AIMLBot/xmls");
        foreach (TextAsset aimlFile in aimlFiles)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(aimlFile.text);
                aimlXmlDocumentListFileName.Add(aimlFile.name);
                aimlXmlDocumentList.Add(xmlDoc);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }

    void OnDisable()
    {
        bot.SaveBrain();
    }

}
