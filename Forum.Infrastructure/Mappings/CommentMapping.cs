using Forum.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Mappings;

internal class CommentMapping : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comment");
        
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Creator)
            .HasColumnName("Creator")
            .IsRequired();
        
        builder.Property(e => e.CreatorId)
            .HasColumnName("CreatorId")
            .IsRequired();

        builder.Property(e => e.TopicId)
            .HasColumnName("TopicId")
            .IsRequired();
        
        builder.Property(e => e.Text)
            .HasColumnName("Text")
            .IsRequired();

        builder.Property(e => e.Likes)
            .HasColumnName("Likes");
        
        builder.Property(e => e.Status)
            .HasColumnName("Status");
        
        


    }
}