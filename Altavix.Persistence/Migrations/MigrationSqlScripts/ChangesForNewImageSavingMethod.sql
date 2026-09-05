BEGIN TRANSACTION;
EXEC sp_rename N'[tbProductImages].[ImageContent]', N'ImagePath', 'COLUMN';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260905203051_ChangesForNewImageSavingMethod', N'10.0.9');

COMMIT;
GO

