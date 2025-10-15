using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
    using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configuration
{
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(X => X.CreatedAt)
       .HasColumnName("BookingDate")
       .HasDefaultValueSql("GetDate()");

            builder.HasKey(X => new { X.MemberId, X.SessionId });
            builder.Ignore(X => X.Id);
        }
    }
}
