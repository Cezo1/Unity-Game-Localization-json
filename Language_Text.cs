using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Language_Text : MonoBehaviour
{
    #region Var
    [Header("Перевод")]
    [Tooltip("Переводимые объекты")]
    public LocalazableText[] texts;
    [Header("Опиции")] 
    [Tooltip("Язык игры по умолчанию")] public DefaultLanguages defaultLanguage;
    [Header("Остальное")] 
    private System.Globalization.CultureInfo _currentLanguage;
    [HideInInspector]
    public string localizationFilePath;
  
    #endregion
    private void Awake()
    {
        DefineSystemLanguage();
        
    }

    private JsonKeys GetJson()
    {
        var o = JsonUtility.FromJson<JsonKeys>(File.ReadAllText(localizationFilePath));
        return o;
    }
    public static JsonKeys GetJson(string path)
    {
        var o = JsonUtility.FromJson<JsonKeys>(File.ReadAllText(path));
        return o;
    }
    private void DefineSystemLanguage()
    {
        _currentLanguage = System.Globalization.CultureInfo.CurrentCulture;
        localizationFilePath = Path.Combine(Application.dataPath, "languages\\" + _currentLanguage + ".json");
        if (File.Exists(Path.Combine(Application.dataPath, "languages\\" + _currentLanguage + ".json" ))) // Ищем файл с переводом
            SetLanguage(localizationFilePath);
        else // Файла нету 
            SetLanguage(defaultLanguage == DefaultLanguages.Eng ? "default_eng" : "default_rus");
    }
    public void SetLanguage(string property)
    {
        switch (property)
        {
            case "default_eng":
                localizationFilePath = Path.Combine(Application.dataPath, "languages\\" + "en-US.json");
                ApplyLanguage();
                break;
            case "default_rus":
                localizationFilePath = Path.Combine(Application.dataPath, "languages\\" + "ru-RU.json");
                ApplyLanguage();
                break;
            default:
                ApplyLanguage();
                break;
        }
    }

    private void ApplyLanguage()
    {
        var localization = GetJson();
        for (var i = 0; i < texts.Length; i++)
        {
            if (localization.keys[i] == texts[i].id)
            {
                texts[i].text.text = localization.Shop_Button[i];
            }
        }  
    }
}
public enum DefaultLanguages
{
 [InspectorName("Русский")] Rus,
 [InspectorName("Английский")] Eng
}

[Serializable]
public class LocalazableText
{
    
   public string name;
   public Text text;
   public string id;

}
public class JsonKeys
{
    public string language;
    public string[] Shop_Button;
    public string[] keys;
} 


