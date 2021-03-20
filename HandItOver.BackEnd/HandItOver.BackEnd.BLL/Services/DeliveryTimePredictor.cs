using HandItOver.BackEnd.BLL.Interfaces;
using HandItOver.BackEnd.BLL.Models.DeliveryTimePredictor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HandItOver.BackEnd.BLL.Services
{
    public class DeliveryTimePredictor : IDeliveryTimePredictor
    {
        private readonly IEnumerable<DeliveryPredictionData> deliveries;

        public DeliveryTimePredictor(IEnumerable<DeliveryPredictionData> deliveries)
        {
            if (deliveries == null || !deliveries.Any())
            {
                throw new ArgumentNullException(
                    paramName: nameof(deliveries),
                    message: "Data must contain at least one entry."
                );
            }
            this.deliveries = deliveries;
        }

        public TimeSpan Predict(DeliveryPredictionData newDelivery)
        {
            var clusters = GetDeliveryClustersByHours();
            var distancesToClusters = FindDistancesToClusters(newDelivery, clusters);
            int nearestCluster = FindNearestCluster(distancesToClusters);

            return TimeSpan.FromHours(nearestCluster);
        }

        protected virtual IList<IGrouping<int, DeliveryPredictionData>> GetDeliveryClustersByHours()
        {
            return this.deliveries.GroupBy(
                k => k.Duration.TotalHours switch
                {
                    <= 4 => 4,
                    <= 12 => 12,
                    <= 24 => 24,
                    <= 48 => 48,
                    <= 72 => 72,
                    _ => 168
                },
                v => v
            ).ToList();
        }

        protected virtual IDictionary<int, float> FindDistancesToClusters(
            DeliveryPredictionData newDelivery, 
            IList<IGrouping<int, DeliveryPredictionData>> clusters)
        {
            return clusters.ToDictionary(
                cluster => cluster.Key,
                cluster =>
                {
                    var clusterCenter = new DeliveryPredictionData(
                        DayOfWeek: FindMostFrequent(cluster.Select(d => d.DayOfWeek)),
                        Hour: FindMostFrequent(cluster.Select(d => d.Hour)),
                        Season: FindMostFrequent(cluster.Select(d => d.Season)),
                        Weight: cluster.Average(d => d.Weight),
                        Duration: TimeSpan.Zero
                    );
                    float dispersion = cluster.Sum(d => DistanceSqrBetweenPoints(d, clusterCenter)) / cluster.Count();
                    float probability = DistanceSqrBetweenPoints(newDelivery, clusterCenter);
                    return probability / dispersion;
                }
            );
        }

        private static int FindNearestCluster(IDictionary<int, float> distancesToClusters)
        {
            KeyValuePair<int, float> nearestCluster = distancesToClusters.First();
            foreach (var cluster in distancesToClusters.Skip(1))
            {
                if (cluster.Value < nearestCluster.Value)
                {
                    nearestCluster = cluster;
                }
            }

            return nearestCluster.Key;
        }

        protected virtual float DistanceSqrBetweenPoints(DeliveryPredictionData first, DeliveryPredictionData second)
        {
            int daysDistance = DistanceBetweenDaysOfWeek(first.DayOfWeek, second.DayOfWeek);
            int hoursDistance = DistanceBetweenHours(first.Hour, second.DayOfWeek);
            int seasonDistance = DistanceBetweenSeasons(first.Season, second.Season);
            float weightDistance = Math.Abs(first.Weight - second.Weight);

            const float maxDaysDistance = 3f;
            float daysDistanceNorm = daysDistance / maxDaysDistance;

            const float maxHoursDistance = 12f;
            float hoursDistanceNorm = hoursDistance / maxHoursDistance;

            const float maxSeasonDistance = 2f;
            float seasonDistanceNorm = seasonDistance / maxSeasonDistance;

            float weightDistanceNorm = 1f / (1f + (float)Math.Exp(-weightDistance));

            float squaredDistance =
                daysDistanceNorm * daysDistanceNorm
                + hoursDistanceNorm * hoursDistanceNorm
                + seasonDistanceNorm * seasonDistanceNorm
                + weightDistanceNorm * weightDistanceNorm;

            return squaredDistance;
        }

        private static int FindMostFrequent(IEnumerable<int> daysOfWeek)
        {
            var daysCount = daysOfWeek.GroupBy(k => k, v => v)
                .ToDictionary(k => k.Key, v => v.Count());
            int maxFrequency = int.MinValue;
            int mostFrequentDay = -1;
            foreach (var dayFrequency in daysCount)
            {
                if (dayFrequency.Value > maxFrequency)
                {
                    mostFrequentDay = dayFrequency.Key;
                    maxFrequency = dayFrequency.Value;
                }
            }
            return mostFrequentDay;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DistanceBetweenDaysOfWeek(int first, int second)
        {
            int result = Math.Abs(first - second);
            return result > 3 ? 7 - result : result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DistanceBetweenHours(int first, int second)
        {
            int result = Math.Abs(first - second);
            return result > 12 ? 24 - result : result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DistanceBetweenSeasons(int first, int second)
        {
            int result = Math.Abs(first - second);
            return result > 3 ? 1 : result;
        }
    }
}
