using Microsoft.EntityFrameworkCore;

namespace DotnetHomework.Models
{
    /// <summary>
    /// Represents the data context for the application, inheriting from DbContext.
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet for documents.
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for tags.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for document data.
        /// </summary>
        public DbSet<DocumentData> DocumentData { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for the many-to-many relationship between documents and tags.
        /// </summary>
        public DbSet<DocumentTag> DocumentTags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            EnsureDatabaseCreated();
        }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types 
        /// exposed in DbSet properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Document configuration
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);

                // One-to-One relation with DocumentData
                entity.HasOne(d => d.Data)
                      .WithOne(dd => dd.Document)
                      .HasForeignKey<DocumentData>(dd => dd.DocumentId);
            });

            // Tag configuration
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            });

            // DocumentTag configuration (Many-to-Many relationship)
            modelBuilder.Entity<DocumentTag>(entity =>
            {
                entity.HasKey(dt => new { dt.DocumentId, dt.TagId });

                entity.HasOne(dt => dt.Document)
                      .WithMany(d => d.DocumentTags)
                      .HasForeignKey(dt => dt.DocumentId);

                entity.HasOne(dt => dt.Tag)
                      .WithMany(t => t.DocumentTags)
                      .HasForeignKey(dt => dt.TagId);
            });

            // DocumentData configuration
            modelBuilder.Entity<DocumentData>(entity =>
            {
                entity.HasKey(e => e.DocumentId);
                entity.Property(e => e.FileData).IsRequired();
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            });
        }

        /// <summary>
        /// Ensures that the database for the context exists. If it exists, no action is taken.
        /// If it does not exist then the database and all its schema are created.
        /// </summary>
        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
        }
    }
}
