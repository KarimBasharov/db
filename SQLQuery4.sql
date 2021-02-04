CREATE TABLE [dbo].[vanemad] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Nimi]          VARCHAR (50) NULL,
    [Perekonnanimi] VARCHAR (50) NULL,
    [Opilane]       INT          NULL,
    [Email]         VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Opilane]) REFERENCES [dbo].[opilane] ([Id])
);