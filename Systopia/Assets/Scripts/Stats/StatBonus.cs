public class StatBonus {

	public int value { get; set; }
	public string statType { get; set; }

	public StatBonus (int value, string statType) {
		this.value = value;
		this.statType = statType;
	}
}
