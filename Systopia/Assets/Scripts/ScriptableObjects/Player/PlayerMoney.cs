using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/Money")]
public class PlayerMoney : ScriptableObject {

	public int money;

	public void AddMoney (int amount) {
		money += amount;
	}

	public bool SubtractMoney (int amount) {
		money -= amount;
		if (money < 0) {
			money += amount;
			return false;
		}
		return true;
	}

	public void SetMoney (int amount) {
		money = amount;
	}

	public void Reset ()  {
		money = 0;
	}
}
