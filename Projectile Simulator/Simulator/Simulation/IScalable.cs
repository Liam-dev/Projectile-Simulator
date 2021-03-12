namespace Simulator.Simulation
{
    /// <summary>
    /// An interface for scaling objects.
    /// </summary>
    interface IScalable
    {
        bool MaintainAspectRatio { get; set; }
    }
}