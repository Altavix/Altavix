BEGIN TRANSACTION;
ALTER TABLE [tbProductImages] ADD [Position] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260905222634_AddProductImagePosition', N'10.0.9');

COMMIT;
GO

