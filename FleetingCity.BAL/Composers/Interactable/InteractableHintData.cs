using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class InteractableHintData : BaseGameData<InteractableHintData, InteractableHintModel>
{
    protected override string ResourceFileName => "InteractionHint";

    public InteractableHintData() : base() {}
}