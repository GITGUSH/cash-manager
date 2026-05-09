-- Table: public.usuario
-- DROP TABLE IF EXISTS public.usuario;
CREATE TABLE IF NOT EXISTS public.usuario
(
    id_usuario integer NOT NULL DEFAULT nextval('usuario_id_usuario_seq'::regclass),
    nome character varying(50) COLLATE pg_catalog."default" NOT NULL,
    email character varying(100) COLLATE pg_catalog."default" NOT NULL,
    senha_hash text COLLATE pg_catalog."default" NOT NULL,
    data_inclusao timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT usuario_pkey PRIMARY KEY (id_usuario),
    CONSTRAINT usuario_email_key UNIQUE (email)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.usuario
    OWNER to postgres;



-- Table: public.operacao
-- DROP TABLE IF EXISTS public.operacao;

CREATE TABLE IF NOT EXISTS public.operacao
(
    id_operacao integer NOT NULL DEFAULT nextval('operacao_id_operacao_seq'::regclass),
    descricao character varying(1000) COLLATE pg_catalog."default" NOT NULL,
    valor numeric(15,2) NOT NULL DEFAULT 0.00,
    tipo_es character varying(1) COLLATE pg_catalog."default" NOT NULL,
    data_operacao timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_conta integer NOT NULL,
    id_usuario integer NOT NULL,
    id_categoria integer,
    CONSTRAINT operacao_pkey PRIMARY KEY (id_operacao),
    CONSTRAINT operacao_id_categoria_fkey FOREIGN KEY (id_categoria)
        REFERENCES public.categoria (id_categoria) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT operacao_id_conta_fkey FOREIGN KEY (id_conta)
        REFERENCES public.conta (id_conta) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT operacao_id_usuario_fkey FOREIGN KEY (id_usuario)
        REFERENCES public.usuario (id_usuario) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT operacao_tipo_es_check CHECK (tipo_es::text = ANY (ARRAY['E'::character varying, 'S'::character varying]::text[]))
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.operacao
    OWNER to postgres;




-- Table: public.conta
-- DROP TABLE IF EXISTS public.conta;

CREATE TABLE IF NOT EXISTS public.conta
(
    id_conta integer NOT NULL DEFAULT nextval('conta_id_conta_seq'::regclass),
    nome character varying(50) COLLATE pg_catalog."default" NOT NULL,
    id_usuario integer NOT NULL,
    saldo numeric(15,2) NOT NULL DEFAULT 0.00,
    data_inclusao timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT conta_pkey PRIMARY KEY (id_conta),
    CONSTRAINT conta_id_usuario_fkey FOREIGN KEY (id_usuario)
        REFERENCES public.usuario (id_usuario) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.conta
    OWNER to postgres;



-- Table: public.categoria
-- DROP TABLE IF EXISTS public.categoria;

CREATE TABLE IF NOT EXISTS public.categoria
(
    id_categoria integer NOT NULL DEFAULT nextval('categoria_id_categoria_seq'::regclass),
    nome character varying(50) COLLATE pg_catalog."default" NOT NULL,
    tipo_es character varying(1) COLLATE pg_catalog."default" NOT NULL,
    id_usuario integer NOT NULL,
    CONSTRAINT categoria_pkey PRIMARY KEY (id_categoria),
    CONSTRAINT categoria_id_usuario_fkey FOREIGN KEY (id_usuario)
        REFERENCES public.usuario (id_usuario) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT categoria_tipo_es_check CHECK (tipo_es::text = ANY (ARRAY['E'::character varying, 'S'::character varying]::text[]))
)

TABLESPACE pg_default;



--Altera a tabela de oeprações para o DELETE em cascata
ALTER TABLE IF EXISTS public.categoria
    OWNER to postgres;    


ALTER TABLE operacao
DROP CONSTRAINT operacao_id_conta_fkey;

ALTER TABLE operacao
ADD CONSTRAINT operacao_id_conta_fkey
FOREIGN KEY (id_conta)
REFERENCES conta(id_conta)
ON DELETE CASCADE;