using AnitaSMS.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnitaSMS.Infrastructure.Entity_Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Courses>
    {
        public void Configure(EntityTypeBuilder<Courses> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(s => s.CourseName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.CourseDescription)
                .IsRequired()
                .HasMaxLength(500);


            builder.Property(p => p.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("DATETIME");


            builder.Property(e => e.CreatedBy)
                //.IsRequired()
                .IsUnicode(true);


            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime");

            builder.Property(e => e.ModifiedBy)
                //.IsRequired()
                .IsUnicode(false);


            builder.HasMany(e => e.Student)
                .WithOne(pt => pt.Course)
                .HasForeignKey(e => e.CourseId);

        }
    }
}
