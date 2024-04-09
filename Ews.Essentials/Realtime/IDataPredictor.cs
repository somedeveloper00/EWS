using System;
using System.Collections.Generic;

namespace Ews.Essentials.Realtime
{
    /// <summary>
    /// Represents a system that can predict data, given its history
    /// </summary>
    public interface IDataPredictor
    {
        /// <summary>
        /// Calculate and return the predicted data for the given date time.
        /// </summary>
        public float CalculateDataForTime(IList<float> historyData, IList<DateTime> historyDates, DateTime dateTime);
    }
}