using System;
using FluentValidation;
using SimpleInjector;

namespace KanbanBoardApi
{
    public class FluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly Container container;

        public FluentValidatorFactory(Container container)
        {
            this.container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            try
            {
                return container.GetInstance(validatorType) as IValidator;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}