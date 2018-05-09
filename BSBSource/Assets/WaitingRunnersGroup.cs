using UnityEngine;

public class WaitingRunnersGroup : MonoBehaviour {

	void Start ()
    {
        if (!GameController.GameStats.ShowWaitingRunners)
            Destroy(gameObject);
    }
}
