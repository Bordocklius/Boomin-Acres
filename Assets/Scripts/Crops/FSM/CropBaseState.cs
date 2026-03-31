using PD4.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class PlantedCrop
{
    public class CropBaseState : IState
    {
        // FSM
        public CropFSM FSM { get; private set; }
        public PlantedCrop Context => FSM.Context;
        public CropBaseState(CropFSM fsm)
        {
            FSM = fsm;
        }

        // IState methods
        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float fixedDeltaTime) { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        public virtual void TransitionToStage() { }
        public virtual void HarvestCrop() { }

    }
}
