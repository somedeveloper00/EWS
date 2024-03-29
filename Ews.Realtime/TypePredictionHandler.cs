using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ews.Realtime
{
    public sealed class TypePredictionHandler<T>
        where T : unmanaged
    {
        private readonly IDataPredictor _predictor;
        private static readonly FieldInfo[] s_floatFields = typeof(T)
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(f => f.FieldType == typeof(float)).ToArray();

        public List<float>[] _history;
        public List<DateTime> _dates;

        public TypePredictionHandler(int historySize, IDataPredictor predictor)
        {
            _predictor = predictor;
            _history = new List<float>[s_floatFields.Length];
            for (int i = 0; i < _history.Length; i++)
            {
                _history[i] = new(historySize);
            }
            _dates = new(historySize);
        }

        public void AddPoint(DateTime date, T data)
        {
            for (int i = 0; i < s_floatFields.Length; i++)
            {
                if (_history[i].Count + 1 == _history[i].Capacity)
                    _history[i].RemoveAt(0);
                _history[i].Add((float)s_floatFields[i].GetValueDirect(__makeref(data)));
            }
            if (_dates.Count + 1 == _dates.Capacity)
                _dates.RemoveAt(0);
            _dates.Add(date);
        }

        public T GetPredictedValue(DateTime dateTime)
        {
            if (_history.Length == 0)
                return default;
            if (_history[0].Count == 0)
                return default;

            T result = new();
            for (int i = 0; i < s_floatFields.Length; i++)
            {
                s_floatFields[i].SetValueDirect(__makeref(result), _predictor.CalculateDataForTime(_history[i], _dates, dateTime));
            }
            return result;
        }
    }
}