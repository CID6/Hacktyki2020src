﻿using System;
using System.Collections.Generic;

namespace RabbitEntityConsumer.Models
{
    public partial class CarFeatureCarModel
    {
        public int CarFeatureId { get; set; }
        public int CarModelId { get; set; }

        public virtual CarFeatures CarFeature { get; set; }
        public virtual CarModels CarModel { get; set; }
    }
}