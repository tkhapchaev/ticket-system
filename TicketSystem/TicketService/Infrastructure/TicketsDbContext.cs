using Microsoft.EntityFrameworkCore;
using TicketService.Entities;

namespace TicketService.Infrastructure;

public class TicketsDbContext : DbContext
{
    public TicketsDbContext(DbContextOptions<TicketsDbContext> o) : base(o)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Ticket>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).IsRequired();
            e.Property(x => x.ReporterEmail).IsRequired().HasMaxLength(200);
            e.Property(x => x.Priority).HasConversion<int>().IsRequired();
            e.Property(x => x.Status).HasConversion<int>().IsRequired();
            e.Property(x => x.CreatedAt).HasColumnType("timestamptz");
            e.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        });
    }
}