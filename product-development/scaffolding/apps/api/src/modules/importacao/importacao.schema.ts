import { z } from 'zod'

export const criarImportacaoSchema = z.object({
  tipo: z.enum(['FUNCIONARIOS', 'CONTRATOS', 'RETORNO_FOLHA']),
  nomeArquivo: z.string(),
  tamanhoBytes: z.number(),
})

export type CriarImportacaoInput = z.infer<typeof criarImportacaoSchema>
