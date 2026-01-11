using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();
                if (HasPlans && HasCategories) return false;
                if (!HasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any()) dbContext.Plans.AddRange(plans);


                }
                if (!HasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any()) dbContext.Categories.AddRange(categories);

                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName) 
        {
            //D:\Csharp projects\GymManagementSystemSolution\GymManagementPL  Directory.GetCurrentDirectory()
            //\wwwroot\Files\plans.json
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
            if (!File.Exists(FilePath)) throw new FileNotFoundException($"The file at path {FilePath} was not found.");
            
            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
