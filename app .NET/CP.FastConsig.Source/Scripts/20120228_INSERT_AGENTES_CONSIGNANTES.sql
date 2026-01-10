INSERT INTO  Recurso([Nome],[ArquivoHelp],[Arquivo],[IDModulo],[Visivel],[Objetivo],[Bloqueado],[Ativo],[IDRecursoPai],[Imagem],[ParametrosAoIniciar],[Ordem]) VALUES (N'Agentes',NULL,NULL,1,1,N'Gerenciamento de Agentes Bancários',0,1,NULL,N'IconeGerencial.png',NULL,7);
INSERT INTO  Recurso([Nome],[ArquivoHelp],[Arquivo],[IDModulo],[Visivel],[Objetivo],[Bloqueado],[Ativo],[IDRecursoPai],[Imagem],[ParametrosAoIniciar],[Ordem]) VALUES (N'Agentes',NULL,N'WebUserControlSubMenu',1,1,N'Permite o cadastro de Agentes',0,1,NULL,N'IconeConsignataria.png',NULL,3);
INSERT INTO  Recurso([Nome],[ArquivoHelp],[Arquivo],[IDModulo],[Visivel],[Objetivo],[Bloqueado],[Ativo],[IDRecursoPai],[Imagem],[ParametrosAoIniciar],[Ordem]) VALUES (N'Agentes',NULL,N'WebUserControlAgentesEdicao',1,0,N'Permite modificar, ou cadastrar um novo Agente',0,1,NULL,N'MenuGerenciarConsignatarias.png',NULL,NULL);

INSERT INTO  Recurso([Nome],[ArquivoHelp],[Arquivo],[IDModulo],[Visivel],[Objetivo],[Bloqueado],[Ativo],[IDRecursoPai],[Imagem],[ParametrosAoIniciar],[Ordem]) VALUES (N'Gerenciar Agentes ',NULL,NULL,1,1,N'Permite gereciamento de agentes consignatários',0,1,XXX,N'IconeGerencial.png',NULL,1); -- ID DO RECURSO INSERIDO ANTES
INSERT INTO  Recurso([Nome],[ArquivoHelp],[Arquivo],[IDModulo],[Visivel],[Objetivo],[Bloqueado],[Ativo],[IDRecursoPai],[Imagem],[ParametrosAoIniciar],[Ordem]) VALUES (N'Configurar Agentes',NULL,N'WebUserControlAgentes',1,1,N'Permite cadastrar novos agentes',0,1,XXX,N'MenuGerenciarConsignatarias.png',NULL,NULL); -- ID DO RECURSO INSERIDO ANTES

INSERT INTO dbo.PermissaoSistema ( IDPermissao, IDRecurso, IDPerfil ) VALUES  ( 1, XXX, 1 ) -- ID DO RECURSO INSERIDO ANTES
INSERT INTO dbo.PermissaoSistema ( IDPermissao, IDRecurso, IDPerfil ) VALUES  ( 1, XXX, 1 ) -- ID DO RECURSO INSERIDO ANTES
INSERT INTO dbo.PermissaoSistema ( IDPermissao, IDRecurso, IDPerfil ) VALUES  ( 1, XXX, 1 ) -- ID DO RECURSO INSERIDO ANTES
INSERT INTO dbo.PermissaoSistema ( IDPermissao, IDRecurso, IDPerfil ) VALUES  ( 1, XXX, 1 ) -- ID DO RECURSO INSERIDO ANTES
INSERT INTO dbo.PermissaoSistema ( IDPermissao, IDRecurso, IDPerfil ) VALUES  ( 1, XXX, 1 ) -- ID DO RECURSO INSERIDO ANTES

INSERT INTO dbo.PermissaoUsuario ( IDPermissao, IDRecurso, IDPerfil, IDEmpresa ) VALUES  ( 1, XXX, 1, YYY ) -- RECURSO, CONSIGNANTE
INSERT INTO dbo.PermissaoUsuario ( IDPermissao, IDRecurso, IDPerfil, IDEmpresa ) VALUES  ( 1, XXX, 1, YYY ) -- RECURSO, CONSIGNANTE
INSERT INTO dbo.PermissaoUsuario ( IDPermissao, IDRecurso, IDPerfil, IDEmpresa ) VALUES  ( 1, XXX, 1, YYY ) -- RECURSO, CONSIGNANTE
INSERT INTO dbo.PermissaoUsuario ( IDPermissao, IDRecurso, IDPerfil, IDEmpresa ) VALUES  ( 1, XXX, 1, YYY ) -- RECURSO, CONSIGNANTE
INSERT INTO dbo.PermissaoUsuario ( IDPermissao, IDRecurso, IDPerfil, IDEmpresa ) VALUES  ( 1, XXX, 1, YYY ) -- RECURSO, CONSIGNANTE