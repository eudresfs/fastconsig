insert into permissaorecurso (idpermissao, idrecurso) values (41, 303);
insert into permissaorecurso (idpermissao, idrecurso) values (42, 303);
insert into PermissaoUsuario(IDRecurso,IDEmpresa,IDPermissao,IDPerfil)
select pr.IDRecurso, e.idempresa, pr.IDPermissao, p.IDPerfil
from PermissaoRecurso pr, Recurso r, Empresa e, Perfil p
where pr.IDRecurso = r.IDRecurso and r.IDModulo = 3 and e.IDEmpresaTipo in (3,4) and p.IDModulo = 3
and r.IDRecurso = 303 and pr.IDPermissao in (1) and pr.idrecurso = 41;