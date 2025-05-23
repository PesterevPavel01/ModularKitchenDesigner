﻿using Interceptors;
using ModularKitchenDesigner.Domain.Entityes.Base;
using ModularKitchenDesigner.Domain.Interfaces;

namespace ModularKitchenDesigner.Domain.Entityes
{
    public class PriceSegment : SimpleEntity<PriceSegment>, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Component> Components { get; set; } = [];
        public List<KitchenType> Types { get; set; } = [];
    }
}