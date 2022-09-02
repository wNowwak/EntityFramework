using Microsoft.EntityFrameworkCore;

namespace MyBoards.Entities
{
    public class MyBoardsContext : DbContext
    {

        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItemState>()
                 .Property(s => s.Value)
                 .IsRequired()
                 .HasMaxLength(50);

            modelBuilder.Entity<Epic>()
                .Property(wi => wi.EndDate)
                .HasPrecision(3);

            modelBuilder.Entity<Task>()
                .Property(wi => wi.Activity)
                .HasMaxLength(200);

            modelBuilder.Entity<Task>()
                .Property(wi => wi.RemaningWork)
                .HasPrecision(14, 2);

            modelBuilder.Entity<Issue>()
                .Property(wi => wi.Efford)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<WorkItem>(eb =>
            {
                
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.Priority).HasDefaultValue(1);
                eb.HasMany(x => x.Comments)
                .WithOne(x => x.WorkItem)
                .HasForeignKey(x => x.WorkItemId);
                eb.HasOne(x => x.Author)
                .WithMany(x => x.WorkItems)
                .HasForeignKey(x => x.AuthorId);

                eb.HasMany(x => x.Tags)
                .WithMany(x => x.WorkItems)
                .UsingEntity<WorkItemTag>(
                    w => w.HasOne(wit => wit.Tag)
                    .WithMany()
                    .HasForeignKey(wit => wit.TagId),
                    w => w.HasOne(wit => wit.WorkItem)
                    .WithMany()
                    .HasForeignKey(wit => wit.WorkItemId),
                    
                    wit =>
                    {
                        wit.HasKey(x => new {x.TagId, x.WorkItemId});
                        wit.Property(x => x.PublicationDate)
                        .HasDefaultValueSql("getutcdate()");
                    }

                    );
                eb.HasOne(x => x.State)
                .WithMany()
                .HasForeignKey(x => x.StateId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<User>(eb =>
            {
                eb.HasOne(u => u.Address)
                .WithOne(u => u.User)
                .HasForeignKey<Address>(a => a.UserId);
            });

        }


    }
}
