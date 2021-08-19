using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.ModelsDto.Common.Recipie;

namespace Api.ModelsDto.Requests.Recipie
{
    public class RecipieCreateUpdateDto
    {
        [Required] public string Name { get; set; }
        public TimeToCookDto TimeToCook { get; set; }
        public string Method { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public List<IngredientCreateUpdateDto> Ingredients { get; set; }
    }
}