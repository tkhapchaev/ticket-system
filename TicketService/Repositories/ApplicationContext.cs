using Microsoft.EntityFrameworkCore;
using TicketService.Entities.Implementations;

namespace TicketService.Repositories;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketPriority> TicketPriorities => Set<TicketPriority>();
    public DbSet<TicketStatus> TicketStatuses => Set<TicketStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Login).IsUnique();
        modelBuilder.Entity<User>().Property(user => user.Login).IsRequired();

        modelBuilder.Entity<Project>().Property(project => project.Name).IsRequired();

        modelBuilder.Entity<Ticket>().Property(ticket => ticket.Title).IsRequired();
        modelBuilder.Entity<Ticket>().Property(ticket => ticket.Description).IsRequired();
        modelBuilder.Entity<Ticket>().HasOne(ticket => ticket.Reporter).WithMany().HasForeignKey(ticket => ticket.ReporterId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Ticket>().HasOne(ticket => ticket.Assignee).WithMany().HasForeignKey(ticket => ticket.AssigneeId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Ticket>().HasOne(ticket => ticket.Project).WithMany(project => project.Tickets).HasForeignKey(ticket => ticket.ProjectId);
        modelBuilder.Entity<Ticket>().HasOne(ticket => ticket.Priority).WithMany().HasForeignKey(ticket => ticket.PriorityId);
        modelBuilder.Entity<Ticket>().HasOne(ticket => ticket.Status).WithMany().HasForeignKey(ticket => ticket.StatusId);

        modelBuilder.Entity<TicketPriority>().Property(ticketPriority => ticketPriority.Name).IsRequired();

        modelBuilder.Entity<TicketStatus>().Property(ticketStatus => ticketStatus.Name).IsRequired();

        modelBuilder.Entity<TicketPriority>().HasData(
            new TicketPriority(1, "Low"),
            new TicketPriority(2, "Medium"),
            new TicketPriority(3, "High")
        );

        modelBuilder.Entity<TicketStatus>().HasData(
            new TicketStatus(1, "New"),
            new TicketStatus(2, "InProgress"),
            new TicketStatus(3, "Done")
        );
    }
}