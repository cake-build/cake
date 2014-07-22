namespace Cake.Core
{
    /// <summary>
    /// Represents a report printer.
    /// </summary>
    public interface ICakeReportPrinter
    {
        /// <summary>
        /// Writes the specified report to a target.
        /// </summary>
        /// <param name="report">The report to write.</param>
        void Write(CakeReport report);
    }
}
