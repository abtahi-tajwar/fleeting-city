using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class MaterialResourceData : BaseGameData<MaterialResourceData, NaturalResourceModel>
{
    protected override string ResourceFileName => "MaterialResource";

    public MaterialResourceData() : base() {}
}