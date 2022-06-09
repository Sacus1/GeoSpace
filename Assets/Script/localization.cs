using UnityEngine;
using TMPro;

public static class localization
{
    //csv file to read
    public static TextAsset translationFile;
    // method that take system language and return the language code
    // 0 : English , 1 : French
    public static int getLanguage(){
        return 1; // for test purpose
        // switch (Application.systemLanguage)
        // {
        //     case SystemLanguage.French:
        //         return 1;
        //         break;
        //     default:
        //         return 0;
        //         break;
        // }
    }
    // function that take all TextMeshPro objects and change their text to the translation
    public static void translate()
    {
        // if the translation file is null
        if (translationFile == null)
        {
            // load the translation file
            translationFile = Resources.Load<TextAsset>("translations");
        }
        //get all TextMeshPro objects
        TextMeshPro[] texts = GameObject.FindObjectsOfType<TextMeshPro>();
        //for each text
        foreach (TextMeshPro text in texts)
        {
            // get the text content
            string textContent = text.text;
            string textFinalContent = "";
           // for each line in the text
            foreach (string line in textContent.Split('\n'))
            {
                // if the line is not empty translate it and add it to the final text
                if (line != "")
                {
                    textFinalContent += findKey(line,getLanguage()) + "\n";
                }

            }
            // set the text to the final text
            text.text = textFinalContent;
        }

        // same with TextMeshProUI
        TextMeshProUGUI[] textsUI = GameObject.FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in textsUI)
        {
            string textContent = text.text;
            string textFinalContent = "";
            foreach (string line in textContent.Split('\n'))
            {
                if (line != "")
                {
                    // if line is company , game name or version number , don't translate it
                    if (line == "Company" || line == "Game Name" || line == "Version")
                    {
                        textFinalContent += line + "\n";
                    }
                    else
                    {
                        textFinalContent += findKey(line, getLanguage()) + "\n";
                    }
                }
            }
            text.text = textFinalContent;
        }
    }
    //function to find the key in the csv file
    public static string findKey(string key, int language)
    {
        //split the csv file into lines
        string[] lines = translationFile.text.Split('\n');
        //loop through all the lines
        for (int i = 1; i < lines.Length; i++)
        {
            //split the lines into key and value
            string[] line = lines[i].Split(',');

            //if the key is found return the value
            if (line[0] == key)
            {
                //return the value of the key in the language
                return line[language];
            }
        }
        //if the key is not found return the key
        Debug.LogError("Key" + key + "not found in " + language + " language");
        return key;
    }
}