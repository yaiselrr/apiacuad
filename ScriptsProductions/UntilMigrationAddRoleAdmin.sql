IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [Names] VARCHAR(40) NULL,
        [FirstLastName] VARCHAR(40) NULL,
        [SecondLastName] VARCHAR(40) NULL,
        [Title] VARCHAR(4) NULL,
        [MobileNumber] VARCHAR(10) NULL,
        [Sex] VARCHAR(9) NULL,
        [Address] VARCHAR(255) NULL,
        [IsActive] bit NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
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
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [Properties] (
        [PropertyNumber] nvarchar(450) NOT NULL,
        [GeneralDataUserId] nvarchar(450) NOT NULL,
        [DerivativeNumber] nvarchar(max) NULL,
        [TakeNumber] nvarchar(max) NULL,
        [MeterNumber] nvarchar(max) NULL,
        [TypeService] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsOwner] bit NOT NULL,
        [IsEnabled] bit NOT NULL,
        CONSTRAINT [PK_Properties] PRIMARY KEY ([PropertyNumber], [GeneralDataUserId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [CreditCards] (
        [Id] int NOT NULL IDENTITY,
        [CardNumber] VARCHAR(16) NOT NULL,
        [ExpiredDate] nvarchar(max) NOT NULL,
        [UserName] nvarchar(16) NOT NULL,
        [IsDefault] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [GeneralDataUserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_CreditCards] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CreditCards_AspNetUsers_GeneralDataUserId] FOREIGN KEY ([GeneralDataUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE TABLE [UsersProperties] (
        [GeneralDataUserId] nvarchar(450) NOT NULL,
        [PropertyId] nvarchar(450) NOT NULL,
        [PropertyNumber] nvarchar(450) NULL,
        [PropertyGeneralDataUserId] nvarchar(450) NULL,
        CONSTRAINT [PK_UsersProperties] PRIMARY KEY ([GeneralDataUserId], [PropertyId]),
        CONSTRAINT [FK_UsersProperties_AspNetUsers_GeneralDataUserId] FOREIGN KEY ([GeneralDataUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UsersProperties_Properties_PropertyNumber_PropertyGeneralDataUserId] FOREIGN KEY ([PropertyNumber], [PropertyGeneralDataUserId]) REFERENCES [Properties] ([PropertyNumber], [GeneralDataUserId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [IX_CreditCards_GeneralDataUserId] ON [CreditCards] ([GeneralDataUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    CREATE INDEX [IX_UsersProperties_PropertyNumber_PropertyGeneralDataUserId] ON [UsersProperties] ([PropertyNumber], [PropertyGeneralDataUserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174854_UploadFirstMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220414174854_UploadFirstMigration', N'5.0.15');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174914_SeedRole')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName', N'ConcurrencyStamp') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    VALUES (N''f9567dd7-d14d-45d2-b294-629779c0f549'', N''Customer'', N''CUSTOMER'', N''e1f3847b-fbe4-4ea4-b015-d538cad4e3dd'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName', N'ConcurrencyStamp') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174914_SeedRole')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName', N'ConcurrencyStamp') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    VALUES (N''15a5e761-a0f4-46fc-bad3-fee26745342a'', N''Admin'', N''ADMIN'', N''09991ec7-b325-4122-8190-6d44016fea94'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName', N'ConcurrencyStamp') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174914_SeedRole')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220414174914_SeedRole', N'5.0.15');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174959_AddAdminUser')
BEGIN
    INSERT INTO [dbo].[AspNetUsers] ([Id], [Names], [FirstLastName], [SecondLastName], [Title], [MobileNumber], [Sex], [Address], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive]) VALUES (N'812e6d6b-0fd3-4285-9936-fe300a43ff04', NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'admin@simas.torreon.com', N'ADMIN@SIMAS.TORREON.COM', N'admin@simas.torreon.com', N'ADMIN@SIMAS.TORREON.COM', 1, N'AQAAAAEAACcQAAAAEI3YYAWl3hab6pdKCAF5MbLY302ul1VXX5SZyHTFzH/PpsYlfhiA+6ykGkG/Wc3PPQ==', N'ADD4I5PSUNKPAYAYB2ZIF5NWVPSJVN4O', N'44253ff2-f00a-4299-ac1c-f3d18fd64e53', NULL, 0, 0, NULL, 1, 0, 1)
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174959_AddAdminUser')
BEGIN
    INSERT INTO [dbo].[AspNetUsers] ([Id], [Names], [FirstLastName], [SecondLastName], [Title], [MobileNumber], [Sex], [Address], [IsActive], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'adc23d8a-f3c0-4542-a7c6-5d204de9cdc4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'text@gmail.com', N'TEXT@GMAIL.COM', N'text@gmail.com', N'TEXT@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEHRT7kW2J3Vm97xyK5PU5SmNWz03UoykIZVKNRqMaE7b8Fbiyp/ap/DZUX/J4OPc0Q==', N'D3B3TWKKGZOP5Q7THQJWY4U26OV7LVMK', N'23f3f531-c9b1-44a2-aea3-98987dd2ff57', NULL, 0, 0, NULL, 1, 0)
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414174959_AddAdminUser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220414174959_AddAdminUser', N'5.0.15');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414175038_AddRoleAdmin')
BEGIN
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('812e6d6b-0fd3-4285-9936-fe300a43ff04', '15a5e761-a0f4-46fc-bad3-fee26745342a')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414175038_AddRoleAdmin')
BEGIN
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('adc23d8a-f3c0-4542-a7c6-5d204de9cdc4', 'f9567dd7-d14d-45d2-b294-629779c0f549')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220414175038_AddRoleAdmin')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220414175038_AddRoleAdmin', N'5.0.15');
END;
GO

COMMIT;
GO