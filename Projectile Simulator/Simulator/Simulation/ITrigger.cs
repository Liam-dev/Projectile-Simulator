using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Simulation
{
    /// <summary>
    /// A trigger to trigger an ITriggerable.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Occurs when trigger is triggered.
        /// </summary>
        event EventHandler Triggered;
    }
}
