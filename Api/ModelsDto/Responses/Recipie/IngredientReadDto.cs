namespace Api.ModelsDto.Responses.Recipie
{
    public class IngredientReadDto
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
    }
}