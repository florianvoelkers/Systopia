public class MoneyReward : Reward {

	public int moneyRewarded;
	public PlayerMoney playerMoney;

	public override void RewardPlayer () {
		playerMoney.AddMoney (moneyRewarded);
	}
}
