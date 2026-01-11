import { describe, it, expect } from 'vitest'
import {
  validateTransition,
  isTransitionValid,
  isEditableState,
  consumesMargem,
  isTerminalState,
  getTransitionActionName,
  SituacaoAverbacaoValues,
  getAvailableActions,
  getStateMetadata,
} from '../averbacoes.state-machine'
import { StateTransitionError } from '@/shared/errors'

describe('Averbacoes State Machine', () => {
  describe('validateTransition', () => {
    it('should allow valid transitions', () => {
      expect(() =>
        validateTransition(
          SituacaoAverbacaoValues.AGUARDANDO_APROVACAO,
          SituacaoAverbacaoValues.APROVADA
        )
      ).not.toThrow()
    })

    it('should throw error for invalid transitions', () => {
      expect(() =>
        validateTransition(
          SituacaoAverbacaoValues.REJEITADA,
          SituacaoAverbacaoValues.APROVADA
        )
      ).toThrow(StateTransitionError)
    })
  })

  describe('isTransitionValid', () => {
    it('should return true for valid transitions', () => {
      expect(
        isTransitionValid(
          SituacaoAverbacaoValues.AGUARDANDO_APROVACAO,
          SituacaoAverbacaoValues.APROVADA
        )
      ).toBe(true)
    })

    it('should return false for invalid transitions', () => {
      expect(
        isTransitionValid(
          SituacaoAverbacaoValues.REJEITADA,
          SituacaoAverbacaoValues.APROVADA
        )
      ).toBe(false)
    })
  })

  describe('isEditableState', () => {
    it('should return true for editable states', () => {
      expect(isEditableState(SituacaoAverbacaoValues.AGUARDANDO_APROVACAO)).toBe(
        true
      )
    })

    it('should return false for non-editable states', () => {
      expect(isEditableState(SituacaoAverbacaoValues.APROVADA)).toBe(false)
      expect(isEditableState(SituacaoAverbacaoValues.REJEITADA)).toBe(false)
    })
  })

  describe('consumesMargem', () => {
    it('should return true for states that consume margin', () => {
      expect(consumesMargem(SituacaoAverbacaoValues.APROVADA)).toBe(true)
      expect(consumesMargem(SituacaoAverbacaoValues.ENVIADA)).toBe(true)
      expect(consumesMargem(SituacaoAverbacaoValues.DESCONTADA)).toBe(true)
    })

    it('should return false for states that do not consume margin', () => {
      expect(consumesMargem(SituacaoAverbacaoValues.REJEITADA)).toBe(false)
      expect(consumesMargem(SituacaoAverbacaoValues.CANCELADA)).toBe(false)
      expect(consumesMargem(SituacaoAverbacaoValues.LIQUIDADA)).toBe(false)
    })
  })

  describe('isTerminalState', () => {
    it('should return true for terminal states', () => {
      expect(isTerminalState(SituacaoAverbacaoValues.REJEITADA)).toBe(true)
      expect(isTerminalState(SituacaoAverbacaoValues.CANCELADA)).toBe(true)
      expect(isTerminalState(SituacaoAverbacaoValues.LIQUIDADA)).toBe(true)
    })

    it('should return false for non-terminal states', () => {
      expect(isTerminalState(SituacaoAverbacaoValues.APROVADA)).toBe(false)
      expect(isTerminalState(SituacaoAverbacaoValues.AGUARDANDO_APROVACAO)).toBe(
        false
      )
    })
  })

  describe('getTransitionActionName', () => {
    it('should return correct action names', () => {
      expect(getTransitionActionName(SituacaoAverbacaoValues.APROVADA)).toBe(
        'APROVAR'
      )
      expect(getTransitionActionName(SituacaoAverbacaoValues.REJEITADA)).toBe(
        'REJEITAR'
      )
      expect(getTransitionActionName(SituacaoAverbacaoValues.SUSPENSA)).toBe(
        'SUSPENDER'
      )
    })
  })

  describe('getAvailableActions', () => {
    it('should return actions for a state', () => {
      const actions = getAvailableActions(
        SituacaoAverbacaoValues.AGUARDANDO_APROVACAO
      )
      expect(actions).toHaveLength(3)
      expect(actions.map((a) => a.targetState)).toContain(
        SituacaoAverbacaoValues.APROVADA
      )
      expect(actions.map((a) => a.targetState)).toContain(
        SituacaoAverbacaoValues.REJEITADA
      )
      expect(actions.map((a) => a.targetState)).toContain(
        SituacaoAverbacaoValues.CANCELADA
      )
    })
  })

  describe('getStateMetadata', () => {
    it('should return metadata for all states', () => {
      const metadata = getStateMetadata()
      expect(Object.keys(metadata)).toHaveLength(9)
      expect(metadata[SituacaoAverbacaoValues.AGUARDANDO_APROVACAO]).toBeDefined()
    })
  })
})
