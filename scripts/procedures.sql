-- PROCEDURE: public.inserir_usuario(character varying, character varying, text)
-- DROP PROCEDURE IF EXISTS public.inserir_usuario(character varying, character varying, text);

CREATE OR REPLACE PROCEDURE public.inserir_usuario(
	IN p_nome character varying,
	IN p_email character varying,
	IN p_senha_hash text)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
    INSERT INTO usuario (nome, email, senha_hash)
    VALUES (p_nome, p_email, p_senha_hash);
END;
$BODY$;
ALTER PROCEDURE public.inserir_usuario(character varying, character varying, text)
    OWNER TO postgres;



-- PROCEDURE: public.inserir_operacao(character varying, numeric, character varying, integer, integer, integer)
-- DROP PROCEDURE IF EXISTS public.inserir_operacao(character varying, numeric, character varying, integer, integer, integer);

CREATE OR REPLACE PROCEDURE public.inserir_operacao(
	IN p_descricao character varying,
	IN p_valor numeric,
	IN p_tipo_es character varying,
	IN p_id_conta integer,
	IN p_id_usuario integer,
	IN p_id_categoria integer)
LANGUAGE 'plpgsql'
AS $BODY$

DECLARE
V_SALDO_ATUAL NUMERIC;
V_SALDO_NOVO NUMERIC;

BEGIN

--PEGA O SALDO ATUAL DA CONTA
SELECT SALDO  
INTO V_SALDO_ATUAL 
FROM CONTA
WHERE ID_CONTA = P_ID_CONTA;

--CALCULA NOVO SALDO
IF P_TIPO_ES = 'E' THEN 
V_SALDO_NOVO := V_SALDO_ATUAL + P_VALOR;
ELSE
V_SALDO_NOVO := V_SALDO_ATUAL - P_VALOR;
END IF;

--INSERE A OPERAÇÃO
INSERT INTO OPERACAO(
DESCRICAO, 
VALOR, 
TIPO_ES, 
ID_CONTA, 
ID_USUARIO, 
ID_CATEGORIA
)
VALUES(
P_DESCRICAO,
P_VALOR,
P_TIPO_ES,
P_ID_CONTA,
P_ID_USUARIO,
P_ID_CATEGORIA
);

-- ATUALIZA SALDO DA CONTA

UPDATE CONTA SET SALDO = V_SALDO_NOVO
WHERE ID_CONTA = P_ID_CONTA;

END;
$BODY$;
ALTER PROCEDURE public.inserir_operacao(character varying, numeric, character varying, integer, integer, integer)
    OWNER TO postgres;




-- PROCEDURE: public.inserir_conta(character varying, integer, numeric)
-- DROP PROCEDURE IF EXISTS public.inserir_conta(character varying, integer, numeric);

CREATE OR REPLACE PROCEDURE public.inserir_conta(
	IN p_nome character varying,
	IN p_id_usuario integer,
	IN p_saldo_inicial numeric DEFAULT 0.00)
LANGUAGE 'plpgsql'
AS $BODY$

BEGIN

INSERT INTO CONTA (NOME, ID_USUARIO, SALDO)
VALUES (P_NOME, P_ID_USUARIO, P_SALDO_INICIAL);

END;
$BODY$;
ALTER PROCEDURE public.inserir_conta(character varying, integer, numeric)
    OWNER TO postgres;

