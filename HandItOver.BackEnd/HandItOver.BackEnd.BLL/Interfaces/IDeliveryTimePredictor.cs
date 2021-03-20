using HandItOver.BackEnd.BLL.Models.DeliveryTimePredictor;
using System;

namespace HandItOver.BackEnd.BLL.Interfaces
{
    public interface IDeliveryTimePredictor
    {
        TimeSpan Predict(DeliveryPredictionData newDelivery);
    }
}