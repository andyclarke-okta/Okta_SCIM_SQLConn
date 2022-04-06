
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/30/2017 13:19:45
-- Generated from EDMX file: C:\Okta\SCIM_OPP\Projects_Csharp\OPP_SQLServer\SCIMSQLConn\EmployeeModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [employees];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_dept_emp_dept_emp_ibfk_2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[dept_emp] DROP CONSTRAINT [FK_dept_emp_dept_emp_ibfk_2];
GO
IF OBJECT_ID(N'[dbo].[FK_dept_manager_dept_manager_ibfk_2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[dept_manager] DROP CONSTRAINT [FK_dept_manager_dept_manager_ibfk_2];
GO
IF OBJECT_ID(N'[dbo].[FK_dept_emp_dept_emp_ibfk_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[dept_emp] DROP CONSTRAINT [FK_dept_emp_dept_emp_ibfk_1];
GO
IF OBJECT_ID(N'[dbo].[FK_dept_manager_dept_manager_ibfk_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[dept_manager] DROP CONSTRAINT [FK_dept_manager_dept_manager_ibfk_1];
GO
IF OBJECT_ID(N'[dbo].[FK_salaries_salaries_ibfk_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[salaries] DROP CONSTRAINT [FK_salaries_salaries_ibfk_1];
GO
IF OBJECT_ID(N'[dbo].[FK_titles_titles_ibfk_1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[titles] DROP CONSTRAINT [FK_titles_titles_ibfk_1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[departments];
GO
IF OBJECT_ID(N'[dbo].[dept_emp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[dept_emp];
GO
IF OBJECT_ID(N'[dbo].[dept_manager]', 'U') IS NOT NULL
    DROP TABLE [dbo].[dept_manager];
GO
IF OBJECT_ID(N'[dbo].[employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[employees];
GO
IF OBJECT_ID(N'[dbo].[salaries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[salaries];
GO
IF OBJECT_ID(N'[dbo].[titles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[titles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'departments'
CREATE TABLE [dbo].[departments] (
    [dept_no] nchar(4)  NOT NULL,
    [dept_name] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'dept_emp'
CREATE TABLE [dbo].[dept_emp] (
    [emp_no] int  NOT NULL,
    [dept_no] nchar(4)  NOT NULL,
    [from_date] datetime  NOT NULL,
    [to_date] datetime  NOT NULL
);
GO

-- Creating table 'dept_manager'
CREATE TABLE [dbo].[dept_manager] (
    [dept_no] nchar(4)  NOT NULL,
    [emp_no] int  NOT NULL,
    [from_date] datetime  NOT NULL,
    [to_date] datetime  NOT NULL
);
GO

-- Creating table 'employees'
CREATE TABLE [dbo].[employees] (
    [emp_no] int IDENTITY(1,1) NOT NULL,
    [birth_date] datetime  NULL,
    [first_name] nvarchar(14)  NOT NULL,
    [last_name] nvarchar(16)  NOT NULL,
    [gender] nvarchar(1)  NULL,
    [hire_date] datetime  NULL,
    [username] nvarchar(max)  NOT NULL,
    [primary_email] nvarchar(max)  NOT NULL,
    [active] bit  NOT NULL
);
GO

-- Creating table 'salaries'
CREATE TABLE [dbo].[salaries] (
    [emp_no] int  NOT NULL,
    [salary1] int  NOT NULL,
    [from_date] datetime  NOT NULL,
    [to_date] datetime  NOT NULL
);
GO

-- Creating table 'titles'
CREATE TABLE [dbo].[titles] (
    [emp_no] int  NOT NULL,
    [title1] nvarchar(50)  NOT NULL,
    [from_date] datetime  NOT NULL,
    [to_date] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [dept_no] in table 'departments'
ALTER TABLE [dbo].[departments]
ADD CONSTRAINT [PK_departments]
    PRIMARY KEY CLUSTERED ([dept_no] ASC);
GO

-- Creating primary key on [emp_no], [dept_no] in table 'dept_emp'
ALTER TABLE [dbo].[dept_emp]
ADD CONSTRAINT [PK_dept_emp]
    PRIMARY KEY CLUSTERED ([emp_no], [dept_no] ASC);
GO

-- Creating primary key on [dept_no], [emp_no] in table 'dept_manager'
ALTER TABLE [dbo].[dept_manager]
ADD CONSTRAINT [PK_dept_manager]
    PRIMARY KEY CLUSTERED ([dept_no], [emp_no] ASC);
GO

-- Creating primary key on [emp_no] in table 'employees'
ALTER TABLE [dbo].[employees]
ADD CONSTRAINT [PK_employees]
    PRIMARY KEY CLUSTERED ([emp_no] ASC);
GO

-- Creating primary key on [emp_no], [from_date] in table 'salaries'
ALTER TABLE [dbo].[salaries]
ADD CONSTRAINT [PK_salaries]
    PRIMARY KEY CLUSTERED ([emp_no], [from_date] ASC);
GO

-- Creating primary key on [emp_no], [title1], [from_date] in table 'titles'
ALTER TABLE [dbo].[titles]
ADD CONSTRAINT [PK_titles]
    PRIMARY KEY CLUSTERED ([emp_no], [title1], [from_date] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [dept_no] in table 'dept_emp'
ALTER TABLE [dbo].[dept_emp]
ADD CONSTRAINT [FK_dept_emp_dept_emp_ibfk_2]
    FOREIGN KEY ([dept_no])
    REFERENCES [dbo].[departments]
        ([dept_no])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dept_emp_dept_emp_ibfk_2'
CREATE INDEX [IX_FK_dept_emp_dept_emp_ibfk_2]
ON [dbo].[dept_emp]
    ([dept_no]);
GO

-- Creating foreign key on [dept_no] in table 'dept_manager'
ALTER TABLE [dbo].[dept_manager]
ADD CONSTRAINT [FK_dept_manager_dept_manager_ibfk_2]
    FOREIGN KEY ([dept_no])
    REFERENCES [dbo].[departments]
        ([dept_no])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [emp_no] in table 'dept_emp'
ALTER TABLE [dbo].[dept_emp]
ADD CONSTRAINT [FK_dept_emp_dept_emp_ibfk_1]
    FOREIGN KEY ([emp_no])
    REFERENCES [dbo].[employees]
        ([emp_no])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [emp_no] in table 'dept_manager'
ALTER TABLE [dbo].[dept_manager]
ADD CONSTRAINT [FK_dept_manager_dept_manager_ibfk_1]
    FOREIGN KEY ([emp_no])
    REFERENCES [dbo].[employees]
        ([emp_no])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dept_manager_dept_manager_ibfk_1'
CREATE INDEX [IX_FK_dept_manager_dept_manager_ibfk_1]
ON [dbo].[dept_manager]
    ([emp_no]);
GO

-- Creating foreign key on [emp_no] in table 'salaries'
ALTER TABLE [dbo].[salaries]
ADD CONSTRAINT [FK_salaries_salaries_ibfk_1]
    FOREIGN KEY ([emp_no])
    REFERENCES [dbo].[employees]
        ([emp_no])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [emp_no] in table 'titles'
ALTER TABLE [dbo].[titles]
ADD CONSTRAINT [FK_titles_titles_ibfk_1]
    FOREIGN KEY ([emp_no])
    REFERENCES [dbo].[employees]
        ([emp_no])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------