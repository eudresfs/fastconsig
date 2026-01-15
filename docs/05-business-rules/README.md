# Regras de Negócio

Documentação das regras de negócio, permissões e fluxos de aprovação do FastConsig.

## Documentos

### [regras-negocio.md](./regras-negocio.md)

**Conteúdo:** Regras de negócio numeradas e detalhadas do NOVO sistema

Especificação completa das regras com identificadores únicos:
- **RN-FUN-###:** Módulo de Funcionários
- **RN-MAR-###:** Módulo de Margem
- **RN-AVE-###:** Módulo de Averbações (Empréstimos)
- **RN-APR-###:** Módulo de Aprovações
- **RN-SIM-###:** Módulo de Simulações
- **RN-CON-###:** Módulo de Conciliação
- **RN-IMP-###:** Módulo de Importação
- **RN-REL-###:** Módulo de Relatórios

**Formato das Regras:**
```markdown
#### RN-MAR-005: Cálculo de Margem Disponível

**Fórmula:**
```
MargemDisponivel = MargemBruta - SomaParcelasAverbadas
MargemBruta = Salario × (PercentualMargemBruta / 100)
```

**Validações:**
- Margem disponível nunca pode ser negativa
- Percentual configurável por tenant (padrão 35%)
```

> **Nota:** Este documento especifica regras do NOVO sistema Node.js/TypeScript.
> Para referência do sistema legado .NET, consulte [../archive/dotnet/business-rules.md](../archive/dotnet/business-rules.md)

---

### [permissoes-detalhadas.md](./permissoes-detalhadas.md)

**Conteúdo:** RBAC (Role-Based Access Control) matrix

Sistema de permissões multi-tenant:

**Conceitos:**
- **User:** Usuário autenticado (via Clerk)
- **Profile (Perfil):** Conjunto de permissões (Admin, Operador, Aprovador, Viewer)
- **Permission:** Ação específica (criar, editar, deletar, aprovar)
- **Tenant:** Isolamento de dados (Consignante ou Consignatária)
- **Scope:** Abrangência (Own, Consignee, All)

**Tipos de Usuário:**

| Tipo | Tenant | Perfis Disponíveis |
|------|--------|--------------------|
| **Consignante (Órgão)** | Órgão público | Admin, Operador RH, Aprovador, Viewer |
| **Consignatária (Banco)** | Instituição financeira | Admin, Operador, Agente, Viewer |

**Matriz de Permissões por Módulo:**
- Funcionários: Criar, Editar, Inativar, Visualizar
- Averbações: Criar, Aprovar, Rejeitar, Cancelar, Visualizar
- Margem: Consultar, Reservar, Liberar
- Conciliação: Importar, Validar, Gerar Arquivo
- Relatórios: Gerar, Exportar, Auditar
- Configurações: Alterar Regras, Gerenciar Usuários

---

## Módulos de Negócio

### 1. Gestão de Funcionários (RN-FUN)

**Regras principais:**
- **RN-FUN-001:** Identificação única por matrícula + CPF
- **RN-FUN-005:** Restrições por situação funcional
- **RN-FUN-007:** Sistema de bloqueios (completo, por produto, por empresa)
- **RN-FUN-010:** Histórico de alterações obrigatório

**Entidade:** `Employee` (packages/database/schema/employees.ts)

---

### 2. Cálculo de Margem (RN-MAR)

**Regras críticas:**
- **RN-MAR-001:** Cálculo da margem bruta (Salário × %)
- **RN-MAR-005:** Cálculo da margem disponível (Bruta - Parcelas)
- **RN-MAR-006:** Bloqueio de averbação se margem insuficiente
- **RN-MAR-008:** Margem Viva (atualização em tempo real por eventos)

**Fórmulas:**
```
MargemBruta = Salario × (PercentualMargemBruta / 100)
MargemDisponivel = MargemBruta - SomaParcelasAverbadas
```

**Entidade:** `Margin` (event-driven, atualizada por handlers)

---

### 3. Averbações (Empréstimos) (RN-AVE)

**Ciclo de vida:**
1. **Pré-Reserva** → Margem reservada temporariamente
2. **Aguardando Aprovação** → Requer aprovação(ões)
3. **Averbado** → Contrato ativo, parcelas geradas
4. **Liquidado** → Quitado (manual ou refinanciamento)
5. **Cancelado** → Rejeitado ou cancelado

**Regras principais:**
- **RN-AVE-002:** Geração de número único de protocolo
- **RN-AVE-005:** Validação de margem antes de averbar
- **RN-AVE-008:** Geração automática de parcelas
- **RN-AVE-012:** Portabilidade entre consignatárias

**Entidade:** `Loan` (packages/database/schema/loans.ts)

---

### 4. Fluxos de Aprovação (RN-APR)

**Tipos de aprovação:**
- Aprovação do Consignante (Órgão)
- Aprovação da Consignatária (Banco)
- Confirmação do Funcionário

**Regras:**
- **RN-APR-001:** Fluxos configuráveis por produto
- **RN-APR-003:** Aprovação pode ter prazo de expiração
- **RN-APR-005:** Histórico completo de tramitações

**Workflow Engine:** Configurável via YAML

---

### 5. Conciliação Mensal (RN-CON)

**Processo:**
1. Importação de retornos bancários (CNAB)
2. Comparação com valores averbados
3. Identificação de divergências
4. Ajustes e fechamento
5. Geração de arquivo para folha

**Regras:**
- **RN-CON-002:** Destacar divergências > R$ 0,01
- **RN-CON-006:** Competência calculada por data de corte
- **RN-CON-009:** Arquivo de folha consolidado (todas as consignatárias)

---

## Relacionamento com Código

### Como encontrar implementação de uma regra

**Exemplo:** Implementar RN-MAR-005 (Cálculo de Margem Disponível)

1. **Domain Model:** `packages/database/schema/margins.ts`
2. **Business Logic:** `apps/api/src/modules/margins/margin.service.ts`
3. **Tests:** `apps/api/src/modules/margins/__tests__/margin.service.test.ts`

**Convenção de nomeação:**
```typescript
// apps/api/src/modules/margins/margin.service.ts

class MarginService {
  // RN-MAR-005: Cálculo de margem disponível
  calculateAvailableMargin(employee: Employee, loans: Loan[]): Decimal {
    const grossMargin = this.calculateGrossMargin(employee); // RN-MAR-001
    const usedMargin = loans.reduce(/* ... */); // RN-MAR-002
    return grossMargin.minus(usedMargin); // RN-MAR-005
  }
}
```

**Testes devem referenciar a regra:**
```typescript
describe('MarginService', () => {
  it('RN-MAR-005: deve calcular margem disponível corretamente', () => {
    // ...
  });
});
```

---

## Diferenças do Sistema Legado

### Margem: Sob Demanda → Event-Driven

**Legado (.NET):**
```csharp
// Calculada a cada consulta
public decimal CalcularMargemDisponivel(int idFuncionario) {
  var margem = Salario * 0.35m - SomaParcelas();
  return margem;
}
```

**Novo (Node.js):**
```typescript
// Pré-calculada e atualizada por eventos
class LoanCreatedHandler {
  async handle(event: LoanCreatedEvent) {
    await marginService.decreaseAvailable(event.employeeId, event.amount);
    // Margem atualizada em tempo real!
  }
}
```

### Aprovações: Hardcoded → Configurável

**Legado:** Lógica de aprovação em `if/else` no BLL

**Novo:** Workflow engine com definições YAML:
```yaml
workflows:
  loan_approval:
    steps:
      - actor: consignante
        action: approve
        required: true
      - actor: consignataria
        action: approve
        required: true
```

---

## Glossário de Termos

| Termo | Definição |
|-------|-----------|
| **Averbação** | Registro de um empréstimo consignado no sistema |
| **Consignante** | Órgão empregador que processa os descontos em folha |
| **Consignatária** | Instituição financeira que concede o empréstimo |
| **Margem** | Percentual do salário disponível para consignação |
| **Competência** | Mês/ano de referência (formato AAAA/MM) |
| **Portabilidade** | Transferência de dívida entre consignatárias |
| **Refinanciamento** | Liquidação de contrato existente para novo contrato |

---

## Relacionados

- **Arquitetura:** [../03-architecture/](../03-architecture/)
- **API Spec:** [../06-integrations/api-specification.md](../06-integrations/api-specification.md)
- **User Stories:** [../02-planning/user-stories.md](../02-planning/user-stories.md)
- **Regras Legadas:** [../archive/dotnet/business-rules.md](../archive/dotnet/business-rules.md)

---

*Última atualização: Janeiro 2026*
