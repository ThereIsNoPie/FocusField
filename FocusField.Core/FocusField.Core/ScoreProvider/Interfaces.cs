using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace FocusField.Core
{

    //MVP fix on where user is looking and white out the rest.
    public interface IScoreCalculator
    {
        Score GetScore(bool isLookingAtObject);
    }

    public class ScoreCalculator : IScoreCalculator
    {
        public Score GetScore(bool isLookingAtObject)
        {
            throw new NotImplementedException();
        }
    }

    public class Score : ValueObject
    {
        public Score(double value)
        {
            Value = value;
        }

        public double Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
