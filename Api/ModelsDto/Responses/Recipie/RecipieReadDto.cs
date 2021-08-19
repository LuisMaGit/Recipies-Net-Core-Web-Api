using System.Collections.Generic;
using Api.ModelsDto.Common.Recipie;

namespace Api.ModelsDto.Responses.Recipie
{
    public class RecipieReadDto
    {
        public int RecipieId { get; set; }
        public string Name { get; set; }
        public TimeToCookDto TimeToCook { get; set; }
        public string Method { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public List<IngredientReadDto> Ingredients { get; set; }
    }
}