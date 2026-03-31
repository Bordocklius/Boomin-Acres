using PD4.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PlantedCrop
{
    public class CropFSM : FiniteStateMachine
    {
        // Context for this FSM
        public PlantedCrop Context { get; set; }

        // TODO: States for this FSM (add these in when needed)
        public CropPlantedState CropPlantedState { get; private set; }
        public CropGrowingState CropGrowingState { get; private set; }
        public CropGrownState CropGrownState { get; private set; }
        public CropStoppedGrowingState CropStoppedGrowingState { get; private set; }

        // TODO: Switch BaseState for the actual base state
        public new CropBaseState CurrentState
        {
            get { return base.CurrentState as CropBaseState; }
        }

        public CropFSM(PlantedCrop context)
        {
            Context = context;
            // TODO: Create states & transition to initial state
            CropPlantedState = new CropPlantedState(this);
            CropGrowingState = new CropGrowingState(this);
            CropStoppedGrowingState = new CropStoppedGrowingState(this);
            CropGrownState = new CropGrownState(this);
            TransitionTo(CropPlantedState);
        }
    }
}
