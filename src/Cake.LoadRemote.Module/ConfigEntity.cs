using System.Collections.Generic;

namespace Cake.LoadRemote.Module
{
    /// <summary>
    /// Represents a setting model.
    /// </summary>
    public class ConfigEntity
    {
        /// <summary>
        /// Gets the load order of files.
        /// </summary>
        public List<string> FileOrder { get; set; }
    }
}
