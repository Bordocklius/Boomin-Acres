using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class PlantedCrop
{
    public class CropGrownState : CropBaseState
    {
        public CropGrownState(CropFSM fsm) : base(fsm) { }

        public override void OnEnter()
        {
            Context.RequestHarvest();
        }

        public override void HarvestCrop()
        {
            Context.Harvest();
        }
    }
}
