select * from recurso

set identity_insert recurso on
insert into Recurso(IDRecurso,Nome,Arquivo,IDModulo,Visivel,Bloqueado,Ativo,IDRecursoPai,Imagem,Ordem)
values (304,'Importar Aposentados','WebUserControlFuncionarioAposentar',1,1,0,1,3,'IconeGerencial.png',3)
set identity_insert recurso off

insert into PermissaoRecurso(IDRecurso,IDPermissao) values(304,1)

insert into PermissaoUsuario(IDRecurso,IDEmpresa,IDPermissao,IDPerfil)
select pr.IDRecurso, e.idempresa, pr.IDPermissao, p.IDPerfil
from PermissaoRecurso pr, Recurso r, Empresa e, Perfil p
where pr.IDRecurso = r.IDRecurso and r.IDModulo = 1 and e.IDEmpresaTipo in (2) and p.IDModulo = 1
and r.IDRecurso in (304) and pr.IDPermissao in (1) and p.IDPerfil in (1)

