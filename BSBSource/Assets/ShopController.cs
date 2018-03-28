using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {

    public Text MoneyText;
    public GameController GameController;
	
	void Update ()
    {
        MoneyText.text = GameController.GameStats.Money.ToString();
    }
}
