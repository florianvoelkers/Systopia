using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class IntVariable : ScriptableObject {

	#if UNITY_EDITOR
	[Multiline]
	public string developerDescription = "";
	#endif

	public int value;
	public int maxValue;

	public void SetValue (int value) {
		this.value = value;
		maxValue = this.value;
	}

	public void SetValue (IntVariable value) {
		this.value = value.value;
		maxValue = this.value;
	}

	public bool ApplyChange (int amount) {
		if (value >= maxValue)
			return false;
		value += amount;
		if (value > maxValue)
			value = maxValue;
		return true;
	}

	public bool ApplyChange (IntVariable amount) {
		if (value >= maxValue)
			return false;
		value += amount.value;
		if (value > maxValue)
			value = maxValue;
		return true;
	}
}
