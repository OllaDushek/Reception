﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Reception.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ReseptionEntities : DbContext
    {
        public ReseptionEntities()
            : base("name=ReseptionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CheckIn> CheckIn { get; set; }
        public virtual DbSet<ClassRoom> ClassRoom { get; set; }
        public virtual DbSet<GroupVisitors> GroupVisitors { get; set; }
        public virtual DbSet<ListDopService> ListDopService { get; set; }
        public virtual DbSet<ListService> ListService { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Visitor> Visitor { get; set; }
        public virtual DbSet<Worker> Worker { get; set; }
    }
}