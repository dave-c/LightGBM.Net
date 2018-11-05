// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using LightGBMNet.Train;
using LightGBMNet.Tree;

namespace LightGBMNet.Train
{

    public sealed class BinaryTrainer : TrainerBase<double>
    {
        public override PredictionKind PredictionKind => PredictionKind.BinaryClassification;

        public BinaryTrainer(LearningParameters lp, ObjectiveParameters op, MetricParameters mp) : base(lp, op, mp)
        {
            if (lp.Objective != ObjectiveType.Binary)
                throw new Exception("Require Objective == ObjectiveType.Binary");
            if (mp.Metric == MetricType.DefaultMetric)
                mp.Metric = MetricType.BinaryLogLoss;
        }

        private protected override IPredictorWithFeatureWeights<double> CreatePredictor()
        {
            var pred = new BinaryPredictor(TrainedEnsemble, FeatureCount, AverageOutput);
            var cali = new PlattCalibrator(-Objective.Sigmoid);
            return new CalibratedPredictor(pred, cali);
        }
        
    }

}
