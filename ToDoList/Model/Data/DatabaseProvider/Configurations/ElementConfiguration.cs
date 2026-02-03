using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Data.DatabaseProvider.Configurations
{
    internal class ElementConfiguration : IEntityTypeConfiguration<Element>
    {
        public void Configure(EntityTypeBuilder<Element> builder)
        {
            builder.HasKey(e => e.Id);  
        }
    }
}
