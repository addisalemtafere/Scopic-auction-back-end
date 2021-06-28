namespace Application.SeedSampleData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Seeder
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IDateTime dateTime;
        private readonly IUserManager userManager;

        public Seeder(IAuctionSystemDbContext context, IDateTime dateTime, IUserManager userManager)
        {
            this.context = context;
            this.dateTime = dateTime;
            this.userManager = userManager;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            await SeedUsers(cancellationToken);
            await SeedCategories(context, cancellationToken);
            await SeedItems(context, userManager, cancellationToken);
        }

        private async Task SeedUsers(CancellationToken cancellationToken)
        {
            if (!await context.Users.AnyAsync())
            {
                var allUsers = new List<AuctionUser>();
                for (var i = 1; i <= 2; i++)
                {
                    var user = new AuctionUser
                    {
                        Email = $"test{i}@test.com",
                        FullName = $"Test Testov{i}",
                        UserName = $"test{i}@test.com",
                        EmailConfirmed = true
                    };

                    allUsers.Add(user);
                }

                foreach (var user in allUsers)
                {
                    await userManager.CreateUserAsync(user, "test123");
                    await context.RefreshTokens.AddAsync(new RefreshToken
                    {
                        Token = Guid.NewGuid(),
                        JwtId = Guid.NewGuid().ToString(),
                        CreationDate = dateTime.UtcNow,
                        ExpiryDate = dateTime.UtcNow.AddMonths(AppConstants.RefreshTokenExpirationTimeInMonths),
                        Invalidated = false,
                        UserId = user.Id
                    });
                }

                var admin = new AuctionUser
                {
                    Email = "admin@admin.com",
                    FullName = "Admin Adminski",
                    UserName = "admin@admin.com",
                    EmailConfirmed = true
                };

                await userManager.CreateUserAsync(admin, "admin123");
                await SeedAdminRole();
                await userManager.AddToRoleAsync(admin, AppConstants.AdministratorRole);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task SeedAdminRole()
        {
            await userManager.CreateRoleAsync(new IdentityRole(AppConstants.AdministratorRole));
        }

        private static async Task SeedCategories(IAuctionSystemDbContext dbContext, CancellationToken cancellationToken)
        {
            if (!dbContext.Categories.Any())
            {
                var categories =
                    await File.ReadAllTextAsync(Path.GetFullPath(AppConstants.CategoriesPath), cancellationToken);

                var deserializedCategoriesWithSubCategories =
                    JsonConvert.DeserializeObject<CategoryDto[]>(categories);

                var allCategories = deserializedCategoriesWithSubCategories.Select(deserializedCategory => new Category
                {
                    Name = deserializedCategory.Name,
                    SubCategories = deserializedCategory.SubCategoryNames.Select(deserializedSubCategory =>
                        new SubCategory
                        {
                            Name = deserializedSubCategory.Name
                        }).ToList()
                }).ToList();

                await dbContext.Categories.AddRangeAsync(allCategories, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task SeedItems(IAuctionSystemDbContext dbContext,
            IUserManager manager,
            CancellationToken cancellationToken)
        {
            if (!dbContext.Items.Any())
            {
                var random = new Random();
                var allItems = new List<Item>();
                foreach (var category in dbContext.Categories.Include(c => c.SubCategories))
                {
                    var i = 1;
                    foreach (var subCategory in category.SubCategories)
                    {
                        var startTime = dateTime.UtcNow.AddDays(random.Next(0, 5)).ToUniversalTime();
                        var item = new Item
                        {
                            Description = $"Test Description_{i}",
                            Title = $"Test Title_{i}",
                            StartTime = startTime,
                            EndTime = startTime.AddHours(random.Next(1, 10)),
                            StartingPrice = random.Next(10, 500),
                            MinIncrease = random.Next(1, 100),
                            SubCategoryId = subCategory.Id,
                            Pictures = new List<Picture>
                            {
                                new() {Url = AppConstants.DefaultPictureUrl, Created = dateTime.UtcNow}
                            },
                            UserId = await manager.GetFirstUserId()
                        };

                        i++;
                        allItems.Add(item);
                    }
                }

                await dbContext.Items.AddRangeAsync(allItems, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private class CategoryDto
        {
            public string Name { get; set; }

            public SubCategoryDto[] SubCategoryNames { get; set; }
        }

        private class SubCategoryDto
        {
            public string Name { get; set; }
        }
    }
}