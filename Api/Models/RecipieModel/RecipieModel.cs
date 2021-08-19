using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Models.Identity.DB;

namespace Api.Models.RecipieModel
{
    public class Recipie
    {
        public int RecipieId { get; set; }
        [Required] [MaxLength(100)] public string Name { get; set; }
        public TimeSpan TimeToCook { get; set; }
        public bool IsDeleted { get; set; } = false;
        [MaxLength(200)] public string Method { get; set; }
        public bool IsVegetarian { get; set; } = false;
        public bool IsVegan { get; set; } = false;
        public List<Ingredient> Ingredients { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }
    }
}