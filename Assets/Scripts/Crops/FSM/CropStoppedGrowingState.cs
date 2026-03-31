using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class PlantedCrop
{
    public class CropStoppedGrowingState : CropBaseState
    {
        public CropStoppedGrowingState(CropFSM fsm) : base(fsm) { }
    }
}
