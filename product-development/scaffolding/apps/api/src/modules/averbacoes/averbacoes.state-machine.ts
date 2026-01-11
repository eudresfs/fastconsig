import type { SituacaoAverbacao } from '@prisma/client'
import { StateTransitionError } from '@/shared/errors'

/**
 * All possible averbacao states
 */
export const SituacaoAverbacaoValues = {
  AGUARDANDO_APROVACAO: 'AGUARDANDO_APROVACAO',
  APROVADA: 'APROVADA',
  REJEITADA: 'REJEITADA',
  ENVIADA: 'ENVIADA',
  DESCONTADA: 'DESCONTADA',
  SUSPENSA: 'SUSPENSA',
  BLOQUEADA: 'BLOQUEADA',
  LIQUIDADA: 'LIQUIDADA',
  CANCELADA: 'CANCELADA',
} as const

export type SituacaoAverbacaoType = (typeof SituacaoAverbacaoValues)[keyof typeof SituacaoAverbacaoValues]

/**
 * State transition rules for averbacoes
 * Maps current state to allowed target states
 */
const transitionRules: Record<SituacaoAverbacao, SituacaoAverbacao[]> = {
  // New averbacao awaiting approval
  AGUARDANDO_APROVACAO: [
    'APROVADA',    // Approved by consignante
    'REJEITADA',   // Rejected by consignante
    'CANCELADA',   // Cancelled by consignataria
  ],

  // Approved - ready to be sent to payroll
  APROVADA: [
    'ENVIADA',     // Sent to payroll file
    'SUSPENSA',    // Suspended temporarily
    'BLOQUEADA',   // Blocked by consignante
    'CANCELADA',   // Cancelled
  ],

  // Rejected - terminal state but can be resubmitted
  REJEITADA: [
    // No transitions - terminal state
    // New averbacao must be created for resubmission
  ],

  // Sent to payroll - awaiting discount
  ENVIADA: [
    'DESCONTADA',  // Successfully discounted
    'SUSPENSA',    // Suspended temporarily
    'BLOQUEADA',   // Blocked by consignante
  ],

  // Being discounted in payroll
  DESCONTADA: [
    'LIQUIDADA',   // Fully paid
    'SUSPENSA',    // Suspended temporarily
    'BLOQUEADA',   // Blocked by consignante
  ],

  // Temporarily suspended
  SUSPENSA: [
    'APROVADA',    // Reactivated to approved state
    'ENVIADA',     // Reactivated to sent state
    'DESCONTADA',  // Reactivated to discounting state
    'CANCELADA',   // Permanently cancelled
    'BLOQUEADA',   // Blocked by consignante
  ],

  // Blocked by consignante
  BLOQUEADA: [
    'APROVADA',    // Unblocked and reactivated
    'ENVIADA',     // Unblocked and sent
    'DESCONTADA',  // Unblocked and discounting
    'SUSPENSA',    // Changed to suspended
    'CANCELADA',   // Permanently cancelled
  ],

  // Fully paid - terminal state
  LIQUIDADA: [
    // No transitions - terminal state
  ],

  // Cancelled - terminal state
  CANCELADA: [
    // No transitions - terminal state
  ],
}

/**
 * States that consume margin (parcela is being reserved/used)
 */
export const ESTADOS_QUE_CONSOMEM_MARGEM: SituacaoAverbacao[] = [
  'AGUARDANDO_APROVACAO',
  'APROVADA',
  'ENVIADA',
  'DESCONTADA',
]

/**
 * States that are considered active (loan is ongoing)
 */
export const ESTADOS_ATIVOS: SituacaoAverbacao[] = [
  'AGUARDANDO_APROVACAO',
  'APROVADA',
  'ENVIADA',
  'DESCONTADA',
  'SUSPENSA',
  'BLOQUEADA',
]

/**
 * Terminal states (no further transitions allowed)
 */
export const ESTADOS_TERMINAIS: SituacaoAverbacao[] = [
  'REJEITADA',
  'LIQUIDADA',
  'CANCELADA',
]

/**
 * States that allow editing of averbacao data
 */
export const ESTADOS_EDITAVEIS: SituacaoAverbacao[] = [
  'AGUARDANDO_APROVACAO',
]

/**
 * Checks if a state transition is valid
 */
export function isTransitionValid(
  currentState: SituacaoAverbacao,
  targetState: SituacaoAverbacao
): boolean {
  const allowedTransitions = transitionRules[currentState]
  return allowedTransitions.includes(targetState)
}

/**
 * Gets all allowed transitions from a given state
 */
export function getAllowedTransitions(
  currentState: SituacaoAverbacao
): SituacaoAverbacao[] {
  return [...transitionRules[currentState]]
}

/**
 * Validates a state transition and throws if invalid
 */
export function validateTransition(
  currentState: SituacaoAverbacao,
  targetState: SituacaoAverbacao
): void {
  if (!isTransitionValid(currentState, targetState)) {
    throw new StateTransitionError('Averbacao', currentState, targetState, {
      allowedTransitions: getAllowedTransitions(currentState),
    })
  }
}

/**
 * Checks if a state is terminal (no further transitions)
 */
export function isTerminalState(state: SituacaoAverbacao): boolean {
  return ESTADOS_TERMINAIS.includes(state)
}

/**
 * Checks if a state consumes margin
 */
export function consumesMargem(state: SituacaoAverbacao): boolean {
  return ESTADOS_QUE_CONSOMEM_MARGEM.includes(state)
}

/**
 * Checks if a state is considered active
 */
export function isActiveState(state: SituacaoAverbacao): boolean {
  return ESTADOS_ATIVOS.includes(state)
}

/**
 * Checks if a state allows editing
 */
export function isEditableState(state: SituacaoAverbacao): boolean {
  return ESTADOS_EDITAVEIS.includes(state)
}

/**
 * Gets the appropriate action name for a state transition (for audit)
 */
export function getTransitionActionName(
  targetState: SituacaoAverbacao
): string {
  switch (targetState) {
    case 'APROVADA':
      return 'APROVAR'
    case 'REJEITADA':
      return 'REJEITAR'
    case 'SUSPENSA':
      return 'SUSPENDER'
    case 'CANCELADA':
      return 'CANCELAR'
    case 'BLOQUEADA':
      return 'BLOQUEAR'
    case 'ENVIADA':
      return 'ENVIAR'
    case 'DESCONTADA':
      return 'DESCONTAR'
    case 'LIQUIDADA':
      return 'LIQUIDAR'
    default:
      return 'ATUALIZAR'
  }
}

/**
 * State transition metadata for UI display
 */
export interface TransitionMetadata {
  state: SituacaoAverbacao
  label: string
  description: string
  color: string
  requiresObservacao: boolean
  requiresMotivoRejeicao: boolean
}

/**
 * Gets metadata for all states
 */
export function getStateMetadata(): Record<SituacaoAverbacao, TransitionMetadata> {
  return {
    AGUARDANDO_APROVACAO: {
      state: 'AGUARDANDO_APROVACAO',
      label: 'Aguardando Aprovacao',
      description: 'Averbacao pendente de aprovacao pelo consignante',
      color: 'yellow',
      requiresObservacao: false,
      requiresMotivoRejeicao: false,
    },
    APROVADA: {
      state: 'APROVADA',
      label: 'Aprovada',
      description: 'Averbacao aprovada e pronta para envio a folha',
      color: 'green',
      requiresObservacao: false,
      requiresMotivoRejeicao: false,
    },
    REJEITADA: {
      state: 'REJEITADA',
      label: 'Rejeitada',
      description: 'Averbacao rejeitada pelo consignante',
      color: 'red',
      requiresObservacao: false,
      requiresMotivoRejeicao: true,
    },
    ENVIADA: {
      state: 'ENVIADA',
      label: 'Enviada',
      description: 'Averbacao enviada para desconto em folha',
      color: 'blue',
      requiresObservacao: false,
      requiresMotivoRejeicao: false,
    },
    DESCONTADA: {
      state: 'DESCONTADA',
      label: 'Descontada',
      description: 'Averbacao sendo descontada em folha de pagamento',
      color: 'indigo',
      requiresObservacao: false,
      requiresMotivoRejeicao: false,
    },
    SUSPENSA: {
      state: 'SUSPENSA',
      label: 'Suspensa',
      description: 'Averbacao temporariamente suspensa',
      color: 'orange',
      requiresObservacao: true,
      requiresMotivoRejeicao: false,
    },
    BLOQUEADA: {
      state: 'BLOQUEADA',
      label: 'Bloqueada',
      description: 'Averbacao bloqueada pelo consignante',
      color: 'gray',
      requiresObservacao: true,
      requiresMotivoRejeicao: false,
    },
    LIQUIDADA: {
      state: 'LIQUIDADA',
      label: 'Liquidada',
      description: 'Averbacao totalmente quitada',
      color: 'emerald',
      requiresObservacao: false,
      requiresMotivoRejeicao: false,
    },
    CANCELADA: {
      state: 'CANCELADA',
      label: 'Cancelada',
      description: 'Averbacao cancelada permanentemente',
      color: 'red',
      requiresObservacao: true,
      requiresMotivoRejeicao: false,
    },
  }
}

/**
 * Gets available transition actions for a given state (for UI)
 */
export function getAvailableActions(
  currentState: SituacaoAverbacao
): Array<{
  targetState: SituacaoAverbacao
  action: string
  label: string
  variant: 'default' | 'destructive' | 'outline' | 'secondary'
}> {
  const allowed = getAllowedTransitions(currentState)

  return allowed.map((targetState) => {
    let variant: 'default' | 'destructive' | 'outline' | 'secondary' = 'default'
    let label = ''

    switch (targetState) {
      case 'APROVADA':
        label = 'Aprovar'
        variant = 'default'
        break
      case 'REJEITADA':
        label = 'Rejeitar'
        variant = 'destructive'
        break
      case 'ENVIADA':
        label = 'Enviar para Folha'
        variant = 'default'
        break
      case 'DESCONTADA':
        label = 'Marcar como Descontada'
        variant = 'default'
        break
      case 'SUSPENSA':
        label = 'Suspender'
        variant = 'secondary'
        break
      case 'BLOQUEADA':
        label = 'Bloquear'
        variant = 'secondary'
        break
      case 'LIQUIDADA':
        label = 'Liquidar'
        variant = 'default'
        break
      case 'CANCELADA':
        label = 'Cancelar'
        variant = 'destructive'
        break
      default:
        label = targetState
    }

    return {
      targetState,
      action: getTransitionActionName(targetState),
      label,
      variant,
    }
  })
}
