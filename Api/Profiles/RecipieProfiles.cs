using System;
using Api.Models.RecipieModel;
using Api.ModelsDto.Common.Recipie;
using Api.ModelsDto.Requests.Recipie;
using Api.ModelsDto.Responses.Recipie;
using AutoMapper;

namespace Api.Profiles
{
    public class RecipieProfiles : Profile
    {
        public RecipieProfiles()
        {
            //Source -> Map
            CreateMap<Recipie, RecipieReadDto>();
            CreateMap<RecipieCreateUpdateDto, Recipie>();
            CreateMap<Ingredient, IngredientReadDto>();
            CreateMap<IngredientCreateUpdateDto, Ingredient>();
            CreateMap<TimeToCookDto, TimeSpan>().ConvertUsing<TimeToCookDtoConverter>();
            CreateMap<TimeSpan, TimeToCookDto>().ConvertUsing<TimeSpanConverter>();
        }
    }

    internal class TimeToCookDtoConverter : ITypeConverter<TimeToCookDto, TimeSpan>
    {
        public TimeSpan Convert(TimeToCookDto source, TimeSpan destination, ResolutionContext context)
        {
            return new TimeSpan(0, source.Hours, source.Minutes, source.Seconds);
        }
    }

    internal class TimeSpanConverter : ITypeConverter<TimeSpan, TimeToCookDto>
    {
        public TimeToCookDto Convert(TimeSpan source, TimeToCookDto destination, ResolutionContext context)
        {
            return new TimeToCookDto()
            {
                Hours = source.Hours,
                Minutes = source.Minutes,
                Seconds = source.Seconds,
            };
        }
    }
}