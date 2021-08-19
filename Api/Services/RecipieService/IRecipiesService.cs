using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models.RecipieModel;

namespace Api.Services.RecipieService
{
    public interface IRecipieService
    {
        Task<bool> DoesRecipieExistsAsync(int id);
        Task<List<Recipie>> GetRecipiesAsync(int userId);
        Task<Recipie> GetRecipieByIdAsync(int id);
        Task<Recipie> CreateRecipieAsync(Recipie recipie);
        Task<bool> DeleteRecipeAsync(int id);
        Task<bool> HardDeleteRecipeAsync(int id);
        Task<Recipie> UpdateRecipieAsync(int id, Recipie recipie);

        Task<bool> UserOwnsRecipie(int userId, int recipieId);
    }
}