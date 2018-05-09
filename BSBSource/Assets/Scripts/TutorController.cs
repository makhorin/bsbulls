using Assets;
using UnityEngine;
using UnityEngine.UI;

public class TutorController : MonoBehaviour {

    private static KeyCode? _key;
    private float _showTime;
    private float _lastSwitch;
    private static int _times;

    public Text TextField;
    public Shadow Shadow;
    public GameSettings Settings;

    public static void ShowTutor(KeyCode key, int times)
    {
       // _key = key;
       // _times = times;
    }

    void Start()
    {
       
    }
    
    void Update ()
    {
        if (TextField.IsActive())
        {
            switch (_key.Value)
            {
                case KeyCode.LeftArrow:
                case KeyCode.RightArrow:
                    if (InputHelper.LeftTap() || InputHelper.RightTap())
                        _times--;
                    break;
                case KeyCode.DownArrow:
                    if (InputHelper.Down())
                        _times--;
                  break;
            }

            if (Time.time - _showTime >= GameSettings.DefaultTutorShowTime || _times <= 0)
            {
                TextField.gameObject.SetActive(false);
                _key = null;
            }   
            else if (Time.time - _lastSwitch >= 0.2f)
            {
                var shadowColor = Shadow.effectColor;
                Shadow.effectColor = TextField.color;
                TextField.color = shadowColor;
                _lastSwitch = Time.time;
            }
        }
        else
        {
            if (!_key.HasValue)
                return;
            TextField.gameObject.SetActive(true);
            switch (_key.Value)
            {
                case KeyCode.LeftArrow:
                case KeyCode.RightArrow:
                    TextField.text = "TAP";
                    break;
                case KeyCode.DownArrow:
                    TextField.text = "SWIPE DOWN";
                    break;
            }
            _showTime = Time.time;
        }
    }
}
