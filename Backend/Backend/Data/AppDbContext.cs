using System.Reflection.Emit;
using BRMSAPI.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BRMSAPI.Data;

public class AppDbContext : IdentityDbContext<Passengers>
{
	public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
	{
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<PickUpPoint> PickUpPoints { get; set; }
    public DbSet<RegRoute> Route { get; set; }
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Schedule> Schedule { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<AssignBusRoute> AssignBusRoutes { get; set; }


    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    builder.Entity<Passengers>()
    //        .HasOne(p => p.Route)
    //        .WithOne(p => p.Passenger)
    //    .HasForeignKey(c => c.ArticleId);

    //    builder.Entity<IdentityUserLogin<string>>()
    //   .HasKey(login => new { login.UserId, login.LoginProvider, login.ProviderKey });

    //    builder.Entity<IdentityUserRole<string>>()
    //  .HasKey(ur => new { ur.UserId, ur.RoleId });


    //    builder.Entity<IdentityUserToken<string>>()
    //   .HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });


    //    builder.Entity<PickUpPoint>()
    //        .HasOne<Location>()
    //        .WithMany(e => e.PickUpPoints)
    //        .HasForeignKey(e => e.LocationId)
    //        .IsRequired(false);



    //    builder.Entity<Bus>()
    //       .HasOne<RegRoute>()
    //       .WithMany(p => p.Buses)
    //       .HasForeignKey(e => e.RegRouteId)
    //       .IsRequired(false);


    //    builder.Entity<RegRoute>()
    //        .HasMany<PickUpPoint>()
    //        .WithOne(e => e.Route)
    //        .HasForeignKey(e => e.RegRouteId)
    //        .IsRequired(false);


    //}
}

