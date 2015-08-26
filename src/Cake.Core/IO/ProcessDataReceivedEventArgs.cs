using System;

namespace Cake.Core.IO
{
    /// <summary> 
    /// The event args that contains information about the line of text written to either
    /// StandardOutputStream or StandardErrorStream on the created process. 
    /// </summary>
    public class ProcessDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the line of text written to StandardOutputStream / StandardErrorStream.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessDataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ProcessDataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }
}