using FleetingCity.BAL.Model;

namespace FleetingCity.BAL.Data;

public class TreeData : BaseGameData<TreeData, TreeDataModel>
{
    protected override string ResourceFileName => "Tree";

    public TreeData() : base() {}
}