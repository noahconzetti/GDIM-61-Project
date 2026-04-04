using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Collections.Generic;
using Random = UnityEngine.Random;

public static class ExtraRandom {
    public static T WeightedChoice<T>(Collection<T> values, Collection<float> weights) {
        float total = 0f;
        foreach (float weight in weights) {
            total += weight;
        }

        float choice = Random.Range(0f, total);
        float runningCount = 0f;
        for (int i = 0; i < values.Count(); i++) {
            runningCount += weights[i];
            if (choice <= runningCount) {
                return values[i];
            }
        }

        return values[0];
    }
    public static T WeightedChoice<T>(IList<T> values, Func<T, float> valueToWeight) {
        float total = values.Sum(valueToWeight);

        float choice = Random.Range(0f, total);
        float runningCount = 0f;
        foreach (T value in values) {
            runningCount += valueToWeight(value);
            if (choice <= runningCount) {
                return value;
            }
        }

        return values[0];
    }
}
