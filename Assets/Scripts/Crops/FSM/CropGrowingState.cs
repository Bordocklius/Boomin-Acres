using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PlantedCrop
{
    public class CropGrowingState : CropBaseState
    {
        public CropGrowingState(CropFSM fsm) : base(fsm) { }

        public override void Update(float deltaTime)
        {
            Context._timeSpentGrowing += deltaTime * Context.GrowthMultiplier;
            if(Context._timeSpentGrowing > Context.CropData.GrowthTime)
            {
                FSM.TransitionTo(FSM.CropGrownState);
            }
        }

        public override void OnExit()
        {
            TransitionToStage();
        }

        public override void TransitionToStage()
        {
            Context.TransitionToStage(2);
        }
    }
}
