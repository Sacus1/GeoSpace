using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class localization
{
    //csv file to read
    public static TextAsset translationFile;
    // an liste of string to don't translate
    static List<string> ignore = new List<string>(new string[]{
            "+","-","*","/","=","(",")","[","]","{","}","<",">",";",":",",",".","!","?","#","$","%","^","&","|","~","`","@","_","\\","/","\"","'"," ","\n","\r","\t",Application.companyName, Application.productName ,"Version " + Application.version,"00.00.00","00:00:00","X","O"
    });
    // method that take system language and return the language code
    // 0 : English , 1 : French
    public static int getLanguage()
    {
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
            text.text = "";
            // for each line in the text
            foreach (string line in textContent.Split('\n'))
            {
                // if the line is not empty translate it and add it to the final text
                if (line != "")
                {
                    float result;
                    // if line is in ignore list or if the line is a number and can be parsed to a float don't translate it
                    if (ignore.Contains(line) || (float.TryParse(line, out result)))
                    {
                        text.text += line + "\n";
                    }
                    else // else translate it
                    {
                        text.text += findKey(line, getLanguage()) + "\n";
                    }
                }
            }
        }

        // same with TextMeshProUI
        TextMeshProUGUI[] textsUI = GameObject.FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in textsUI)
        {
            string textContent = text.text;
            text.text = "";
            foreach (string line in textContent.Split('\n'))
            {
                if (line != "")
                {
                    float result;
                    if (ignore.Contains(line) || (float.TryParse(line, out result)))
                    {
                        text.text += line + "\n";
                    }
                    else
                    {
                        // ignore every text between [] and translate the rest
                        if (line.Contains("[") && line.Contains("]"))
                        {
                            int start = line.IndexOf("[");
                            int end = line.IndexOf("]");
                            text.text += findKey(line.Substring(0, start), getLanguage());
                            text.text += line.Substring(start, end - start + 1);
                            text.text += findKey(line.Substring(end + 1), getLanguage()) + "\n";
                        }
                        else
                        {
                            text.text += findKey(line, getLanguage()) + "\n";
                        }
                    }
                }
            }
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
        return key;
    }
}