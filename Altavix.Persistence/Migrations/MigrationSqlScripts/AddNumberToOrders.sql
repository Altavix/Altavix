BEGIN TRANSACTION;
CREATE SEQUENCE [OrderNumbers] START WITH 10000 INCREMENT BY 1 NO CYCLE;

ALTER TABLE [tbOrders] ADD [Number] bigint NOT NULL DEFAULT (NEXT VALUE FOR OrderNumbers);

CREATE UNIQUE INDEX [IX_tbOrders_Number] ON [tbOrders] ([Number]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260815212153_AddNumberToOrders', N'10.0.9');

COMMIT;
GO

