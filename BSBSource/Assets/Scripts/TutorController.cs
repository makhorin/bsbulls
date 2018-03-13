using Assets;
using UnityEngine;
using UnityEngine.UI;

public class TutorController : MonoBehaviour {

    private static KeyCode? _key;
    private static int _times;
    private float _showTime;
    private float _lastSwitch;

    public Text TextField;
    public Shadow Shadow;
    public GameSettings Settings;

    public static void ShowTutor(KeyCode key, int times)
    {
        _key = key;
        _times = times;
    }

    void Start()
    {
       
    }
    
    void Update ()
    {
        if (TextField.IsActive())
        {
            if (Input.GetKeyDown(_key.Value))
                _times--;
            var prt = GameSettings.MaxSpeed / GameSettings.DefaultTutorShowTime;
            var spd = Mathf.FloorToInt(GameStats.CurrentSpeed / prt);

            if (Time.time - _showTime >= GameSettings.DefaultTutorShowTime - spd || _times <=0)
            {
                TextField.gameObject.SetActive(false);
                _key = null;
                _times = 0;
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
                    TextField.text = "TAP LEFT";
                    break;
                case KeyCode.RightArrow:
                    TextField.text = "TAP RIGHT";
                    break;
                case KeyCode.DownArrow:
                    TextField.text = "GET DOWN";
                    break;
            }
            _showTime = Time.time;
        }
    }
}
