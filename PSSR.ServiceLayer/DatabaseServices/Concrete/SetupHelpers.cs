using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PSSR.DataLayer.EfCode;
using System;
using System.Linq;

namespace PSSR.ServiceLayer.DatabaseServices.Concrete
{
    public enum DbStartupModes { UseExisting, EnsureCreated, EnsureDeletedCreated, UseMigrations }

    public static class SetupHelpers
    {
        public const string SeedDataSearchName = "RefineryData*.json";
        public const string SeedFileSubDirectory = "seedData";
        private const decimal DefaultBookPrice = 40;    //Any book without a price is set to this value

        public static void DevelopmentEnsureDeleted(this EfCoreContext db)
        {
            db.Database.EnsureDeleted();
        }

        public static void DevelopmentEnsureCreated(this EfCoreContext db)
        {
            db.Database.EnsureCreated();
        }

        public static int SeedDatabase(this EfCoreContext context, string dataDirectory)
        {
            if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                throw new InvalidOperationException("The database does not exist. If you are using Migrations then run PMC command update-database to create it");

            var persons = context.Persons.Count();
            if (persons == 0)
            {
                //context.Persons.Add(new Person("Amin","sahranavard","0323526462","09389723798"));

                //var contractor = new Contractor("Sample Contractor", "02111111111", "Sample Address", DateTime.Now);
                //var project = new Project("Sample Project", 0, DateTime.Now, DateTime.Now.AddDays(30));
                //contractor.Projects.Add(project);
                //context.Contractors.Add(contractor);

                //context.ProjectRoadMaps.Add(new WorkPackage("PreCommissioning"));
                //context.ProjectRoadMaps.Add(new WorkPackage("Commissioning"));
                //context.LocationTypes.Add(new LocationType("Yard"));
                //context.LocationTypes.Add(new LocationType("Site"));
                //context.SaveChanges();

                context.Database.ExecuteSqlCommand("CREATE TYPE [dbo].[UpdateTaskWFType] AS TABLE([ActivityId] [bigint] NOT NULL,[WF] [real] NOT NULL)");
                context.Database.ExecuteSqlCommand("CREATE TYPE [dbo].[UpdateWBSWF] AS TABLE([Id][bigint] NOT NULL,[WF][real] NOT NULL)");

                context.Database.ExecuteSqlCommand("Create PROCEDURE [dbo].[UpdateTaskWF](@MAsset UpdateTaskWFType READONLY) AS begin SET NOCOUNT OFF; MERGE INTO OrganizationResources.Activity b USING @MAsset a ON b.Id=a.ActivityId WHEN MATCHED Then update  set b.WeightFactor=a.WF; end ");
                context.Database.ExecuteSqlCommand("CREATE PROCEDURE [dbo].[UpdateWBSWF](@MAsset UpdateWBSWF READONLY) AS begin SET NOCOUNT OFF; MERGE INTO OrganizationResources.ProjectWBS as b USING @MAsset a ON b.Id = a.Id WHEN MATCHED Then update  set b.WF = a.WF; end");
            }

            return 1;
        }

        /// <summary>
        /// This wipes all the existing orders and creates a new set of orders
        /// </summary>
        /// <param name="context"></param>
        /// <param name="books"></param>
        //public static void ResetOrders(this EfCoreContext context, List<Book> books = null)
        //{
        //    context.RemoveRange(context.Orders.ToList());        //remove the existing orders (the lineitems will go too)
        //    context.AddDummyOrders(books);              //add a dummy set of orders
        //    context.SaveChanges();
        //}

        //------------------------------------------------------
        //private methods


        //private static readonly string[] DummyUsersIds = new[]
        //{
        //    CheckoutCookieService.DefaultUserId,
        //    "albert@einstein.com",
        //    "ada@lovelace.co.uk"
        //};

        //private static void AddDummyOrders(this EfCoreContext context, List<Book> books = null)
        //{
        //    if (books == null)
        //        books = context.Books.ToList();

        //    var orders = new List<Order>();
        //    var i = 0;
        //    foreach (var usersId in DummyUsersIds)
        //    {
        //        orders.Add(BuildDummyOrder(usersId, DateTime.UtcNow.AddDays(-10), books[i++]));
        //        orders.Add(BuildDummyOrder(usersId, DateTime.UtcNow, books[i++]));
        //    }
        //    context.AddRange(orders);
        //}

        //private static Order BuildDummyOrder(string userId, DateTime orderDate, Book bookOrdered)
        //{
        //    var deliverDay = orderDate.AddDays(5);
        //    var bookOrders = new List<OrderBooksDto>() { new OrderBooksDto(1, bookOrdered, 1) };
        //    return Order.CreateOrderFactory(userId, deliverDay, bookOrders)?.Result;
        //}
    }
}
