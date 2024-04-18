using System;
using Forum.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Mappings;

internal class TopicMapping : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("topic");
        
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();
        
        builder.Property(e => e.Creator)
            .HasColumnName("Creator")
            .HasMaxLength(256)
            .IsRequired();
        
        
        builder.Property(e => e.Subject)
            .HasColumnName("subject")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(e => e.CreatorId)
            .HasColumnName("CreatorId")
            .IsRequired();

        builder.Property(e => e.CreateDate)
            .HasColumnName("CreateDate")
            .HasColumnType("timestamp")
            .HasConversion(
                v => v, // Use default value
                v => v == default ? default : DateTime.SpecifyKind((DateTime)v, DateTimeKind.Utc) // Convert to UTC DateTime
            );
        
        builder.Property(e => e.UpdateDate)
            .HasColumnName("UpdateDate")
            .HasColumnType("timestamp")
            .HasConversion(
                v => v, // Use default value
                v => v == default ? default : DateTime.SpecifyKind((DateTime)v, DateTimeKind.Utc) // Convert to UTC DateTime
            );
        
        builder.Property(e => e.Likes)
            .HasColumnName("Likes")
            .HasColumnType("int");

        builder.Property(e => e.Status)
            .HasColumnName("Status");
        
        
    }
}