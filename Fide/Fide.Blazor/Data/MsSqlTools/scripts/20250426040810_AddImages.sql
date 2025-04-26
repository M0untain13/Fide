IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250426040810_AddImages')
BEGIN

CREATE TABLE [ImageLinks] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] nvarchar(450) NULL,
    [Url] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ImageLinks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ImageLinks_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);
CREATE INDEX [IX_ImageLinks_UserId] ON [ImageLinks] ([UserId]);
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250426040810_AddImages', N'8.0.11');

END;
GO
