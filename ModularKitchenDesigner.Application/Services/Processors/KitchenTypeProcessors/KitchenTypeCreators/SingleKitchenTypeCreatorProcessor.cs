﻿using Microsoft.EntityFrameworkCore;
using ModularKitchenDesigner.Domain.Dto;
using ModularKitchenDesigner.Domain.Entityes;
using ModularKitchenDesigner.Domain.Interfaces.Processors;
using ModularKitchenDesigner.Domain.Interfaces.Validators;
using Newtonsoft.Json;
using Repository;
using Result;

namespace ModularKitchenDesigner.Application.Services.Processors.KitchenTypeProcessors.KitchenTypeCreator
{
    public sealed class SingleKitchenTypeCreatorProcessor : ICreatorProcessor<KitchenTypeDto>
    {
        private IRepositoryFactory _repositoryFactory;
        private IValidatorFactory _validatorFactory;

        public ICreatorProcessor<KitchenTypeDto> SetRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            return this;
        }

        public ICreatorProcessor<KitchenTypeDto> SetValidatorFactory(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
            return this;
        }

        public async Task<BaseResult<KitchenTypeDto>> ProcessAsync(KitchenTypeDto model)
        {
            var kitchenTypeResult = await _repositoryFactory.GetRepository<KitchenType>().GetAllAsync(predicate: x => x.Code == model.Code);

            _validatorFactory
                .GetCreateValidator()
                .Validate(
                    models: kitchenTypeResult,
                    preffix: "",
                    $"Object: SingleKitchenTypeCreator.CreateAsync(KitchenTypeDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            var priceSegmentResult = _validatorFactory
                .GetObjectNullValidator()
                .Validate(
                    model: (await _repositoryFactory.GetRepository<PriceSegment>().GetAllAsync(predicate: x => x.Title == model.PriceSegment)).FirstOrDefault(),
                    preffix: "",
                    $"Object: SingleKitchenTypeCreator.CreateAsync(KitchenTypeDto model)", $"Argument: {JsonConvert.SerializeObject(model, Formatting.Indented)}");

            KitchenType kitchenTypeCreatorResult = await _repositoryFactory
                .GetRepository<KitchenType>()
                .CreateAsync(
                    new KitchenType()
                    {
                        Title = model.Title,
                        Code = model.Code,
                        PriceSegmentId = priceSegmentResult.Id,
                    });

            var newKitchenType = (await _repositoryFactory
                .GetRepository<KitchenType>()
                .GetAllAsync(
                    include: query => query.Include(x => x.PriceSegment),
                    predicate: x => x.Code == model.Code
                ))
                .FirstOrDefault();

            return new()
            {
                Data = new(newKitchenType)
            };
        }
    }
}
