BEGIN TRANSACTION;
ALTER TABLE [tbProducts] ADD [BrandId] uniqueidentifier NULL;

ALTER TABLE [tbProducts] ADD [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit);

ALTER TABLE [tbProducts] ADD [InStock] bit NOT NULL DEFAULT CAST(1 AS bit);

CREATE TABLE [tbBrands] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_tbBrands] PRIMARY KEY ([Id])
);

CREATE TABLE [tbCharacteristics] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Enabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_tbCharacteristics] PRIMARY KEY ([Id])
);

CREATE TABLE [tbProductCharacteristics] (
    [Id] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [CharacteristicId] uniqueidentifier NOT NULL,
    [Value] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_tbProductCharacteristics] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_tbProductCharacteristics_tbCharacteristics_CharacteristicId] FOREIGN KEY ([CharacteristicId]) REFERENCES [tbCharacteristics] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_tbProductCharacteristics_tbProducts_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [tbProducts] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_tbProducts_BrandId] ON [tbProducts] ([BrandId]);

CREATE INDEX [IX_tbProductCharacteristics_CharacteristicId] ON [tbProductCharacteristics] ([CharacteristicId]);

CREATE INDEX [IX_tbProductCharacteristics_ProductId] ON [tbProductCharacteristics] ([ProductId]);

ALTER TABLE [tbProducts] ADD CONSTRAINT [FK_tbProducts_tbBrands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [tbBrands] ([Id]) ON DELETE SET NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260827203619_AddBrandsAndCharacteristics', N'9.0.2');

COMMIT;
GO

