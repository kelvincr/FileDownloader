using System;
using System.Collections.Generic;
using System.Text;

namespace Extensibility
{
    /// <summary>
    /// Downloaded completed states
    /// </summary>
    public enum CompletedState
    {

        /// <summary>
        /// Non started
        /// </summary>
        NonStarted,

        /// <summary>
        /// Download successful
        /// </summary>
        Succeeded,

        /// <summary>
        /// Download partial
        /// </summary>
        Partial,

        /// <summary>
        /// Download canceled
        /// </summary>
        Canceled,

        /// <summary>
        /// Download failed
        /// </summary>
        Failed
    }
}
