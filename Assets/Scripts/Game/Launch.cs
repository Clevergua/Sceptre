using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    private GameState gameState;
    private StringBuilder stringBuilder;
    [SerializeField]
    private Text text;
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += LogOnContent;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LogOnContent(string condition, string stackTrace, LogType type)
    {
        stringBuilder.Append($"[{type.ToString()}]{condition}\n");
        text.text = stringBuilder.ToString();
    }
}
