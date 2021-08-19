namespace Api.Models.RecipieModel

{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public int RecipieId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
    }
}