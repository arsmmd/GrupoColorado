CREATE DATABASE [GrupoColorado]
GO


USE [GrupoColorado]
GO

CREATE TABLE [dbo].[Clientes](
	[CodigoCliente] [int] IDENTITY(1,1) NOT NULL,
	[RazaoSocial] [varchar](100) NOT NULL,
	[NomeFantasia] [varchar](100) NOT NULL,
	[TipoPessoa] [char](1) NOT NULL,
	[Documento] [varchar](14) NOT NULL,
	[Endereco] [varchar](100) NOT NULL,
	[Complemento] [varchar](100) NULL,
	[Bairro] [varchar](100) NOT NULL,
	[Cidade] [varchar](100) NOT NULL,
	[CEP] [char](8) NOT NULL,
	[UF] [char](2) NOT NULL,
	[DataInsercao] [datetime] NOT NULL,
	[UsuarioInsercao] [int] NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[CodigoCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Telefones](
	[CodigoCliente] [int] NOT NULL,
	[NumeroTelefone] [varchar](11) NOT NULL,
	[CodigoTipoTelefone] [int] NOT NULL,
	[Operadora] [varchar](100) NOT NULL,
	[Ativo] [bit] NOT NULL,
	[DataInsercao] [datetime] NOT NULL,
	[UsuarioInsercao] [int] NOT NULL,
 CONSTRAINT [PK_Telefones] PRIMARY KEY CLUSTERED 
(
	[CodigoCliente] ASC,
	[NumeroTelefone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TiposTelefone](
	[CodigoTipoTelefone] [int] IDENTITY(1,1) NOT NULL,
	[DescricaoTipoTelefone] [varchar](80) NOT NULL,
	[DataInsercao] [datetime] NOT NULL,
	[UsuarioInsercao] [int] NOT NULL,
 CONSTRAINT [PK_TiposTelefone] PRIMARY KEY CLUSTERED 
(
	[CodigoTipoTelefone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Usuarios](
	[CodigoUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Email] [varchar](250) NOT NULL,
	[Senha] [varchar](75) NOT NULL,
	[Ativo] [bit] NOT NULL,
	[DataInsercao] [datetime] NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[CodigoUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Clientes] ADD  CONSTRAINT [DF_Clientes_DataInsercao]  DEFAULT (getdate()) FOR [DataInsercao]
GO

ALTER TABLE [dbo].[Telefones] ADD  CONSTRAINT [DF_Telefones_Ativo]  DEFAULT ((1)) FOR [Ativo]
GO

ALTER TABLE [dbo].[Telefones] ADD  CONSTRAINT [DF_Telefones_DataInsercao]  DEFAULT (getdate()) FOR [DataInsercao]
GO

ALTER TABLE [dbo].[TiposTelefone] ADD  CONSTRAINT [DF_TiposTelefone_DataInsercao]  DEFAULT (getdate()) FOR [DataInsercao]
GO

ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Ativo]  DEFAULT ((1)) FOR [Ativo]
GO

ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_DataInsercao]  DEFAULT (getdate()) FOR [DataInsercao]
GO

ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Usuarios] FOREIGN KEY([UsuarioInsercao])
REFERENCES [dbo].[Usuarios] ([CodigoUsuario])
GO

ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Usuarios]
GO

ALTER TABLE [dbo].[Telefones]  WITH CHECK ADD  CONSTRAINT [FK_Telefones_Clientes] FOREIGN KEY([CodigoCliente])
REFERENCES [dbo].[Clientes] ([CodigoCliente])
GO

ALTER TABLE [dbo].[Telefones] CHECK CONSTRAINT [FK_Telefones_Clientes]
GO

ALTER TABLE [dbo].[Telefones]  WITH CHECK ADD  CONSTRAINT [FK_Telefones_TiposTelefone] FOREIGN KEY([CodigoTipoTelefone])
REFERENCES [dbo].[TiposTelefone] ([CodigoTipoTelefone])
GO

ALTER TABLE [dbo].[Telefones] CHECK CONSTRAINT [FK_Telefones_TiposTelefone]
GO

ALTER TABLE [dbo].[Telefones]  WITH CHECK ADD  CONSTRAINT [FK_Telefones_Usuarios] FOREIGN KEY([UsuarioInsercao])
REFERENCES [dbo].[Usuarios] ([CodigoUsuario])
GO

ALTER TABLE [dbo].[Telefones] CHECK CONSTRAINT [FK_Telefones_Usuarios]
GO

ALTER TABLE [dbo].[TiposTelefone]  WITH CHECK ADD  CONSTRAINT [FK_TiposTelefone_Usuarios] FOREIGN KEY([UsuarioInsercao])
REFERENCES [dbo].[Usuarios] ([CodigoUsuario])
GO

ALTER TABLE [dbo].[TiposTelefone] CHECK CONSTRAINT [FK_TiposTelefone_Usuarios]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_Email] ON [dbo].[Usuarios] ([Email] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Telefones] ON [dbo].[Telefones] ([CodigoCliente] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

INSERT Usuarios (Nome, Email, Senha) VALUES ('Administrador', 'admin@grupocolorado.com.br', '10000.XvMI8mHXwXb2vGZz7h+3HA==.hSuFcdIYBosLOhJ+pXrtyCDEEyIjpi1VOh/lbJE9Ne0=')
GO

-- Senha padrão do Administrador: entrada#CRUD