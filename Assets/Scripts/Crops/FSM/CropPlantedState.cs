using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PlantedCrop
{
    public class CropPlantedState : CropBaseState
    {
        private float _timer;
        private float _startGrowingTimer = 2f;

        public CropPlantedState(CropFSM fsm) : base(fsm) { }

        public override void OnEnter()
        {
            _timer = 0f;
            Context._timeSpentGrowing = 0f;
            Context.TransitionToStage(0);
        }

        public override void Update(float deltaTime)
        {
            _timer += deltaTime;
            if(_timer > _startGrowingTimer)
            {
                FSM.TransitionTo(FSM.CropGrowingState);
            }
        }

        public override void OnExit()
        {
            TransitionToStage();
        }

        public override void TransitionToStage()
        {
            Context.TransitionToStage(1);
        }
    }
}
