using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class NaturalResourceData : BaseGameData<NaturalResourceData, NaturalResourceModel>
{
    protected override string ResourceFileName => "NaturalResource";

    public NaturalResourceData() : base() {}
}