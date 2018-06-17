using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Money")]
public class PlayerMoney : ScriptableObject {

	public int money;

	public void AddMoney (int amount) {
		money += amount;
	}

	public void SubtractMoney (int amount) {
		money -= amount;
	}

	public void SetMoney (int amount) {
		money = amount;
	}

	public void Reset ()  {
		money = 0;
	}
}
