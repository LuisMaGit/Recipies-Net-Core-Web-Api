namespace Api.ModelsDto.Requests.Recipie
{
    public class IngredientCreateUpdateDto
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
    }
}