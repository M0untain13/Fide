SET QUOTED_IDENTIFIER ON;

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL 
BEGIN

CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
);

END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema')
BEGIN

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(100) NOT NULL,
    [Name] nvarchar(100) NULL,
    [NormalizedName] nvarchar(100) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(100) NOT NULL,
    [UserName] nvarchar(100) NULL,
    [NormalizedUserName] nvarchar(100) NULL,
    [Email] nvarchar(100) NULL,
    [NormalizedEmail] nvarchar(100) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(100) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(100) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(100) NOT NULL,
    [ProviderKey] nvarchar(100) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(100) NOT NULL,
    [RoleId] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(100) NOT NULL,
    [LoginProvider] nvarchar(100) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'00000000000000_CreateIdentitySchema', N'8.0.11');

END;
GO
