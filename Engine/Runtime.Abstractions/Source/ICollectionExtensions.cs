using System;
using System.Collections.Generic;
using System.Linq;

namespace MudDesigner.Runtime
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// return a random element of the list or default if list is empty
        /// </summary>
        /// <typeparam name="T">The Type that this method will use to compare</typeparam>
        /// <param name="originalCollection">The sender.</param>
        /// <param name="weightSelector">return chances to be picked for the element. A weigh of 0 or less means 0 chance to be picked.
        /// If all elements have weight of 0 or less they all have equal chances to be picked.</param>
        /// <returns>Returns a reference to the item that was selected using the given delegate</returns>
        /// <exception cref="System.Exception">Unable to produce a result from the given collection using the supplied selector.</exception>
        public static T AnyOrDefaultFromWeight<T>(this ICollection<T> originalCollection, Func<T, double> weightSelector)
        {
            if (originalCollection.Count < 1)
            {
                return default(T);
            }
            else if (originalCollection.Count == 1)
            {
                return originalCollection.ElementAtOrDefault(0);
            }

            var weights = weightSelector == null ? new double[0] : originalCollection.Select(item => Math.Max(weightSelector(item), 0)).ToArray();
            var sum = weights.Sum(d => d);

            var rnd = new Random().NextDouble();
            return FindWeight<T>(originalCollection, weights, sum, rnd);
        }

        /// <summary>
        /// Returns a random item out of a collection based on the weights each item is assigned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalCollection">The e.</param>
        /// <param name="weights">The weights.</param>
        /// <param name="sum">The sum.</param>
        /// <param name="randomValue">The random value.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Unable to produce a result from the given collection using the supplied selector.</exception>
        static T FindWeight<T>(ICollection<T> originalCollection, double[] weights, double sum, double randomValue)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                // Normalize weight
                double weight = Math.Round(sum, 2).Equals(0.00)
                    ? 1 / (double)originalCollection.Count
                    : weights.ElementAtOrDefault(i) / sum;

                if (randomValue < weight)
                {
                    return originalCollection.ElementAtOrDefault(i);
                }

                randomValue -= weight;
            }

            throw new InvalidOperationException("Unable to produce a result from the given collection using the supplied selector.");
        }
    }
}
