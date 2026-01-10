INSERT INTO dbo.Recurso
( Nome ,
ArquivoHelp ,
Arquivo ,
IDModulo ,
Visivel ,
Objetivo ,
Bloqueado ,
Ativo ,
IDRecursoPai ,
Imagem ,
ParametrosAoIniciar ,
Ordem
)
VALUES  ( 'Simulação de Empréstimo' , -- Nome - varchar(50)
NULL , -- ArquivoHelp - varchar(100)
'WebUserControlSimulacaoEmprestimo' , -- Arquivo - varchar(150)
2 , -- IDModulo - int
0 , -- Visivel - bit
'Simulação de Empréstimo por Funcionário' , -- Objetivo - varchar(500)
0 , -- Bloqueado - bit
1 , -- Ativo - int
NULL , -- IDRecursoPai - int
'' , -- Imagem - varchar(50)
'' , -- ParametrosAoIniciar - varchar(50)
0  -- Ordem - int
)

INSERT INTO dbo.PermissaoRecurso
( IDPermissao, IDRecurso )
VALUES  ( 1, -- IDPermissao - int
xxx  -- IDRecurso inserido anteriormente - int
)

INSERT INTO dbo.PermissaoSistema
( IDPermissao, IDRecurso, IDPerfil )
VALUES  ( 1, -- IDPermissao - int
xxx, -- IDRecurso inserido anteriormente - int
2  -- IDPerfil - int
)

INSERT INTO dbo.PermissaoUsuario
( IDPermissao ,
IDRecurso ,
IDPerfil ,
IDEmpresa
)
VALUES  ( 1 , -- IDPermissao - int
xxx , -- IDRecurso inserido anteriormente - int
2 , -- IDPerfil - int
xxx  -- IDEmpresa consignante - int
)