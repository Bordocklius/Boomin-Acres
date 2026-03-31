using UnityEngine;

public enum InteractionMode
{
    Idle,
    Plowing,
    Planting,
    Watering,
    Harvesting
}

public class FarmInteraction : MonoBehaviour
{
    public InteractionMode Mode = InteractionMode.Idle;
    private 

    void Update()
    {
        switch (Mode)
        {
            case InteractionMode.Idle:
                break;
            case InteractionMode.Plowing:
                Plowing();
                break;
            case InteractionMode.Planting:
                Planting();
                break;
            case InteractionMode.Watering:
                Watering();
                break;
            case InteractionMode.Harvesting:
                Harvesting();
                break;
        }
            
    }

    void Plowing()
    {

    }

    void Planting()
    {

    }

    void Watering()
    {

    }

    void Harvesting()
    {

    }
}
