using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Models.RecipieModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.RecipieService
{
    public class RecipieService : IRecipieService
    {
        //Services
        private readonly RecipiesDbContext _recipiesContext;

        public RecipieService(RecipiesDbContext recipiesContext)
        {
            _recipiesContext = recipiesContext;
        }

        public async Task<bool> DoesRecipieExistsAsync(int id)
        {
            var recipie = await _recipiesContext.Recipie.FirstOrDefaultAsync(r => r.RecipieId == id && !r.IsDeleted);
            return recipie != null;
        }

        public async Task<List<Recipie>> GetRecipiesAsync(int userId)
        {
            return await _recipiesContext.Recipie.Where(r => !r.IsDeleted && r.UserId == userId)
                .Include(r => r.Ingredients).ToListAsync();
        }

        public async Task<Recipie> GetRecipieByIdAsync(int id)
        {
            return await _recipiesContext.Recipie.Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.RecipieId == id);
        }

        public async Task<Recipie> CreateRecipieAsync(Recipie recipie)
        {
            _recipiesContext.Add(recipie);
            await _recipiesContext.SaveChangesAsync();
            return recipie;
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            var recipie = await GetRecipieByIdAsync(id);
            recipie.IsDeleted = true;
            return await _recipiesContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HardDeleteRecipeAsync(int id)
        {
            var recipie = await GetRecipieByIdAsync(id);
            _recipiesContext.Recipie.Remove(recipie);
            return await _recipiesContext.SaveChangesAsync() > 0;
        }

        public async Task<Recipie> UpdateRecipieAsync(int id, Recipie recipie)
        {
            var updated = await GetRecipieByIdAsync(id);
            updated.RecipieId = id;
            updated.Name = recipie.Name;
            updated.Method = recipie.Method;
            updated.IsVegan = recipie.IsVegan;
            updated.IsVegetarian = recipie.IsVegetarian;
            updated.TimeToCook = recipie.TimeToCook;
            updated.Ingredients = recipie.Ingredients;

            await _recipiesContext.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> UserOwnsRecipie(int userId, int recipieId)
        {
            //AsNoTracking disable EF to track these object
            var recipie = await _recipiesContext.Recipie.AsNoTracking()
                .FirstOrDefaultAsync(r => r.RecipieId == recipieId);

            return recipie?.UserId == userId;
        }
    }
}