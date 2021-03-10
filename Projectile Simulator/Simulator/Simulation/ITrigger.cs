using System;

namespace Simulator.Simulation
{
    /// <summary>
    /// An interface to trigger an triggerable object.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Occurs when trigger is triggered.
        /// </summary>
        event EventHandler Triggered;
    }
}