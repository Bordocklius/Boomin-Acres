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


        // TODO: Switch BaseState for the actual base state
        public new BaseState CurrentState
        {
            get { return base.CurrentState as BaseState; }
        }

        public CropFSM(PlantedCrop context)
        {
            Context = context;
            // TODO: Create states & transition to initial state
        }
    }
}
