﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SCIMSQLConn
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class employeesEntities : DbContext
{
    public employeesEntities()
        : base("name=employeesEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<department> departments { get; set; }

    public virtual DbSet<dept_emp> dept_emp { get; set; }

    public virtual DbSet<dept_manager> dept_manager { get; set; }

    public virtual DbSet<employee> employees { get; set; }

    public virtual DbSet<salary> salaries { get; set; }

    public virtual DbSet<title> titles { get; set; }

}

}

