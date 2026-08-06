BEGIN TRANSACTION;
ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_Roles_RoleId];

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_Users_UserId];

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_Users_UserId];

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_Roles_RoleId];

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_Users_UserId];

ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_Users_UserId];

ALTER TABLE [CategoryEntityProductEntity] DROP CONSTRAINT [FK_CategoryEntityProductEntity_Categories_CategoriesId];

ALTER TABLE [CategoryEntityProductEntity] DROP CONSTRAINT [FK_CategoryEntityProductEntity_Products_ProductEntityId];

ALTER TABLE [ProductImages] DROP CONSTRAINT [FK_ProductImages_Products_ProductId];

ALTER TABLE [Products] DROP CONSTRAINT [FK_Products_Users_UserCreatorId];

ALTER TABLE [Users] DROP CONSTRAINT [PK_Users];

ALTER TABLE [Roles] DROP CONSTRAINT [PK_Roles];

ALTER TABLE [Products] DROP CONSTRAINT [PK_Products];

ALTER TABLE [ProductImages] DROP CONSTRAINT [PK_ProductImages];

ALTER TABLE [CategoryEntityProductEntity] DROP CONSTRAINT [PK_CategoryEntityProductEntity];

ALTER TABLE [Categories] DROP CONSTRAINT [PK_Categories];

ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [PK_AspNetUserTokens];

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [PK_AspNetUserRoles];

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [PK_AspNetUserLogins];

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [PK_AspNetUserClaims];

ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [PK_AspNetRoleClaims];

EXEC sp_rename N'[Users]', N'tbUsers', 'OBJECT';

EXEC sp_rename N'[Roles]', N'tbRoles', 'OBJECT';

EXEC sp_rename N'[Products]', N'tbProducts', 'OBJECT';

EXEC sp_rename N'[ProductImages]', N'tbProductImages', 'OBJECT';

EXEC sp_rename N'[CategoryEntityProductEntity]', N'tbCategoryProduct', 'OBJECT';

EXEC sp_rename N'[Categories]', N'tbCategories', 'OBJECT';

EXEC sp_rename N'[AspNetUserTokens]', N'tbUserTokens', 'OBJECT';

EXEC sp_rename N'[AspNetUserRoles]', N'tbUserRoles', 'OBJECT';

EXEC sp_rename N'[AspNetUserLogins]', N'tbUserLogins', 'OBJECT';

EXEC sp_rename N'[AspNetUserClaims]', N'tbUserClaims', 'OBJECT';

EXEC sp_rename N'[AspNetRoleClaims]', N'tbRoleClaims', 'OBJECT';

EXEC sp_rename N'[tbUsers].[IX_Users_Id]', N'IX_tbUsers_Id', 'INDEX';

EXEC sp_rename N'[tbProducts].[IX_Products_UserCreatorId]', N'IX_tbProducts_UserCreatorId', 'INDEX';

EXEC sp_rename N'[tbProductImages].[IX_ProductImages_ProductId]', N'IX_tbProductImages_ProductId', 'INDEX';

EXEC sp_rename N'[tbCategoryProduct].[IX_CategoryEntityProductEntity_ProductEntityId]', N'IX_tbCategoryProduct_ProductEntityId', 'INDEX';

EXEC sp_rename N'[tbCategories].[IX_Categories_Title]', N'IX_tbCategories_Title', 'INDEX';

EXEC sp_rename N'[tbUserRoles].[IX_AspNetUserRoles_RoleId]', N'IX_tbUserRoles_RoleId', 'INDEX';

EXEC sp_rename N'[tbUserLogins].[IX_AspNetUserLogins_UserId]', N'IX_tbUserLogins_UserId', 'INDEX';

EXEC sp_rename N'[tbUserClaims].[IX_AspNetUserClaims_UserId]', N'IX_tbUserClaims_UserId', 'INDEX';

EXEC sp_rename N'[tbRoleClaims].[IX_AspNetRoleClaims_RoleId]', N'IX_tbRoleClaims_RoleId', 'INDEX';

ALTER TABLE [tbUsers] ADD CONSTRAINT [PK_tbUsers] PRIMARY KEY ([Id]);

ALTER TABLE [tbRoles] ADD CONSTRAINT [PK_tbRoles] PRIMARY KEY ([Id]);

ALTER TABLE [tbProducts] ADD CONSTRAINT [PK_tbProducts] PRIMARY KEY ([Id]);

ALTER TABLE [tbProductImages] ADD CONSTRAINT [PK_tbProductImages] PRIMARY KEY ([Id]);

ALTER TABLE [tbCategoryProduct] ADD CONSTRAINT [PK_tbCategoryProduct] PRIMARY KEY ([CategoriesId], [ProductEntityId]);

ALTER TABLE [tbCategories] ADD CONSTRAINT [PK_tbCategories] PRIMARY KEY ([Id]);

ALTER TABLE [tbUserTokens] ADD CONSTRAINT [PK_tbUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]);

ALTER TABLE [tbUserRoles] ADD CONSTRAINT [PK_tbUserRoles] PRIMARY KEY ([UserId], [RoleId]);

ALTER TABLE [tbUserLogins] ADD CONSTRAINT [PK_tbUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]);

ALTER TABLE [tbUserClaims] ADD CONSTRAINT [PK_tbUserClaims] PRIMARY KEY ([Id]);

ALTER TABLE [tbRoleClaims] ADD CONSTRAINT [PK_tbRoleClaims] PRIMARY KEY ([Id]);

ALTER TABLE [tbCategoryProduct] ADD CONSTRAINT [FK_tbCategoryProduct_tbCategories_CategoriesId] FOREIGN KEY ([CategoriesId]) REFERENCES [tbCategories] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbCategoryProduct] ADD CONSTRAINT [FK_tbCategoryProduct_tbProducts_ProductEntityId] FOREIGN KEY ([ProductEntityId]) REFERENCES [tbProducts] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbProductImages] ADD CONSTRAINT [FK_tbProductImages_tbProducts_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [tbProducts] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbProducts] ADD CONSTRAINT [FK_tbProducts_tbUsers_UserCreatorId] FOREIGN KEY ([UserCreatorId]) REFERENCES [tbUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbRoleClaims] ADD CONSTRAINT [FK_tbRoleClaims_tbRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [tbRoles] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbUserClaims] ADD CONSTRAINT [FK_tbUserClaims_tbUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [tbUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbUserLogins] ADD CONSTRAINT [FK_tbUserLogins_tbUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [tbUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbUserRoles] ADD CONSTRAINT [FK_tbUserRoles_tbRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [tbRoles] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbUserRoles] ADD CONSTRAINT [FK_tbUserRoles_tbUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [tbUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [tbUserTokens] ADD CONSTRAINT [FK_tbUserTokens_tbUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [tbUsers] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260806193616_RenameTablesWithTbPrefix', N'10.0.9');

COMMIT;
GO

