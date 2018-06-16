using UnityEngine;

public class LocationReaction : Reaction {

	public Location location;
	public bool locationDiscoveredState;

	protected override void ImmediateReaction (){
		location.locationDiscovered  = locationDiscoveredState;
	}
}
