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

CREATE TABLE [Authors] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Books] (
    [BookId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [PublishDate] date NOT NULL,
    [BasePrice] decimal(18,2) NOT NULL,
    [AuthorId] int NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([BookId]),
    CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Books_AuthorId] ON [Books] ([AuthorId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231125142357_initial', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FirstName', N'LastName') AND [object_id] = OBJECT_ID(N'[Authors]'))
    SET IDENTITY_INSERT [Authors] ON;
INSERT INTO [Authors] ([Id], [FirstName], [LastName])
VALUES (1, N'Rhoda', N'Lerman'),
(2, N'Ruth', N'Ozeki'),
(3, N'Sofia', N'Segovia'),
(4, N'Ursula K.', N'LeGuin'),
(5, N'Hugh', N'Howey'),
(6, N'Isabelle', N'Allende');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FirstName', N'LastName') AND [object_id] = OBJECT_ID(N'[Authors]'))
    SET IDENTITY_INSERT [Authors] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231125185013_SeedAuthors', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Authors].[Id]', N'AuthorId', N'COLUMN';
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'BookId', N'AuthorId', N'BasePrice', N'PublishDate', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] ON;
INSERT INTO [Books] ([BookId], [AuthorId], [BasePrice], [PublishDate], [Title])
VALUES (1, 1, 0.0, '1989-03-01', N'In God''s Ear'),
(2, 2, 0.0, '2013-12-31', N'A Tale For the Time Being'),
(3, 3, 0.0, '1969-03-01', N'The Left Hand of Darkness');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'BookId', N'AuthorId', N'BasePrice', N'PublishDate', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231130162953_authoridchange', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Artists] (
    [ArtistId] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Artists] PRIMARY KEY ([ArtistId])
);
GO

CREATE TABLE [Covers] (
    [CoverId] int NOT NULL IDENTITY,
    [DesignIdeas] nvarchar(max) NOT NULL,
    [DigitalOnly] bit NOT NULL,
    CONSTRAINT [PK_Covers] PRIMARY KEY ([CoverId])
);
GO

CREATE TABLE [ArtistCover] (
    [ArtistsArtistId] int NOT NULL,
    [CoversCoverId] int NOT NULL,
    CONSTRAINT [PK_ArtistCover] PRIMARY KEY ([ArtistsArtistId], [CoversCoverId]),
    CONSTRAINT [FK_ArtistCover_Artists_ArtistsArtistId] FOREIGN KEY ([ArtistsArtistId]) REFERENCES [Artists] ([ArtistId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ArtistCover_Covers_CoversCoverId] FOREIGN KEY ([CoversCoverId]) REFERENCES [Covers] ([CoverId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_ArtistCover_CoversCoverId] ON [ArtistCover] ([CoversCoverId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240407131703_M_M', N'8.0.3');
GO

COMMIT;
GO

