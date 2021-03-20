using System;

namespace HandItOver.BackEnd.BLL.Models.DeliveryTimePredictor
{
    public record DeliveryPredictionData(
        int DayOfWeek,
        int Hour,
        int Season,
        float Weight,
        TimeSpan Duration
    );
}
