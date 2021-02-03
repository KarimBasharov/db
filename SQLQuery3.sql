CREATE TABLE [dbo].[Vanemad] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Nimi]          VARCHAR (30) NULL,
    [Perekonnanimi]        VARCHAR (30) NOT NULL,
    [Email]         VARCHAR (30) NULL,
    [Opilane]       INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Vanemad_Toopilane] FOREIGN KEY ([Opilane]) REFERENCES [dbo].[opilane] ([Id]));