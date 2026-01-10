ALTER TRIGGER AtualizadorAcesso ON FastConsigItaqua.[dbo].Usuario 
AFTER UPDATE 
AS
IF TRIGGER_NESTLEVEL() > 1 RETURN
IF (UPDATE(APELIDOLOGIN) OR UPDATE(SENHA) OR UPDATE(SENHAPROVISORIA))
UPDATE FastConsigTemp.[dbo].Usuario
SET APELIDOLOGIN = baseAlterada.APELIDOLOGIN, 
SENHA = baseAlterada.SENHA, 
SENHAPROVISORIA = baseAlterada.SENHAPROVISORIA 
from INSERTED baseAlterada 
WHERE baseAlterada.CPF = FastConsigTemp.[dbo].Usuario.CPF