CREATE TABLE [tb_Postagens] (
	id bigint NOT NULL,
	Titulo varchar(255) NOT NULL,
	Texto varchar(5000) NOT NULL,
	Data datetime NOT NULL,
	TemaId bigint NOT NULL,
	UsuarioId bigint NOT NULL,
  CONSTRAINT [PK_TB_POSTAGENS] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [tb_tema] (
	id bigint NOT NULL,
	Descricao varchar(255) NOT NULL,
  CONSTRAINT [PK_TB_TEMA] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [tb_Usuario] (
	id bigint NOT NULL,
	Nome varchar(255) NOT NULL,
	Email varchar(255) NOT NULL,
	Senha varchar(255) NOT NULL,
	Foto varchar(5000) NOT NULL,
  CONSTRAINT [PK_TB_USUARIO] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
ALTER TABLE [tb_Postagens] WITH CHECK ADD CONSTRAINT [tb_Postagens_fk0] FOREIGN KEY ([TemaId]) REFERENCES [tb_tema]([id])
ON UPDATE CASCADE
GO
ALTER TABLE [tb_Postagens] CHECK CONSTRAINT [tb_Postagens_fk0]
GO
ALTER TABLE [tb_Postagens] WITH CHECK ADD CONSTRAINT [tb_Postagens_fk1] FOREIGN KEY ([UsuarioId]) REFERENCES [tb_Usuario]([id])
ON UPDATE CASCADE
GO
ALTER TABLE [tb_Postagens] CHECK CONSTRAINT [tb_Postagens_fk1]
GO



