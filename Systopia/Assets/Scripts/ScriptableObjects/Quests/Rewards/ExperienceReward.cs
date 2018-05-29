public class ExperienceReward : Reward {

	public int experienceRewarded;
	public PlayerExperience playerExperience;

	public override void RewardPlayer () {
		playerExperience.AddExperience (experienceRewarded);
	}
}
