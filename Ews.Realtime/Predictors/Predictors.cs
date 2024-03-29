using System;
using System.Collections.Generic;

namespace Ews.Realtime.Predictors
{
    /// <summary>
    /// Linear interpolation prediction
    /// </summary>
    public readonly struct LinearPredictor : IDataPredictor
    {
        public float CalculateDataForTime(IList<float> historyData, IList<DateTime> historyDates, DateTime dateTime)
        {
            if (historyData.Count < 2)
            {
                return historyData[^1];
            }
            float h1 = historyData[^2], h2 = historyData[^1];
            DateTime t1 = historyDates[^2], t2 = historyDates[^1];
            float slope = (h2 - h1) / (float)(t2 - t1).TotalMilliseconds;
            return h1 + (float)(dateTime - t1).TotalMilliseconds * slope;
        }
    }

    /// <summary>
    /// A more complex prediction which takes acceleration in mind
    /// </summary>
    public readonly struct AccelerationPredictor : IDataPredictor
    {
        public float CalculateDataForTime(IList<float> historyData, IList<DateTime> historyDates, DateTime dateTime)
        {
            if (historyData.Count < 3)
            {
                return historyData[^1];
            }
            float h1 = historyData[^3], h2 = historyData[^2], h3 = historyData[^1];
            DateTime t1 = historyDates[^3], t2 = historyDates[^2], t3 = historyDates[^1];
            float v1 = (h2 - h1) / (float)(t2 - t1).TotalMilliseconds;
            float v2 = (h3 - h2) / (float)(t3 - t2).TotalMilliseconds;
            float a = (v2 - v1) / (float)(t3 - t1).TotalMilliseconds;
            return h2 + v2 * (float)(dateTime - t2).TotalMilliseconds +
                   0.5f * a * (float)Math.Pow((dateTime - t2).TotalMilliseconds, 2);
        }
    }

    /// <summary>
    /// A prediction which takes acceleration and jerk in mind
    /// </summary>
    public readonly struct JerkPredictor : IDataPredictor
    {
        public float CalculateDataForTime(IList<float> historyData, IList<DateTime> historyDates, DateTime dateTime)
        {
            if (historyData.Count < 4)
            {
                return historyData[^1];
            }
            float h1 = historyData[^4], h2 = historyData[^3], h3 = historyData[^2], h4 = historyData[^1];
            DateTime t1 = historyDates[^4], t2 = historyDates[^3], t3 = historyDates[^2], t4 = historyDates[^1];
            float v1 = (h2 - h1) / (float)(t2 - t1).TotalMilliseconds;
            float v2 = (h3 - h2) / (float)(t3 - t2).TotalMilliseconds;
            float v3 = (h4 - h3) / (float)(t4 - t3).TotalMilliseconds;
            float a1 = (v2 - v1) / (float)(t2 - t1).TotalMilliseconds;
            float a2 = (v3 - v2) / (float)(t3 - t2).TotalMilliseconds;
            float j = (a2 - a1) / (float)(t3 - t1).TotalMilliseconds;
            return h3 + v3 * (float)(dateTime - t3).TotalMilliseconds +
                   0.5f * a2 * (float)Math.Pow((dateTime - t3).TotalMilliseconds, 2) +
                   j * (float)Math.Pow((dateTime - t3).TotalMilliseconds, 3) / 6;
        }
    }
}