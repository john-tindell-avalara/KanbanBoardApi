using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using Swashbuckle.Swagger;

namespace KanbanBoardApi
{
    public class AddFluentValidationRules : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            var validator = GetValidator(type);

            if (validator == null)
            {
                return;
            }

            schema.required = new List<string>();

            var validatorDescriptor = validator.CreateDescriptor();

            foreach (var key in schema.properties.Keys)
            {
                foreach (var propertyValidator in validatorDescriptor.GetValidatorsForMember(key))
                {
                    if (propertyValidator is NotEmptyValidator)
                    {
                        schema.required.Add(key);
                    }

                    if (propertyValidator is LengthValidator)
                    {
                        var lengthValidator = (LengthValidator) propertyValidator;
                        if (lengthValidator.Max > 0)
                        {
                            schema.properties[key].maxLength = lengthValidator.Max;
                        }

                        schema.properties[key].minLength = lengthValidator.Min;
                    }

                    if (propertyValidator is RegularExpressionValidator)
                    {
                        var regexExpressionValidator = (RegularExpressionValidator) propertyValidator;
                        schema.properties[key].pattern = regexExpressionValidator.Expression;
                    }
                }
                
            }
        }

        private IValidator GetValidator(Type t)
        {
            var type = typeof (IValidator<>).MakeGenericType(t);
            try
            {
                return WebApiApplication.Container.GetInstance(type) as IValidator;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}