BEGIN TRANSACTION;
CREATE TABLE [tbDeliveryMethods] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Price] decimal(18,2) NOT NULL,
    [Type] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_tbDeliveryMethods] PRIMARY KEY ([Id])
);

CREATE TABLE [tbPaymentMethods] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(150) NOT NULL,
    [Type] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_tbPaymentMethods] PRIMARY KEY ([Id])
);

CREATE TABLE [tbOrders] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [Ordered] datetime2 NULL,
    [Paid] datetime2 NULL,
    [Processing] datetime2 NULL,
    [Shipped] datetime2 NULL,
    [Delivered] datetime2 NULL,
    [Cancelled] datetime2 NULL,
    [DeliveryMethodId] uniqueidentifier NULL,
    [PaymentMethodId] uniqueidentifier NULL,
    [ClientId] uniqueidentifier NULL,
    [ClientName] nvarchar(150) NOT NULL,
    [ClientMobilePhone] nvarchar(20) NOT NULL,
    [ClientEmail] nvarchar(150) NULL,
    [City] nvarchar(150) NULL,
    [CityRef] nvarchar(50) NULL,
    [Address] nvarchar(500) NULL,
    [Comment] nvarchar(1000) NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    [TotalPriceCoin] int NOT NULL,
    CONSTRAINT [PK_tbOrders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_tbOrders_tbDeliveryMethods_DeliveryMethodId] FOREIGN KEY ([DeliveryMethodId]) REFERENCES [tbDeliveryMethods] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_tbOrders_tbPaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [tbPaymentMethods] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_tbOrders_tbUsers_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [tbUsers] ([Id])
);

CREATE TABLE [tbOrderItems] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [UnitPriceCoin] int NOT NULL,
    [Created] datetime2 NOT NULL,
    [Ordered] datetime2 NULL,
    [Pending] datetime2 NULL,
    [ReadyToShip] datetime2 NULL,
    [Shipped] datetime2 NULL,
    [Cancelled] datetime2 NULL,
    [CancelReason] nvarchar(max) NULL,
    CONSTRAINT [PK_tbOrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_tbOrderItems_tbOrders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [tbOrders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_tbOrderItems_tbProducts_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [tbProducts] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_tbOrderItems_OrderId] ON [tbOrderItems] ([OrderId]);

CREATE INDEX [IX_tbOrderItems_ProductId] ON [tbOrderItems] ([ProductId]);

CREATE INDEX [IX_tbOrders_ClientId] ON [tbOrders] ([ClientId]);

CREATE INDEX [IX_tbOrders_DeliveryMethodId] ON [tbOrders] ([DeliveryMethodId]);

CREATE INDEX [IX_tbOrders_PaymentMethodId] ON [tbOrders] ([PaymentMethodId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260812204856_AddOrdersWithItemsDeliveryAndPayment', N'10.0.9');

COMMIT;
GO

