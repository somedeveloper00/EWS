using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ews.Essentials.Realtime
{
    public sealed class TypePredictionHandler<T>
        where T : unmanaged
    {
        private readonly IDataPredictor _predictor;

        private static readonly FieldInfo[] s_floatFields =
            typeof(T)
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(f => f.FieldType == typeof(float))
            .ToArray();

        public List<float>[] history;
        public List<DateTime> dates;

        public TypePredictionHandler(int historySize, IDataPredictor predictor)
        {
            _predictor = predictor;
            history = new List<float>[s_floatFields.Length];
            for (int i = 0; i < history.Length; i++)
            {
                history[i] = new(historySize);
            }
            dates = new(historySize);
        }

        public void AddPoint(DateTime date, T data)
        {
            for (int i = 0; i < s_floatFields.Length; i++)
            {
                if (history[i].Count + 1 == history[i].Capacity)
                    history[i].RemoveAt(0);
                history[i].Add((float)s_floatFields[i].GetValueDirect(__makeref(data)));
            }
            if (dates.Count + 1 == dates.Capacity)
                dates.RemoveAt(0);
            dates.Add(date);
        }

        public T GetPredictedValue(DateTime dateTime)
        {
            if (history.Length == 0)
                return default;
            if (history[0].Count == 0)
                return default;

            T result = new();
            for (int i = 0; i < s_floatFields.Length; i++)
            {
                s_floatFields[i].SetValueDirect(__makeref(result), _predictor.CalculateDataForTime(history[i], dates, dateTime));
            }
            return result;
        }
    }
}