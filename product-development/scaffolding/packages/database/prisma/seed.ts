import { PrismaClient, TipoPerfil } from '@prisma/client';

const prisma = new PrismaClient();

const PERMISSOES = [
  // Modulo Sistema
  { codigo: 'SISTEMA.ACESSAR', nome: 'Acessar Sistema', modulo: 'SISTEMA', descricao: 'Permite acesso basico ao sistema' },
  { codigo: 'SISTEMA.CONFIGURAR', nome: 'Configurar Sistema', modulo: 'SISTEMA', descricao: 'Permite alterar configuracoes do tenant' },

  // Modulo Usuarios
  { codigo: 'USUARIOS.VISUALIZAR', nome: 'Visualizar Usuarios', modulo: 'USUARIOS', descricao: 'Permite listar e ver detalhes de usuarios' },
  { codigo: 'USUARIOS.CRIAR', nome: 'Criar Usuario', modulo: 'USUARIOS', descricao: 'Permite cadastrar novos usuarios' },
  { codigo: 'USUARIOS.EDITAR', nome: 'Editar Usuario', modulo: 'USUARIOS', descricao: 'Permite alterar dados de usuarios' },
  { codigo: 'USUARIOS.EXCLUIR', nome: 'Excluir Usuario', modulo: 'USUARIOS', descricao: 'Permite remover usuarios' },
  { codigo: 'USUARIOS.BLOQUEAR', nome: 'Bloquear Usuario', modulo: 'USUARIOS', descricao: 'Permite bloquear acesso de usuarios' },

  // Modulo Funcionarios
  { codigo: 'FUNCIONARIOS.VISUALIZAR', nome: 'Visualizar Funcionarios', modulo: 'FUNCIONARIOS', descricao: 'Permite listar e ver detalhes de funcionarios' },
  { codigo: 'FUNCIONARIOS.CRIAR', nome: 'Criar Funcionario', modulo: 'FUNCIONARIOS', descricao: 'Permite cadastrar novos funcionarios' },
  { codigo: 'FUNCIONARIOS.EDITAR', nome: 'Editar Funcionario', modulo: 'FUNCIONARIOS', descricao: 'Permite alterar dados de funcionarios' },
  { codigo: 'FUNCIONARIOS.EXCLUIR', nome: 'Excluir Funcionario', modulo: 'FUNCIONARIOS', descricao: 'Permite remover funcionarios' },
  { codigo: 'FUNCIONARIOS.MARGEM', nome: 'Visualizar Margem', modulo: 'FUNCIONARIOS', descricao: 'Permite consultar margem do funcionario' },

  // Modulo Averbacoes
  { codigo: 'AVERBACOES.VISUALIZAR', nome: 'Visualizar Averbacoes', modulo: 'AVERBACOES', descricao: 'Permite listar e ver detalhes de averbacoes' },
  { codigo: 'AVERBACOES.CRIAR', nome: 'Criar Averbacao', modulo: 'AVERBACOES', descricao: 'Permite iniciar nova averbacao' },
  { codigo: 'AVERBACOES.APROVAR', nome: 'Aprovar Averbacao', modulo: 'AVERBACOES', descricao: 'Permite aprovar averbacoes pendentes' },
  { codigo: 'AVERBACOES.REJEITAR', nome: 'Rejeitar Averbacao', modulo: 'AVERBACOES', descricao: 'Permite rejeitar averbacoes' },
  { codigo: 'AVERBACOES.CANCELAR', nome: 'Cancelar Averbacao', modulo: 'AVERBACOES', descricao: 'Permite cancelar averbacoes' },

  // Modulo Consignatarias
  { codigo: 'CONSIGNATARIAS.VISUALIZAR', nome: 'Visualizar Consignatarias', modulo: 'CONSIGNATARIAS', descricao: 'Permite listar consignatarias' },
  { codigo: 'CONSIGNATARIAS.EDITAR', nome: 'Editar Consignataria', modulo: 'CONSIGNATARIAS', descricao: 'Permite alterar dados de consignatarias' },

  // Modulo Conciliacao
  { codigo: 'CONCILIACAO.EXECUTAR', nome: 'Executar Conciliacao', modulo: 'CONCILIACAO', descricao: 'Permite rodar processo de conciliacao' },
  { codigo: 'CONCILIACAO.TRATAR', nome: 'Tratar Divergencias', modulo: 'CONCILIACAO', descricao: 'Permite tratar itens divergentes' },
];

async function main() {
  console.log('🌱 Iniciando seed...');

  // 1. Criar permissoes
  console.log('🔐 Criando permissoes...');
  for (const perm of PERMISSOES) {
    await prisma.permissao.upsert({
      where: { codigo: perm.codigo },
      update: {},
      create: perm,
    });
  }

  // 2. Criar perfis padrao
  console.log('👤 Criando perfis padrao...');

  // Admin do Sistema (acesso total)
  const adminPerfil = await prisma.perfil.upsert({
    where: { nome: 'Administrador' },
    update: {},
    create: {
      nome: 'Administrador',
      descricao: 'Acesso total ao sistema',
      tipo: TipoPerfil.SISTEMA,
      sistema: true,
    },
  });

  // Gestor Consignante (RH do orgao)
  const gestorPerfil = await prisma.perfil.upsert({
    where: { nome: 'Gestor Consignante' },
    update: {},
    create: {
      nome: 'Gestor Consignante',
      descricao: 'Gestao de funcionarios e aprovacoes',
      tipo: TipoPerfil.CONSIGNANTE,
      sistema: true,
    },
  });

  // Operador Consignataria (Banco)
  const operadorPerfil = await prisma.perfil.upsert({
    where: { nome: 'Operador Consignataria' },
    update: {},
    create: {
      nome: 'Operador Consignataria',
      descricao: 'Operacoes de emprestimo e consultas',
      tipo: TipoPerfil.CONSIGNATARIA,
      sistema: true,
    },
  });

  // 3. Associar permissoes aos perfis
  console.log('🔗 Associando permissoes...');

  const todasPermissoes = await prisma.permissao.findMany();

  // Admin: todas as permissoes
  for (const perm of todasPermissoes) {
    await prisma.perfilPermissao.upsert({
      where: {
        perfilId_permissaoId: {
          perfilId: adminPerfil.id,
          permissaoId: perm.id,
        },
      },
      update: {},
      create: {
        perfilId: adminPerfil.id,
        permissaoId: perm.id,
      },
    });
  }

  console.log('✅ Seed finalizado com sucesso!');
}

main()
  .catch((e) => {
    console.error(e);
    process.exit(1);
  })
  .finally(async () => {
    await prisma.$disconnect();
  });
